using System.Collections.Generic;
using IdentityServer4.Models;

namespace IdentityServer
{
    public static class Config
    {
        public static IList<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("api.admin"),
                new ApiScope("api.client")
            };
        }

        public static IList<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api", "My API")
                {
                    Enabled = true,
                    Scopes = {"api.admin", "api.client"},
                }
            };
        }

        public static IList<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client1",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequireClientSecret = false,
                    AllowedScopes = { "api.client" },
                },

                new Client
                {
                    ClientId = "client2",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "api.client" },
                },

                new Client
                {
                    ClientId = "client3",
                    AllowedGrantTypes = GrantTypes.Code, // authorization code
                    RequireClientSecret = false,
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api.client" }
                }
            };
        }
    }
}