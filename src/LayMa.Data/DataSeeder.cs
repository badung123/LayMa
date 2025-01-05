using LayMa.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using LayMa.Core.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using LayMa.Core.Utilities;

namespace LayMa.Data
{
    public class DataSeeder
    {
        public async Task SeedAsync(LayMaContext context)
        {
            var passwordHasher = new PasswordHasher<AppUser>();

            var rootAdminRoleId = Guid.NewGuid();
            if (!context.Roles.Any())
            {
                await context.Roles.AddAsync(new IdentityRole<Guid>()
                {
                    Id = rootAdminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                });
                await context.SaveChangesAsync();
            }

            if (!context.Users.Any())
            {
                var userId = Guid.NewGuid();
                var user = new AppUser()
                {
                    Id = userId,
                    Email = "admin@layma.net",
                    NormalizedEmail = "ADMIN@LAYMA.NET",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    IsActive = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    LockoutEnabled = false,
                    DateCreated = DateTime.Now
                };
                user.PasswordHash = passwordHasher.HashPassword(user, "Zxc248@@");
                await context.Users.AddAsync(user);

                await context.UserRoles.AddAsync(new IdentityUserRole<Guid>()
                {
                    RoleId = rootAdminRoleId,
                    UserId = userId,
                });
                await context.SaveChangesAsync();
            }
            var listUserNotHaveApiUserToken = await context.Users.Where(x=>x.ApiUserToken == null || x.ApiUserToken == "").ToListAsync();
            foreach (var item in listUserNotHaveApiUserToken)
            {
                var md5content = item.UserName + "_" + item.Email;
                var apiUserToken = Helper.MD5(md5content);
                item.ApiUserToken = apiUserToken;
                context.Users.Update(item);
            }
            await context.SaveChangesAsync();

        }
    }
}