using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using Serilog;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();


try
{
    Log.Information("Starting web host");


    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddControllersWithViews();

    builder.Services.AddRazorPages();

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseInMemoryDatabase("Identity");

        options.UseOpenIddict();
    });

    builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts =>
    {

    })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders()
        .AddDefaultUI();

    builder.Services.AddOpenIddict()
        .AddCore(options =>
        {
            options.UseEntityFrameworkCore()
                .UseDbContext<ApplicationDbContext>();
        })
        .AddClient(options =>
        {
            options.AllowAuthorizationCodeFlow();


        })
        .AddServer(options =>
        {
            options.SetAuthorizationEndpointUris("connect/authorize")
                .SetLogoutEndpointUris("connect/logout")
                .SetTokenEndpointUris("connect/token")
                .SetUserinfoEndpointUris("connect/userinfo");

            options.AllowAuthorizationCodeFlow()
                .AllowClientCredentialsFlow()
                .AllowRefreshTokenFlow();

            options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);

            options.AddDevelopmentEncryptionCertificate()
                .AddDevelopmentSigningCertificate();

            options.UseAspNetCore()
                .EnableAuthorizationEndpointPassthrough()
                .EnableLogoutEndpointPassthrough()
                .EnableTokenEndpointPassthrough()
                .EnableUserinfoEndpointPassthrough()
                .EnableStatusCodePagesIntegration();
        })
        .AddValidation(options =>
        {
            options.UseLocalServer();
            options.UseAspNetCore();
        });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins", builder =>
        {
            builder.AllowCredentials()
            .WithOrigins("https://localhost:4200")
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
    });

    var app = builder.Build();

    // Seed users
    await AddClients(app);

    await SeedDatabase(app);

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapDefaultControllerRoute();
    app.MapRazorPages();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

async Task SeedDatabase(IApplicationBuilder app)
{
    using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
    {
        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        var alice = await userMgr.FindByNameAsync("alice");
        if (alice == null)
        {
            alice = new IdentityUser
            {
                UserName = "alice",
                Email = "AliceSmith@email.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                AccessFailedCount = 0,
            };
            var result = await userMgr.CreateAsync(alice, "Password123!");
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = await userMgr.AddClaimsAsync(alice, new Claim[]{
                            new Claim(Claims.Name, "Alice Smith"),
                            new Claim(Claims.GivenName, "Alice"),
                            new Claim(Claims.FamilyName, "Smith"),
                            new Claim(Claims.Website, "http://alice.com"),
                        });
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            logger.LogDebug("alice created");
        }
        else
        {
            logger.LogDebug("alice already exists");
        }

        var bob = await userMgr.FindByNameAsync("bob");
        if (bob == null)
        {
            bob = new IdentityUser
            {
                UserName = "bob",
                Email = "BobSmith@email.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
            };
            var result = await userMgr.CreateAsync(bob, "Password123!");
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = await userMgr.AddClaimsAsync(bob,
            [
                new Claim(Claims.Name, "Bob Smith"),
                new Claim(Claims.GivenName, "Bob"),
                new Claim(Claims.FamilyName, "Smith"),
                new Claim(Claims.Website, "http://bob.com"),
                new Claim("location", "somewhere")
            ]);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            logger.LogDebug("bob created");
        }
        else
        {
            logger.LogDebug("bob already exists");
        }
    }
}

async Task AddClients(IApplicationBuilder app)
{
    await using var scope = app.ApplicationServices.CreateAsyncScope();

    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.EnsureCreatedAsync();

    var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

    if (await manager.FindByClientIdAsync("balosar-blazor-client") is null)
    {
        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = "bff",
            ClientSecret = "bffsecret",
            ConsentType = ConsentTypes.Explicit,
            DisplayName = "BFF client application",
            ClientType = ClientTypes.Confidential,
            PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:7027/signout-oidc")
                },
            RedirectUris =
                {
                    new Uri("https://localhost:7027/signin-oidc")
                },
            Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Logout,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles
                },
            Requirements =
                {
                    Requirements.Features.ProofKeyForCodeExchange
                }
        });
    }

}
