using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Helpers
{
    //Klasse erbt von ObjectResult
    public class UnprocessableEntityObjectResult: ObjectResult
    {
        /// <summary>
        /// UnprocessableEntityObjectResult
        /// </summary>
        /// <param name="modelState"></param>
        public UnprocessableEntityObjectResult(ModelStateDictionary modelState)
            :base (new SerializableError(modelState))
        {
            if (modelState == null)
            {
                throw new ArgumentException(nameof(modelState));
            }
            StatusCode = 422;
        }
    }
}
