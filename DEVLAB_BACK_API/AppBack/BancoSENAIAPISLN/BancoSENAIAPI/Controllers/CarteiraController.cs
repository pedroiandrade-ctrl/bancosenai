using Microsoft.AspNetCore.Mvc;

namespace BancoSENAIAPI.Controllers
{
    public class CarteiraController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
