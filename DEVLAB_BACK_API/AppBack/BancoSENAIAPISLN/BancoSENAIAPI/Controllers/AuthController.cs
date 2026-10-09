using BancoSENAIAPI.Data;
using BancoSENAIAPI.Dto;
using BancoSENAIAPI.Models;
using BancoSENAIAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public UsuarioController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        [AllowAnonymous]
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegisterRequestDto dto)
        {
            if (await _context.Usuario.AnyAsync(e => e.NomeUsuario == dto.NomeUsuario))
            {
                return BadRequest("Agencia já existte");
            }

            Usuario novoUsuario = new Usuario
            {
                NomeUsuario = dto.NomeUsuario,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            };

            _context.Add(novoUsuario);
            await _context.SaveChangesAsync();

            return Created("Usuario criado com sucesso", new { novoUsuario.Id, novoUsuario.NomeUsuario});
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.NomeUsuario == dto.NomeUsuario);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.SenhaHash))
            {
                return BadRequest("Usuario ou senha invalida");
            }

            var tokenJWT = _tokenService.GerarToken(usuario);

            return Ok(new LoginResponseDto { Token = tokenJWT.Token, ExpiraEm = tokenJWT.ExpiraEm }); ;
        }
    }
}