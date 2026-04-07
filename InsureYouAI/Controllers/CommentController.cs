using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAI.Controllers
{
    public class CommentController : Controller
    {
        private readonly InsureContext _context;

        public CommentController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult CommentList()
        {
            var values = _context.Comments.Include(x => x.AppUser).Include(y => y.Article).ToList();
            return View(values);
        }
    }
}
