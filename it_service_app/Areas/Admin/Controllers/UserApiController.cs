using it_service_app.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace it_service_app.Areas.Admin.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class UserApiController : ControllerBase // A  base class for an MVC without view support
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserApiController(UserManager<ApplicationUser> userManager)
        {
                _userManager = userManager;
        }
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userManager.Users.OrderBy(x => x.CreatedDate).ToList();
              
                return Ok(users);
        }
    }
}
