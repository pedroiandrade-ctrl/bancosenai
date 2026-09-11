using Microsoft.AspNetCore.Mvc;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DocumentoController : Controller
    {
        private readonly string _caminhoRaiz = Path.Combine(
            Directory.GetCurrentDIrectory(), "ClienteArquivos"
            );

        private static List<Models.DocumentoMetadados> _DocumentosMetadados = new List<Models.DocumentoMetadados>();

        private static int _nextid = 1;

        [HttpPost("upload/{codigoCliente}")]
        public async Task<IActionResult> AnexarArquivo(int codigoCliente, IFormFile arquivo)
        {

        }
    }
}
