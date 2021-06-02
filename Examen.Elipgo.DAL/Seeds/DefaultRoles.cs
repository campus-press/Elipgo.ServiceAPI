using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Examen.Elipgo.DAL.Constants;
using Examen.Elipgo.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace Examen.Elipgo.DAL.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
        }
    }
}
