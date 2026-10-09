namespace BancoSENAIAPI.Dto
{
    public class LoginResponseDto
    {
        public required string Token { get; set; }
        public required DateTime ExpiraEm { get; set; }
    }
}