using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Examen.Elipgo.DAL.Constants;
using Microsoft.AspNetCore.Identity;

namespace Examen.Elipgo.DAL.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedBasicUserAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new IdentityUser
            {
                UserName = "Basic",
                Email = "basicuser@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "basic123");
                    var result = await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                    await roleManager.SeedClaimsForUser(Roles.Basic.ToString(), "Products", new[] { PermissionsAccess.Create, PermissionsAccess.View });
                }
            }
        }

        public static async Task SeedAdminUserAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new IdentityUser
            {
                UserName = "Admin",
                Email = "adminuser@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "admin123");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await roleManager.SeedClaimsForUser(Roles.Admin.ToString(), "Products", null, true);
                }
            }
        }

        private static async Task SeedClaimsForUser(this RoleManager<IdentityRole> roleManager, string roleName, string module, PermissionsAccess[] permissions, bool allPermissions = false)
        {
            var adminRole = await roleManager.FindByNameAsync(roleName);
            await roleManager.AddPermissionClaim(adminRole, module, permissions, allPermissions);
        }

        public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module, PermissionsAccess[] permissions, bool allPermissions)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var permissionsAccesses = allPermissions? Enum.GetValues(typeof(PermissionsAccess)).Cast<PermissionsAccess>().ToArray() : permissions;

            foreach (var permission in permissionsAccesses)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission.ToString()))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission.ToString()));
                }
            }
        }

        
    }
}
