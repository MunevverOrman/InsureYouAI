using InsureYouAI.Context;
using InsureYouAI.Entities;
using InsureYouAI.Services;
using InsureYouAINew.Services;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers
{
    public class MessageController : Controller
    {
        private readonly InsureContext _context;
        private readonly AIService _aiService;

        public MessageController(InsureContext context, AIService aiService)
        {
            _context = context;
            _aiService = aiService;
        }

        public IActionResult MessageList()
        {
            ViewBag.ControllerName = "Gelen Mesajlar";
            ViewBag.PageName = "İletişim Panelinden Gönderilen Mesaj Listesi";
            var values = _context.Messages.ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateMessage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(Message message)
        {
            var combinedText = $"{message.Subject} - {message.MessageDetail}";
            var predictedCategory = await _aiService.PredictCategoryAsync(combinedText);
            var priority = await _aiService.PredictPriorityAsync(combinedText);

            message.AICategory = predictedCategory;
            message.Priority = priority;

            message.IsRead = false;
            message.SendDate = DateTime.Now;
            _context.Messages.Add(message);
            _context.SaveChanges();
            return RedirectToAction("MessageList");
        }

        [HttpGet]
        public IActionResult UpdateMessage(int id)
        {
            var values = _context.Messages.Find(id);
            return View(values);
        }
        [HttpPost]
        public IActionResult UpdateMessage(Message message)
        {
            _context.Messages.Update(message);
            _context.SaveChanges();
            return RedirectToAction("MessageList");
        }

        public IActionResult DeleteMessage(int id)
        {
            var values = _context.Messages.Find(id);
            _context.Messages.Remove(values);
            _context.SaveChanges();
            return RedirectToAction("MessageList");
        }
    }
}
