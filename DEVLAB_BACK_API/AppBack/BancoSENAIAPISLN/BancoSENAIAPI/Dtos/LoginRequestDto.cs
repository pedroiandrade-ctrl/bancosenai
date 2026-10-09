using System.ComponentModel.DataAnnotations;

namespace BancoSENAIAPI.Dto
{
    public class LoginRequestDto
    {
        [Required]
        public required string NomeUsuario { get; set; }


        [Required]
        public string Password { get; set; }
    }
}