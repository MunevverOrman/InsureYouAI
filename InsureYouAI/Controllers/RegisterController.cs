using Humanizer;
using InsureYouAI.Dtos;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRegisterDto createUserRegisterDto)
        {
            if (!ModelState.IsValid)  
                return View(createUserRegisterDto); 
            AppUser appUser = new AppUser()
            {
                Name = createUserRegisterDto.Name,
                Email = createUserRegisterDto.Email,
                Surname = createUserRegisterDto.Surname,
                UserName = createUserRegisterDto.Username,
                ImageURl = "test",
                Description = "açıklama"
            };
            await _userManager.CreateAsync(appUser,createUserRegisterDto.Password);//Createasync sayesinde şifre hashlenerek gönderilcek.
            return RedirectToAction("UserList");
         
        }
    }
}
