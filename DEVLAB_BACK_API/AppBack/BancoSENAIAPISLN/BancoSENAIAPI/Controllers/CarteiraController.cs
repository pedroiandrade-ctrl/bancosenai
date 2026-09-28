using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;
using BancoSENAIAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CarteiraController : ControllerBase
    {
        private readonly AppDbContext _Context;

        public CarteiraController(AppDbContext context) 
        { 
            _Context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var carteiras = await _Context.Carteira.ToListAsync();
            return Ok(carteiras);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] Carteira novaCarteira)
        {
            if (await _Context.Carteira.AnyAsync(c => c.NumeroCarteira == novaCarteira.NumeroCarteira))
            {
                return BadRequest(new { message = "Este número de carteira já existe." });
            }

            if (novaCarteira.ApetiteCarteira < 0)
            {
                return BadRequest(new { message = "O apetite da carteira não pode ser negativo." });
            }

            _Context.Carteira.Add(novaCarteira);
            await _Context.SaveChangesAsync();
            return Created("", novaCarteira);
        }
        [HttpPut("{numero}")]
        public async Task<IActionResult> Atualizar(int numero, [FromBody] Carteira carteiraAtualizada)
        {
            var carteira = await _Context.Carteira.FirstOrDefaultAsync(c => c.NumeroCarteira == numero);

            if (carteira == null)
            {
                return NotFound(new { message = "Carteira não encontrada." });
            }

            if (carteiraAtualizada.ApetiteCarteira < 0)
            {
                return BadRequest(new { message = "O apetite da carteira não pode ser negativo." });
            }

            carteira.NomeCarteira = carteiraAtualizada.NomeCarteira;
            carteira.ApetiteCarteira = carteiraAtualizada.ApetiteCarteira;

            await _Context.SaveChangesAsync();
            return Ok(carteira);
        }
        [HttpDelete("{numero}")]
        public async Task<IActionResult> Apagar(int numero)
        {
            var carteira = await _Context.Carteira.FirstOrDefaultAsync(c => c.NumeroCarteira == numero);

            if (carteira == null)
            {
                return NotFound(new { message = "Carteira não encontrada." });
            }

             _Context.Carteira.Remove(carteira);

            await _Context.SaveChangesAsync();
            return Ok(new { message = "Carteira apagada com sucesso." });
        }
    }

}