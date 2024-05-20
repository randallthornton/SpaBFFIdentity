using IdentityServer4.Models;
using IdentityServer4;

namespace IdentityServer
{
	public static class Config
	{
		public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
		{
			new ApiScope("ResourceApi", "My API"),
		};

		public static IEnumerable<Client> Clients => new List<Client>
		{
			new Client
			{
				ClientId = "bff",
				ClientSecrets = new List<Secret>()
				{
					new Secret("bffsecret".Sha256())
				},
				AllowedGrantTypes = new List<string>() { GrantType.AuthorizationCode, GrantType.ClientCredentials},
				RedirectUris = new List<string>()
				{
					"https://localhost:7027/signin-oidc"
				},
				PostLogoutRedirectUris =new List <string>()
				{
					"https://localhost:7027/signout-callback-oidc"
				},
				AllowedScopes = new List<string>()
				{
					"ResourceApi",
					IdentityServerConstants.StandardScopes.OpenId,
					IdentityServerConstants.StandardScopes.Profile,
					IdentityServerConstants.StandardScopes.OfflineAccess,
				},
				RequirePkce = true,
				AllowOfflineAccess = true,
			}
		};

		public static IEnumerable<IdentityResource> IdentityResources =>
			new List<IdentityResource>
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Profile(),
			};
	}
}
