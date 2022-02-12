using DevExtreme.AspNet.Data;
using it_service_app.Extensions;
using it_service_app.Models.Identity;
using it_service_app.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace it_service_app.Areas.Admin.Controllers
{
    [Route("api/[controller]/[action]")]
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
        [HttpPut]
        public async Task<IActionResult> UpdateUsers(string key, string values) 
        {
            var data = _userManager.Users.FirstOrDefault(x => x.Id == key);
            if (data == null)
                return StatusCode(StatusCodes.Status409Conflict, new JSonResponseViewModel()
                {
                     IsSuccess=false,
                      ErrorMessage="Kullanıcı bulunamadı"
                });
            JsonConvert.PopulateObject(values, data);

            if (!TryValidateModel(data))
                return BadRequest(ModelState.ToFullErrorString());

            var result=await _userManager.UpdateAsync(data);

            if (!result.Succeeded)

                return BadRequest(new JSonResponseViewModel()
                {
                      IsSuccess = false,
                       ErrorMessage="Kullanıcı guncellenemedi"
                });

            return Ok(new JSonResponseViewModel());

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
