using IdentityModel;
using IdentityServer;
using IdentityServer.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents()
	.AddInteractiveWebAssemblyComponents();

builder.Services.AddDbContext<AppIdentityDbContext>(opts =>
{
	opts.UseInMemoryDatabase("Identity").EnableSensitiveDataLogging();
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
	.AddEntityFrameworkStores<AppIdentityDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddIdentityServer(opts =>
{
	opts.Events.RaiseErrorEvents = true;
	opts.Events.RaiseInformationEvents = true;
	opts.Events.RaiseFailureEvents = true;
	opts.Events.RaiseSuccessEvents = true;
})
	.AddDeveloperSigningCredential()
	.AddInMemoryIdentityResources(Config.IdentityResources)
	.AddInMemoryApiScopes(Config.ApiScopes)
	.AddInMemoryClients(Config.Clients)
	.AddAspNetIdentity<IdentityUser>();

var app = builder.Build();

await SeedDatabase(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseIdentityServer();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode()
	.AddInteractiveWebAssemblyRenderMode();

app.Run();

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
			};
			var result = await userMgr.CreateAsync(alice, "Password123!");
			if (!result.Succeeded)
			{
				throw new Exception(result.Errors.First().Description);
			}

			result = await userMgr.AddClaimsAsync(alice, new Claim[]{
							new Claim(JwtClaimTypes.Name, "Alice Smith"),
							new Claim(JwtClaimTypes.GivenName, "Alice"),
							new Claim(JwtClaimTypes.FamilyName, "Smith"),
							new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
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

			result = await userMgr.AddClaimsAsync(bob, new Claim[]
			{
				new Claim(JwtClaimTypes.Name, "Bob Smith"),
				new Claim(JwtClaimTypes.GivenName, "Bob"),
				new Claim(JwtClaimTypes.FamilyName, "Smith"),
				new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
				new Claim("location", "somewhere")
			});
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
