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
    [RoutePrefix("api/client")]
    public class ClientDetailController : ApiController
    {

        
            [Route("request/save")]
            [System.Web.Http.HttpPost]
            public async Task<HttpResponseMessage> CreateAsync([FromBody] ClientDetail client)
            {
            try
            {
                Document document = await ClientDetailRepository<ClientDetail>.CreateAsync(client);
                if (string.IsNullOrEmpty(document.Id) == false)
                {
                    Response response = new Response { status = true, id = document.Id };
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, document);
                }
            }
            catch (Exception)
            {

                throw;
            }
            }
        [Route("request/get/detail")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get([FromUri] string id)
        {
            try
            {
                Document record = ClientDetailRepository<ClientDetail>.GetAsyn(id);
                // if recevice record doesnt have id then its mean record not found
                if (string.IsNullOrEmpty(record.Id) == false)
                {   //  id not null then its means record found

                    return Request.CreateResponse(HttpStatusCode.OK, record);
                }
                else
                { // id is null 
                    return Request.CreateResponse(HttpStatusCode.BadRequest, record);
                }
            }
            catch (Exception ex)
            {

                throw;

            }
        }

    }
    }
