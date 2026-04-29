using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly InsureContext _context;

        public CategoryController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult CategoryList()
        {
            ViewBag.ControllerName = "Kategoriler";
            ViewBag.PageName = "Kategori Listesi";
            var values = _context.Categories.ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public IActionResult UpdateCategory(int id)
        {
            var values= _context.Categories.Find(id);
            return View(values);
        }
        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
            return RedirectToAction("CategoryList");
        }

        public IActionResult DeleteCategory(int id)
        {
            var values=_context.Categories.Find(id);
            _context.Categories.Remove(values);
            _context.SaveChanges();
            return RedirectToAction("CategoryList");
        }
    }
}
