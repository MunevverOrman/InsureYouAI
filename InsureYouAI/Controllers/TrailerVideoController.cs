using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers
{
    public class TrailerVideoController : Controller
    {
        private readonly InsureContext _context;

        public TrailerVideoController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult TrailerVideoList()
        {
            var value = _context.TrailerVideos.ToList();
            return View(value);
        }

        [HttpGet]
       public IActionResult CreateTrailerVideo()
        {
                return View();
        }

        [HttpPost]
        public IActionResult CreateTrailerVideo(TrailerVideo trailerVideo)
        {
            _context.TrailerVideos.Add(trailerVideo);
            _context.SaveChanges();
            return RedirectToAction("TrailerVideoList");
        }

       
        public IActionResult DeleteTrailerVideo(int id) 
        {

            var value = _context.TrailerVideos.Find(id);
            _context.TrailerVideos.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("TrailerVideoList");
        
        }

        [HttpGet]
        public IActionResult UpdateTrailerVideo(int id)
        {
            var value = _context.TrailerVideos.Find(id);
            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateTrailerVideo(TrailerVideo trailerVideo)
        {
            _context.TrailerVideos.Update(trailerVideo);
            _context.SaveChanges();
            return RedirectToAction("TrailerVideoList");
        }



    }
}
