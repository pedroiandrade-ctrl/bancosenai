using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClienteController : ControllerBase
    {
     

        [HttpGet]
        public IActionResult ListarTodos()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Cadastrar([FromBody] Cliente cliente)
        {
            //var novoCliente = _service.Cadastrar(cliente);

            return Created("", cliente);
        }

        [HttpGet("{codigo}")]
        public IActionResult BuscarPorCodigo(int codigo)
        {
            //var cliente = _service.BuscarPorCodigo(codigo);

            /*if (cliente == null)
            {
                return NotFound(new { message = "Cliente não encontrado." });
            }*/

            return Ok();
        }
    }
}