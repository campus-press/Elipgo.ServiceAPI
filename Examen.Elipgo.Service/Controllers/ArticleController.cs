using Examen.Elipgo.DAO.Interfaces;
using Examen.Elipgo.DAO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Examen.Elipgo.Service.Controllers
{
    [Route("service/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        // GET: api/<ArticleController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _articleRepository.Get();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(new { success = response.Success, ARTICLES = response.Value, total_elements = response.Value.Count() });
            }
            else
            {
                return StatusCode((int) response.StatusCode,
                    new {success = response.Success, error_code = (int) response.StatusCode, message_error = response.Message });
            }
        }

        [HttpGet("SearchArticleByFilter")]
        public async Task<IActionResult> Get([FromQuery] string Name, [FromQuery] string Store, [FromQuery] int Id)
        {
            var request = HttpContext.Request;
            var listStings = request.Query.Select(item => new KeyValuePair<string, string>(item.Key, item.Value)).ToList();
            if (listStings.Count == 0)
            {
                return BadRequest(new { success = false, error_code = 400, message_error = "Debe Ingresar un filtro para procesar su solicitud" });
            }
            var response = await _articleRepository.Get(listStings);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(new { success = response.Success, ARTICLES = response.Value, total_elements = response.Value.Count() });
            }
            else
            {
                return StatusCode((int)response.StatusCode,
                    new { success = response.Success, error_code = (int)response.StatusCode, message_error = response.Message });
            }
        }

        // GET api/<ArticleController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _articleRepository.Get(id);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(new { success = response.Success, ARTICLE = response.Value, total_elements = 1 });
            }
            else
            {
                return StatusCode((int)response.StatusCode,
                    new { success = response.Success, error_code = (int)response.StatusCode, message_error = response.Message });
            }
        }
        
        
        [HttpGet("stores/{id}")]
        public async Task<IActionResult> GetArticlesByStore(string id)
        {
            int idParse;
            if (!Int32.TryParse(id, out idParse))
            {
                return StatusCode(400,
                    new { success = false, error_code = 400, message_error = "Bad Request" });
            }
            else
            {
                var response = await _articleRepository.GetArticlesByStore(idParse);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Ok(new { success = response.Success, ARTICLES = response.Value, total_elements = response.Value.Count() });
                }
                else
                {
                    return StatusCode((int)response.StatusCode,
                        new { success = response.Success, error_code = (int)response.StatusCode, message_error = response.Message });
                }
            }
        }

        // POST api/<ArticleController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] ArticleDAO articleDao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _articleRepository.Post(articleDao);
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

        // PUT api/<ArticleController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] ArticleDAO articleDao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != articleDao.Id)
            {
                return BadRequest(new { success = false, error_code = 400, message_error = "El articulo no coincide con la información" });
            }

            var response = await _articleRepository.Put(articleDao);
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

        // DELETE api/<ArticleController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _articleRepository.Delete(id);
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
