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

namespace it_service_app.Areas.Admin.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SubscriptionTypeApiController : Controller
    {
        private readonly MyContext _dbContext;
        public SubscriptionTypeApiController(MyContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region Crud

        public IActionResult Get(DataSourceLoadOptions options)
        {
            var data = _dbContext.subscriptionTypes;

            return Ok(DataSourceLoader.Load(data, options));
        }
        public IActionResult Detail(Guid id, DataSourceLoadOptions options)
        {
            var data = _dbContext.subscriptionTypes
                       .Where(x => x.Id == id);

            return Ok(DataSourceLoader.Load(data, options));
        }
        [HttpPost]
        public IActionResult Insert(string values)
        {
            var data = new SubscriptionType();
            JsonConvert.PopulateObject(values, data);
            if (!TryValidateModel(data))

                return BadRequest(new JSonResponseViewModel()
                {
                    IsSuccess = false,
                    ErrorMessage = ModelState.ToFullErrorString()
                });
             
            _dbContext.subscriptionTypes.Add(data);

            var result=_dbContext.SaveChanges();

            if (result==0)
            {
                return BadRequest(new JSonResponseViewModel
                {

                    IsSuccess = false,
                    ErrorMessage = "Yeni uyelik tipi kaydedilemedi"

                });
            }

            return Ok(new JSonResponseViewModel());
        }
        [HttpPut]
        public IActionResult Update(Guid key,string values) 
        {
            var data = _dbContext.subscriptionTypes.Find(key);
            if (data == null)
                return BadRequest(new JSonResponseViewModel()
                {
                     IsSuccess=false,
                      ErrorMessage =ModelState.ToFullErrorString(),
                });

            JsonConvert.PopulateObject(values, data);

            if (!TryValidateModel(data))
                return BadRequest(new JSonResponseViewModel()
                {
                     IsSuccess=false,
                     ErrorMessage="Uyelik tipi guncellenemedi"
                });
            
            return Ok(new JSonResponseViewModel());
        }
        [HttpDelete]
        public IActionResult Delete(Guid key) 
        {
            var data = _dbContext.subscriptionTypes.Find(key);

            if (data == null)
                return StatusCode(StatusCodes.Status409Conflict, "Uyelik tipi bulunamadı");

             _dbContext.subscriptionTypes.Remove(data);
            
            var result=_dbContext.SaveChanges();

            if (result == 0)
                return BadRequest("Silme islemi basarısız");

            return Ok(new JSonResponseViewModel());     
                
        }
        #endregion
    }
}
