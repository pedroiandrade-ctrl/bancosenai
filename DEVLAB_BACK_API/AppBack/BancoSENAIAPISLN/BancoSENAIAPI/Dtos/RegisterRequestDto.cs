using System.ComponentModel.DataAnnotations;

namespace BancoSENAIAPI.Dto
{
    public class RegisterRequestDto
    {
        [Required]
        public required string NomeUsuario { get; set; }

        [Required]
        [StringLength(6)]
        public required string Senha { get; set; }
    }
}