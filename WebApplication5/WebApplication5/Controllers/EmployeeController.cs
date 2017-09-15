using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication5.Classes;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    [RoutePrefix("api/employee")]
    public class EmployeeController : ApiController
    {    [Route("request/profile")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> CreateAsync([FromBody] Employee emp)
        {
            emp.created_date = DateTime.Now;
            emp.updated_date = DateTime.Now;
            Document document = await EmployeeRepository<Employee>.CreateAsync(emp);
            if (string.IsNullOrEmpty(document.Id) == false)
            {
                Response response = new Response { status = true, employeeId = document.Id };
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, document);
            }
        }

    }
}
