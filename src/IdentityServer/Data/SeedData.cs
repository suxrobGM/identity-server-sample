using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer.Models;

namespace IdentityServer.Data
{
    public static class SeedData
    {
        public static async void Initialize(IServiceProvider service)
        {
            await CreateUsers(service);
        }

        private static async Task CreateUsers(IServiceProvider service)
        {
            try
            {
                var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
                var users = new List<ApplicationUser>()
                {
                    new ApplicationUser()
                    {
                        UserName = "TestUser1",
                        Email = "TestUser1@mail.ru",
                    },

                    new ApplicationUser()
                    {
                        UserName = "TestUser2",
                        Email = "TestUser2@mail.ru",
                    },

                    new ApplicationUser()
                    {
                        UserName = "TestUser3",
                        Email = "TestUser3@mail.ru",
                    }
                };

                foreach (var user in users)
                {
                    var existedUser = await userManager.FindByNameAsync(user.UserName);
                    if (existedUser == null)
                    {
                        await userManager.CreateAsync(user, "Password1#");
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}