using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Examen.Elipgo.DAO.Interfaces;
using Examen.Elipgo.DAO.Models;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Examen.Elipgo.Service.Controllers
{
    [Route("service/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreRepository _storeRepository;

        public StoreController(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        // GET: api/<ArticleController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var response = await _storeRepository.Get();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(new { success = response.Success, STORES = response.Value, total_elements = response.Value.Count() });
            }
            else
            {
                return StatusCode((int)response.StatusCode,
                    new { success = response.Success, error_code = (int)response.StatusCode, message_error = response.Message });
            }
        }

        // GET api/<ArticleController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _storeRepository.Get(id);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(new { success = response.Success, STORES = response.Value });
            }
            else
            {
                return StatusCode((int)response.StatusCode,
                    new { success = response.Success, error_code = (int)response.StatusCode, message_error = response.Message });
            }
        }

        // POST api/<ArticleController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] StoreDAO articleDao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _storeRepository.Post(articleDao);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(new { success = response.Success, status_code = response.StatusCode, message = response.Message });
            }
            else
            {
                return StatusCode((int)response.StatusCode,
                    new { success = response.Success, tatusCode = (int)response.StatusCode, message_error = response.Message });
            }
        }

        // PUT api/<ArticleController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] StoreDAO articleDao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != articleDao.Id)
            {
                return BadRequest(new { success = false, error_code = 400, message_error = "El articulo no coincide con la información" });
            }

            var response = await _storeRepository.Put(articleDao);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(new { success = response.Success, status_code = response.StatusCode, message = response.Message });
            }
            else
            {
                return StatusCode((int)response.StatusCode,
                    new { success = response.Success, error_code = (int)response.StatusCode, message_error = response.Message });
            }
        }

        // DELETE api/<ArticleController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _storeRepository.Delete(id);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(new { success = response.Success });
            }
            else
            {
                return StatusCode((int)response.StatusCode,
                    new { success = response.Success, error_code = (int)response.StatusCode, message_error = response.Message });
            }
        }
    }
}
