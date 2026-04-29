using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {

            ViewBag.ControllerName = "Dashboard";
            ViewBag.PageName = "Hızlı Bakış Tabloları & Grafikler ve İstatistikler";
            return View();
        }
    }
}
