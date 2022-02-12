using DevExtreme.AspNet.Data;
using it_service_app.Extensions;
using it_service_app.Models.Identity;
using it_service_app.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace it_service_app.Areas.Admin.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserApiController : ControllerBase // A  base class for an MVC without view support
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserApiController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult GetUsers(DataSourceLoadOptions loadOptions)
        {

            var data = _userManager.Users;

            return Ok(DataSourceLoader.Load(data, loadOptions));
        }
        [HttpGet]
        public IActionResult GetTest()
        {
            var users = new List<UserProfileViewModel>();

            for (int i = 0; i < 10000; i++)
            {
                users.Add(new()
                {
                    Email = "Deneme" + i,
                    Name = "soyad" + i,
                    Surname = "ad" + i
                });
            }

            return Ok(new JSonResponseViewModel()
            {
                Data = users

            });
        }
    }
}
