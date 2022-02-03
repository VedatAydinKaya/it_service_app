using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace it_service_app.Extensions
{
    public static class AppExtensions
    {
        public static string GetUserId(this HttpContext context)  //"b0cdf522-ca1b-45b1-bd96-9be5461aa38a"
        {
            var claims=context.User.Claims.ToList();
            return context.User.Claims.First
                (x =>  x.Type == ClaimTypes.NameIdentifier).Value;

            Console.WriteLine();
        }
        public static string ToFullErrorString(this ModelStateDictionary modelState) 
        {
            var messages = new List<string>();


            foreach (var entry in modelState.Values) 
            {

                foreach (var error in entry.Errors)
                {
                    messages.Add(error.ErrorMessage);
                }
            }

            return String.Join(" ", messages);
        }
    }
}
