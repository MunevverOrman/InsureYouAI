using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers
{
    public class AdminLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
