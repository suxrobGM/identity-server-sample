using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data
{
    public static class SeedData
    {
        public static async void Initialize(IServiceProvider service)
        {
            await MigrateDatabaseAsync(service);
            await CreateUsers(service);
        }
        
        private static async Task MigrateDatabaseAsync(IServiceProvider serviceProvider)
        {
            try
            {
                var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.Database.MigrateAsync();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static async Task CreateUsers(IServiceProvider service)
        {
            try
            {
                var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
                var users = new List<ApplicationUser>()
                {
                    new() {
                        UserName = "TestUser1",
                        Email = "TestUser1@mail.ru",
                    },

                    new() {
                        UserName = "TestUser2",
                        Email = "TestUser2@mail.ru",
                    },

                    new() {
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