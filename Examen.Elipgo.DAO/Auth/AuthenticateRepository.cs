using Examen.Elipgo.DAL.Models;
using Examen.Elipgo.DAO.Interfaces;
using Examen.Elipgo.DAO.Models;
using Examen.Elipgo.DAO.Response;
using Examen.Elipgo.DAO.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Examen.Elipgo.DAO.Auth
{
    public class AuthenticateRepository : IAuthenticateRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppIdentitySettings _identitySettings;
        private readonly string _issuerSigningKey;

        public AuthenticateRepository(UserManager<IdentityUser> userManager, IOptions<AppIdentitySettings> identitySettings, RoleManager<IdentityRole> roleManager, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            this._userManager = userManager;
            _roleManager = roleManager;
            this._identitySettings = identitySettings.Value;
            this._issuerSigningKey = configuration.GetValue<string>("AppSettings:IssuerSigningKey");
        }

        public async Task<StatusResponse<object>> Login(LoginDAO model)
        {
            var response = new StatusResponse<object>();
            var listRoles = new List<string>();
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = "Usuario o contraseña incorrectos. Favor de verificar";
                return response;
            }

            if (_userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(user))
            {
                var LockEnd = await _userManager.GetLockoutEndDateAsync(user);
                response.StatusCode = System.Net.HttpStatusCode.Conflict;
                response.Message =
                    $"La cuenta se bloqueó temporalmente por seguridad. Intente dentro de {Math.Round((LockEnd.Value - DateTimeOffset.Now).TotalMinutes)} minutos";
                return response;
            }

            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                if (_userManager.SupportsUserLockout && await _userManager.GetAccessFailedCountAsync(user) > 0)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);
                }
                else
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, model.Username),
                        new Claim(JwtRegisteredClaimNames.UniqueName, $"{user.Email}"),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                    };

                    foreach (var item in await _userManager.GetRolesAsync(user))
                    {
                        claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, item));
                        listRoles.Add(item);
                    }

                    var allPermissions = new List<RoleClaimsDAO>();

                    foreach (var role in await _userManager.GetRolesAsync(user))
                    {
                        var a = await _roleManager.GetClaimsAsync(await _roleManager.FindByNameAsync(role));
                        allPermissions.AddRange(a.Select(claim => new RoleClaimsDAO() {Type = claim.Type, Value = claim.Value}));
                    }

                    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_issuerSigningKey));
                    var token = new JwtSecurityToken(
                        expires: DateTime.UtcNow.ToLocalTime().AddHours(3),
                        claims: claims,
                        signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                    );
                    return new StatusResponse<object>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Autentificación Satisfactoria",
                        Success = true,
                        Value = new
                        {
                            UserName = user.UserName,
                            Email = user.Email,
                            expiration = token.ValidTo.ToLocalTime(),
                            RoleName = listRoles,
                            Permissions = allPermissions,
                            token = new JwtSecurityTokenHandler().WriteToken(token)
                        }
                    };
                }
            }
            else
            {
                if (_userManager.SupportsUserLockout && await _userManager.GetLockoutEnabledAsync(user))
                {

                    if (await _userManager.GetAccessFailedCountAsync(user) >= 4)
                    {
                        await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.AddMinutes(_identitySettings.Lockout.DefaultLockoutTimeSpanInMins));
                        await _userManager.ResetAccessFailedCountAsync(user);
                        response.StatusCode = System.Net.HttpStatusCode.Conflict;
                        response.Message =
                            $"Su cuenta ha sido bloqueada termporalmente. Intente despues de {_identitySettings.Lockout.DefaultLockoutTimeSpanInMins} minutos";
                        return response;
                    }
                    else
                    {
                        await _userManager.AccessFailedAsync(user);
                        response.StatusCode = System.Net.HttpStatusCode.Conflict;
                        response.Message =
                            $"Solo quedan {(_identitySettings.Lockout.MaxFailedAccessAttempts - await _userManager.GetAccessFailedCountAsync(user))} intentos antes de bloquear la cuenta";
                        return response;

                    }
                }
            }

            return new StatusResponse<object>()
            {
                Success = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "Problemas con el servidor intente mas tarde",
                Value = null
            };
        }
    }
}
