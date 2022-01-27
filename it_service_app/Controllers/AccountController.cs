using it_service_app.Models.Identity;
using it_service_app.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace it_service_app.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;  // field
        public AccountController(UserManager<ApplicationUser> userManager) //  gets Model class userManager=>>ApplicationUser
        {
            _userManager = userManager;
        }
        
        [AllowAnonymous]  // Authorization
        [HttpGet] 
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel) 
        {
            // Model state Validation
            if (!ModelState.IsValid)
            {
                registerViewModel.Password = string.Empty;
                registerViewModel.ConfirmPassword= string.Empty;    
                return View(registerViewModel);
            }
           
             var user =await _userManager.FindByNameAsync(registerViewModel.UserName);
            // UserName validation 
            if (user != null) 
            {
                ModelState.AddModelError(nameof(registerViewModel.UserName), "Bu kullanıcı adı daha oncede sisteme eklenmistir");
                return View(registerViewModel);
            }
            // Email validation
            user = await _userManager.FindByEmailAsync(registerViewModel.Email);

            if (user !=null)
            {
                ModelState.AddModelError(nameof(registerViewModel.Email), "Bu email daha oncede sisteme eklenmistir");
                return View(registerViewModel);
            }

            user = new ApplicationUser()  // new User 
            {
                UserName = registerViewModel.UserName,
                Name = registerViewModel.Name,
                Surname = registerViewModel.Surname,
                Email = registerViewModel.Email,

            };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password); // creates specified  user in store with given password
            if (result.Succeeded)
            {
                // to be done later
                // kullanıcıya rol atma
                // kullanıcıya email dogrulama gonderme
                // giris sayfasına yonlendirme
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kayıt isleminde bir hata olustudu");
                return View(registerViewModel);
            }

           return View();
        }

    }
}
