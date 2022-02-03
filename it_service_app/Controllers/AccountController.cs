using AutoMapper;
using it_service_app.Extensions;
using it_service_app.Models;
using it_service_app.Models.Identity;
using it_service_app.Services;
using it_service_app.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace it_service_app.Controllers
{
    [Authorize] // specifies class  that is applied to requires the specified authorization
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;  // field for Register Action

        private readonly SignInManager<ApplicationUser> _signInManager; // field for Login Action

        private readonly RoleManager<ApplicationRole> _roleManager; // field for check&managing Role in persistence store

        private readonly IEmailSender _emailSender; // field for Email Services

        private readonly IMapper _mapper;     // field for Mapper Services 

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IEmailSender emailSender, IMapper mapper)   //  gets Model class userManager=>>ApplicationUser
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _mapper = mapper;

            CheckRoles();
        }

        private void CheckRoles()
        {
            foreach (var roleName in RoleNames.Roles)
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous] // is applied to  does not require authorization
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            // Model state Validation
            if (!ModelState.IsValid)
            {
                registerViewModel.Password = string.Empty;
                registerViewModel.ConfirmPassword = string.Empty;
                return View(registerViewModel);
            }

            var user = await _userManager.FindByNameAsync(registerViewModel.UserName);
            // UserName validation 
            if (user != null)
            {
                ModelState.AddModelError(nameof(registerViewModel.UserName), "Bu kullanıcı adı daha oncede sisteme eklenmistir");
                return View(registerViewModel);
            }
            // Email validation
            user = await _userManager.FindByEmailAsync(registerViewModel.Email);

            if (user != null)
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
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callBackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                    protocol: Request.Scheme);

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
                ModelState.AddModelError(string.Empty, ModelState.ToFullErrorString());
                return View(registerViewModel);
            }

            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return RedirectToAction("Index", "Home");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound($"Unable to load user with ID '{userId}'.");

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            ViewBag.StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";

            if (result.Succeeded && _userManager.IsInRoleAsync(user, RoleNames.Passive).Result)
            {
                await _userManager.RemoveFromRoleAsync(user, RoleNames.Passive);
                await _userManager.AddToRoleAsync(user, RoleNames.User);
            }

            return View();

        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();

        }
        [AllowAnonymous]
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
                //await _emailSender.SendAsync(new EmailMessage()
                //{
                //    Contacts = new string[] { "vedataydinkayaa@gmail.com" },
                //    Body = $"{HttpContext.User.Identity.Name}  Sisteme giriş yaptı!",
                //    Subject = $"Hey {HttpContext.User.Identity.Name}"
                //});
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya sifre hatalı");
                return View(loginViewModel);
            }
            // return View();
        }

        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());  //  b0cdf522-ca1b-45b1-bd96-9be5461aa38a}

            //var model = new UserProfileViewModel()
            //{
            //    Email = user.Email,
            //    Name = user.Name,
            //    Surname = user.Surname

            //};
            var model = _mapper.Map<UserProfileViewModel>(user); // Execute a mapping from the source object to a new destination object

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileViewModel userProfileViewModel)
        {
            if (!ModelState.IsValid)

                return View(userProfileViewModel);


            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());

            user.Name = userProfileViewModel.Name;
            user.Surname = userProfileViewModel.Surname;


            if (user.Email != userProfileViewModel.Email)  // if any changed for email adress
            {
                await _userManager.RemoveFromRoleAsync(user, RoleNames.User);
                await _userManager.AddToRoleAsync(user, RoleNames.Passive);

                user.Email = userProfileViewModel.Email;
                user.EmailConfirmed = false;


                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callBackUrl = Url.Action("ConfirmEmail", "Account",

                    new { userId = user.Id, code = code }, protocol: Request.Scheme);


                var emailMessage = new EmailMessage()
                {
                    Contacts = new string[] { user.Email },
                    Body =
                       $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callBackUrl)}'>clicking here</a>.",
                    Subject = "Confirm your email"
                };

                await _emailSender.SendAsync(emailMessage);
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, ModelState.ToFullErrorString());
            }

            return View(userProfileViewModel);

        }
        public IActionResult PasswordUpdate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordUpdate(PasswordUpdateViewModel passwordUpdateViewModel)
        {
            if (!ModelState.IsValid)
                return View(passwordUpdateViewModel);

            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());  //b0cdf522-ca1b-45b1-bd96-9be5461aa38a

            var result = await _userManager.ChangePasswordAsync(user, passwordUpdateViewModel.OldPassword, passwordUpdateViewModel.NewPassword);

            if (result.Succeeded)
            {
                // send an email
                TempData["Message"] = "Şifre degiştirme işleminiz başarılı";
                return View();
            }
            else
            {
                var message = string.Join("<br>",
                    result.Errors.Select(x => x.Description));
                TempData["Message"] = message;

                return View();
            }

        }

        [AllowAnonymous]
        public IActionResult ResetPassword()
        {
            return View();

        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);  // user id ==>> b0cdf522-ca1b-45b1-bd96-9be5461aa38a"

            if (user == null)
                ViewBag.Message = "Girdiginiz email sistemde bulunamadı";
            else
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callBackUrl = Url.Action("ConfirmResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Scheme);

                var emailMessage = new EmailMessage()
                {
                    Contacts = new string[] { user.Email },
                    Body =
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callBackUrl)}'>clicking here</a>.",
                    Subject = "Reset Password"
                };

                await _emailSender.SendAsync(emailMessage);
                ViewBag.Message = "Mailinize sifre guncelleme yonergesi gonderilmistir";
            }

            return View();
        }
        [AllowAnonymous]

        public IActionResult ConfirmResetPassword(string userId,string code) 
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))

                    return BadRequest("Hatalı Istek");

            ViewBag.Code = code;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public  async Task<IActionResult> ConfirmResetPassword(ResetPasswordViewModel resetPasswordViewModel) 
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordViewModel);
            }
            var user = await _userManager.FindByIdAsync(resetPasswordViewModel.UserId);
            if (user==null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı");
                return View();
            }
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.Code, resetPasswordViewModel.NewPassword);

            if (result.Succeeded)
            {
                // send an email
                TempData["Message"] = "Sifre degisikliginiz gercekleşmiştir";
                return View();
            }
            else
            {
                var message = string.Join("<br>", result.Errors.Select(x => x.Description));
                TempData["Message"] = message;
                return View();
            }


            

        
             
        }
    }
}
