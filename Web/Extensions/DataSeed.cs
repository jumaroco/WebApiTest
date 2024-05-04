using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Models;

namespace Web.Extensions;

public static class DataSeed
{
    public static async Task SeedDataAuthentication(
        this IApplicationBuilder app
    ) 
    {
        using var scope = app.ApplicationServices.CreateScope();
        var service = scope.ServiceProvider;
        var loggerFactory = service.GetRequiredService<ILoggerFactory>();

        try
        {
            var context = service.GetRequiredService<TestDbContext>();
            await context.Database.MigrateAsync();
            var userManager = service.GetRequiredService<UserManager<AppUser>>();
           

            if(!userManager.Users.Any()) 
            {
                var userAdmin = new AppUser
                {
                    NombreCompleto = "Juan Perez",
                    UserName = "jperez",
                    Email = "jperez@gmail.com"
                };

                await userManager.CreateAsync(userAdmin, "Admin123$" );
                await userManager.AddToRoleAsync(userAdmin, CustomRoles.ADMIN);

                var userClient = new AppUser
                {
                    NombreCompleto = "Mario Fernandez",
                    UserName = "mfernandez",
                    Email = "mfernandez@gmail.com"
                };
                
                await userManager.CreateAsync(userClient, "Client123$");
                await userManager.AddToRoleAsync(userClient, CustomRoles.CLIENT);
            }
        }
        catch (Exception e)
        {

            var logger = loggerFactory.CreateLogger<TestDbContext>();
            logger.LogError(e.Message);
        }
    }
}
