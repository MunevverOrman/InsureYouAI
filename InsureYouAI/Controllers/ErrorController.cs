using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult Page404()
        {
            return View();
        }
    }
}
