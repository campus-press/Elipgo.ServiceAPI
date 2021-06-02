using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Examen.Elipgo.DAO.Interfaces;
using Examen.Elipgo.DAO.Models;

namespace Examen.Elipgo.Service.Controllers
{
    [Route("service/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateRepository _authenticateRepository;

        public AuthenticateController(IAuthenticateRepository authenticateRepository)
        {
            _authenticateRepository = authenticateRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginDAO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _authenticateRepository.Login(model);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(new { success = response.Success, User = response.Value, Message = "Inicio sesión satisfactoriamente" });
            }
            else
            {
                return StatusCode((int)response.StatusCode,
                    new { success = response.Success, error_code = (int)response.StatusCode, message_error = response.Message });
            }
        }
    }
}
