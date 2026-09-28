using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.X86;

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

            long limiteMaximoBytes = 2 * 1024 * 1024;
            if (arquivo.Length > limiteMaximoBytes)
            {
                return BadRequest(new { erro = "R06F", mensagem = "O tamanho do arquivo excede o limite máximo permitido de 2 MB." });
            }

            string eXtensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
            string[] extensoesPermitidas = { ".pdf", ".jpg", ".png" };

            if (!extensoesPermitidas.Contains(eXtensao))
            {
                return BadRequest(new { erro = "R06G", mensagem = $"A extensão '{eXtensao}' não é permitida. Apenas arquivos .pdf, .jpg e .png são homologados." });
            }

            string pastaCliente = Path.Combine(_caminhoRaiz, codigoCliente.ToString());

            if (!Directory.Exists(pastaCliente))
            {
                Directory.CreateDirectory(pastaCliente);
            }

            string extensao = Path.GetExtension(arquivo.FileName);
            string nomeOriginal = Path.GetFileNameWithoutExtension(arquivo.FileName);
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

        [HttpGet("listar/{codigoCliente}")]
        public async Task<IActionResult> ListarDocumentos(int codigoCliente)
        {
            var documentos = _DocumentosMetadados.Where(d => d.CodigoCliente == codigoCliente).ToList();
            return Ok(documentos);
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> BaixarDocumentos(int id)
        {
            var documento = _DocumentosMetadados.FirstOrDefault(d => d.Id == id);
            if (documento == null)
                return NotFound();

            if (!System.IO.File.Exists(documento.Caminho))
                return NotFound("Arquivo físico não encontrado.");

            var fileBytes = await System.IO.File.ReadAllBytesAsync(documento.Caminho);
            var nomeArquivo = Path.GetFileName(documento.Caminho);

            return File(fileBytes, "application/octet-stream", nomeArquivo);
        }

        [HttpDelete("excluir/{id}")]
        public async Task<IActionResult> ExcluirDocumento(int id)
        {
            var documento = _DocumentosMetadados.FirstOrDefault(d => d.Id == id);
            if (documento == null)
                return NotFound();

            if (System.IO.File.Exists(documento.Caminho))
            {
                System.IO.File.Delete(documento.Caminho);
            }

            _DocumentosMetadados.Remove(documento);

            return Ok(new { mensagem = "Documento excluído com sucesso" });
        }

    }
}