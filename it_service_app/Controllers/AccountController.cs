using it_service_app.Extensions;
using it_service_app.Models;
using it_service_app.Models.Identity;
using it_service_app.Services;
using it_service_app.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace it_service_app.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;  // field for Register Action
        
        private readonly SignInManager<ApplicationUser> _signInManager; // field for Login Action

        private readonly RoleManager<ApplicationRole> _roleManager; // field for check&managing Role in persistence store

        private readonly IEmailSender _emailSender; // field for Email Services

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,RoleManager<ApplicationRole> roleManager,IEmailSender emailSender) //  gets Model class userManager=>>ApplicationUser
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;

            CheckRoles();
        }

        private void CheckRoles() 
        {
            foreach (var roleName in RoleNames.Roles )
            {
                if (!_roleManager.RoleExistsAsync(roleName).Result)
                {
                    var result = _roleManager.CreateAsync(new ApplicationRole()
                    {

                        Name = roleName
                    }).Result;

                }
            }
            System.Console.WriteLine();
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
                // Kullanıya Rol atama

                var count = _userManager.Users.Count();

                result = await _userManager.AddToRoleAsync(user, count == 1 ? RoleNames.Admin : RoleNames.Passive);

                //if (count==1)
                //{
                //    result = await _userManager.AddToRoleAsync(user, RoleNames.Admin);
                //}
                //else
                //{
                //    result = await _userManager.AddToRoleAsync(user, RoleNames.User);
                //}

                // TODO:kullancıya email dogrulama gonderme

               var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code=WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callBackUrl=Url.Action("ConfirmEmail","Account",new {userId=user.Id,code=code},
                    protocol:Request.Scheme);

                var emailMessage = new EmailMessage()
                {
                    Contacts = new string[] { user.Email },
                    Body =
                          $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callBackUrl)}'>clicking here</a>.",
                    Subject = "Confirm your email"
                };

                await _emailSender.SendAsync(emailMessage);

                //INPROGRESS:giris sayfasına yonlendirme


            }
            else // not succeeded =>
            {
                ModelState.AddModelError(string.Empty, "Kayıt isleminde bir hata olustudu");
                return View(registerViewModel);
            }

           return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId,string code) 
        {
            if (userId==null || code ==null)
                return RedirectToAction("Index","Home");
            
            var user=await _userManager.FindByIdAsync(userId);

            if (user==null)
                return NotFound($"Unable to load user with ID '{userId}'.");

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            ViewBag.StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";

            if (result.Succeeded && _userManager.IsInRoleAsync(user,RoleNames.Passive).Result)
            {
                await _userManager.RemoveFromRoleAsync(user, RoleNames.Passive);
                await _userManager.AddToRoleAsync(user, RoleNames.User);
            }

            return View();

        }

        [HttpGet]
        public IActionResult Login() 
        {
            return View();
        
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel) 
        {

            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, loginViewModel.RememberMe, true);

            if (result.Succeeded)
            {
                await _emailSender.SendAsync(new EmailMessage()
                {
                       Contacts=new string[] {"vedataydinkayaa@gmail.com"},
                       Body=$"{HttpContext.User.Identity.Name}  Sisteme giriş yaptı!",
                       Subject=$"Hey {HttpContext.User.Identity.Name}"
                });
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya sifre hatalı");
                return View(loginViewModel);
            }
            // return View();
       }
        [Authorize]  // Sİgn Out Authorize 
        public async Task<IActionResult> Logout() 
        {

                await _signInManager.SignOutAsync();

                return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> Profile() 
        {
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());

            return View();
        }

    }
}
