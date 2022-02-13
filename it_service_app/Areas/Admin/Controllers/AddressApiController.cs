using DevExtreme.AspNet.Data;
using it_service_app.Data;
using it_service_app.Extensions;
using it_service_app.Models.Entities;
using it_service_app.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace it_service_app.Areas.Admin.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AddressApiController : Controller
    {
        private readonly MyContext _dbContext;

        public AddressApiController(MyContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region Cruds
        [HttpGet]
        public IActionResult Get(string userId,DataSourceLoadOptions options)
        {

            var data = _dbContext.Addresses.Where(x => x.UserId == userId);


            return Ok(DataSourceLoader.Load(data, options));
        }

        [HttpGet]
        public IActionResult Detail(Guid id,DataSourceLoadOptions loadOptions) 
        {
            var data = _dbContext.Addresses.Where(x => x.Id == id);

            return Ok(DataSourceLoader.Load(data, loadOptions));
        }
        [HttpPost]
        public IActionResult Insert(string values) 
        {
            var data = new Address();
            JsonConvert.PopulateObject(values, data);

            if (!TryValidateModel(data))

                return BadRequest(new JSonResponseViewModel()
                {
                    IsSuccess = false,
                    ErrorMessage = ModelState.ToFullErrorString()
                });

            _dbContext.Addresses.Add(data);


            var result=_dbContext.SaveChanges();

            if (result == 0)

                return BadRequest(new JSonResponseViewModel()
                {
                     IsSuccess=false,
                     ErrorMessage = "Yeni adres kaydedilemedi"
                });


            return Ok(new JSonResponseViewModel());             
        }
        [HttpPut]
        public async Task<IActionResult> Update(Guid key, string values)
        {
            var data = _dbContext.Addresses.Find(key);
            if (data == null)
                return BadRequest(new JSonResponseViewModel()
                {
                      IsSuccess=false,
                      ErrorMessage=ModelState.ToFullErrorString()

                });

            JsonConvert.PopulateObject(values, data);

            if (!TryValidateModel(data))
                return BadRequest(ModelState.ToFullErrorString());


            var result =_dbContext.SaveChanges();

            if (result==0)

                return BadRequest(new JSonResponseViewModel()
                {
                    IsSuccess = false,
                    ErrorMessage = "Adres guncellenemedi"
                });

            return Ok(new JSonResponseViewModel());

        }
        [HttpDelete]
        public IActionResult Delete(Guid key) 
        {
            var data = _dbContext.Addresses.Find(key);

            if (data == null)
                return StatusCode(StatusCodes.Status409Conflict, "Adres bulunamadı");

            _dbContext.Addresses.Remove(data);

            var result = _dbContext.SaveChanges();

            if (result == 0)
                return BadRequest("Silme islemi basarısız");

            return Ok(new JSonResponseViewModel());

        #endregion
        }
        [HttpGet]
        public object CityLookUp(DataSourceLoadOptions loadOptions) 
        {
            var data = _dbContext.Cities
              .OrderBy(x => x.Id)
              .Select(x => new
              {
                id=x.Id,
                Value=x.Id,
                Text=$"{x.Name}"
              });

            return Ok(DataSourceLoader.Load(data, loadOptions));
        }
        [HttpGet]
        public object StateLookUp(DataSourceLoadOptions loadOptions) 
        {
            var data = _dbContext.States
              .OrderBy(x => x.Name)
              .Select(x => new
              {
                   id= x.Id,
                   Value=x.Id,
                   Text=$"{x.Name}"
              });

            return Ok(DataSourceLoader.Load(data, loadOptions));
        }
    }
}
