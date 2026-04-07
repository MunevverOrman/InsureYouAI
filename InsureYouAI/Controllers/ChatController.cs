using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult SendChatWithAI()
        {
            return View();
        }
    }
}
