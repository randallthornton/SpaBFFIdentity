using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Serilog;
using Yarp.ReverseProxy.Transforms;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting web host");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
        .AddTransforms(builderContext =>
        {
            builderContext.AddRequestTransform(async context =>
            {
                if (context.HttpContext.User.Identity?.IsAuthenticated == true)
                {
                    var accessToken = await context.HttpContext.GetTokenAsync("access_token");
                    context.ProxyRequest.Headers.Add("Authorization", $"Bearer {accessToken}");
                }
            });
        });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddAuthentication(opts =>
    {
        opts.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        opts.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        opts.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
        .AddCookie(opts =>
        {
            opts.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            opts.Cookie.SameSite = SameSiteMode.Strict;
            opts.Cookie.HttpOnly = true;
        })
        .AddOpenIdConnect("oidc", opts =>
        {
            opts.Authority = builder.Configuration["Identity:Authority"];
            opts.ClientId = builder.Configuration["Identity:ClientId"];
            opts.ClientSecret = builder.Configuration["Identity:ClientSecret"];
            opts.ResponseType = OpenIdConnectResponseType.CodeIdToken;
            opts.ResponseMode = OpenIdConnectResponseMode.FormPost;

            opts.Scope.Add("openid");
            opts.Scope.Add("offline_access");

            opts.SaveTokens = true;
            opts.GetClaimsFromUserInfoEndpoint = true;

            opts.UseTokenLifetime = true;

            opts.Resource = builder.Configuration["Identity:Audience"];
        });

    builder.Services.AddAuthorization(opts =>
    {
        opts.AddPolicy("RequireAuthenticatedUserPolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
        });
    });

    builder.Services.AddControllersWithViews();

    builder.Services.AddHttpClient();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllerRoute(name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapReverseProxy();

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

