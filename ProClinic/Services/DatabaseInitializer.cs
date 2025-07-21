using Microsoft.AspNetCore.Identity;
using ProClinic.Models;

namespace ProClinic.Services
{
  
        public class DatabaseInitializer
        {
            public static async Task SeedDataAsync(UserManager<ApplicationUser>? userManager,
            RoleManager<IdentityRole>? roleManager)
            {
                if (userManager == null || roleManager == null)
                {
                    Console.WriteLine("userManager or roleManager is null => exit");
                    return;
                }

                // check we have the admin role or not
                var exists = await roleManager.RoleExistsAsync("admin");
                if (!exists)
                {
                    Console.WriteLine("Admin role is not defined and will be created");
                    await roleManager.CreateAsync(new IdentityRole("admin"));
                }

                // check we have the seller role or not
                exists = await roleManager.RoleExistsAsync("seller");
                if (!exists)
                {
                    Console.WriteLine("Seller role is not defined and will be created");
                    await roleManager.CreateAsync(new IdentityRole("seller"));
                }

                // check we have the seller role or not
                exists = await roleManager.RoleExistsAsync("client");
                if (!exists)
                {
                    Console.WriteLine("Client role is not defined and will be created");
                    await roleManager.CreateAsync(new IdentityRole("client"));
                }

                // check if we have at least one admin user or not
                var adminUser = await userManager.GetUsersInRoleAsync("admin");
                // if the admin user already exits then exit 
                if (adminUser.Any())
                {
                    Console.WriteLine("Admin user already exists => exit");
                    return;
                }

                // If adminuser doesn't exists then create an admin user
                var user = new ApplicationUser()
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    CreatedAt = DateTime.Now.ToString(),
                };

                string initialPassword = "admin123";

                var result = await userManager.CreateAsync(user, initialPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "admin");
                    Console.WriteLine("Admin user created successfully");
                    Console.WriteLine("Email: " + user.Email);
                    Console.WriteLine("Initial password: " + initialPassword);
                }


            }
        }
    }

