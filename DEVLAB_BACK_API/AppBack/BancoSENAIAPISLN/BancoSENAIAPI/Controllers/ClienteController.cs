using BancoSENAIAPI.Data;
using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClienteController : ControllerBase
    {

        private readonly AppDbContext _Context;

        public ClienteController(AppDbContext context)
        {
            _Context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var Cliente = await _Context.Cliente.ToListAsync();
            return Ok(Cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] Cliente cliente)
        {
            
            _Context.Cliente.Add(cliente);
            await _Context.SaveChangesAsync();
            return Created("", cliente);
        }

        [HttpGet("{codigo}")]
        public async Task <IActionResult> BuscarPorCodigo(int codigo)
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