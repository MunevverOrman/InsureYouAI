using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
