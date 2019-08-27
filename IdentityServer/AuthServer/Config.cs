using IdentityServer4.Models;
using System.Collections.Generic;

namespace AuthServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("resourceapi", "Resource API")
                {
                    Scopes = {new Scope("api.scope") }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client {
                    RequireConsent = false,
                    ClientId = "ng_client_1",
                    ClientName = "Angular Application",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = { "openid", "profile", "email", "api.scope" },
                    RedirectUris = {"http://localhost:4200/auth-callback"},
                    PostLogoutRedirectUris = {"http://localhost:4200/"},
                    AllowedCorsOrigins = {"http://localhost:4200"},
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600
                },
                new Client {
                    RequireConsent = false,
                    ClientId = "ng_client_prod_1",
                    ClientName = "Angular Application (Production)",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = { "openid", "profile", "email", "api.scope" },
                    RedirectUris = {"http://homeserver/HomeApp/auth-callback"},
                    PostLogoutRedirectUris = {"http://homeserver/HomeApp/"},
                    AllowedCorsOrigins = {"http://homeserver"},
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600
                },
                new Client {
                    RequireConsent = false,
                    ClientId = "ng_client_2",
                    ClientName = "Angular Application (MacOS)",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = { "openid", "profile", "email", "api.scope" },
                    RedirectUris = {"http://maclocalhost:4200/auth-callback"},
                    PostLogoutRedirectUris = {"http://maclocalhost:4200/"},
                    AllowedCorsOrigins = {"http://maclocalhost:4200"},
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600
                },
            };
        }
    }
}
