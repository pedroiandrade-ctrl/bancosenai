using Microsoft.AspNetCore.Mvc;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DocumentoController : Controller
    {
        private readonly string _caminhoRaiz = Path.Combine(
            Directory.GetCurrentDirectory(), "ClienteArquivos"
            );

        private static List<Models.DocumentoMetadados> _DocumentosMetadados = new List<Models.DocumentoMetadados>();

        private static int _nextid = 1;

        [HttpPost("upload/{codigoCliente}")]
        public async Task<IActionResult> AnexarArquivo(int codigoCliente, IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0) 
            {
                return BadRequest("Nenhum arquivo foi enviado.");
            }

            string pastaCliente = Path.Combine(_caminhoRaiz, codigoCliente.ToString());

            if(!Directory.Exists(pastaCliente))
            {
                Directory.CreateDirectory(pastaCliente);
            }

            string extensao = Path.GetExtension(arquivo.FileName);
            string nomeOriginal = Path. GetFileNameWithoutExtension(arquivo.FileName);
            string novoNome = $"{codigoCliente}_{nomeOriginal}_{Guid.NewGuid()}{extensao}";
            string caminhoFInal = Path.Combine(pastaCliente, novoNome);

            using (var stream = new FileStream(caminhoFInal, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            var documentoMetadados = new Models.DocumentoMetadados
            {
                Id = _nextid++,
                Name = nomeOriginal,
                Extensao = extensao,
                Caminho = caminhoFInal,
                CodigoCliente = codigoCliente
            };

            _DocumentosMetadados.Add(documentoMetadados);

            return Ok(new { mensagem = "Documento anexado com sucesso", arquivoSalvo = novoNome });
        }
    }
}
