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
    [RoutePrefix("api/vehicle")]
    public class VehicleDetailController : ApiController
    {

        
            [Route("request/save")]
            [System.Web.Http.HttpPost]
            public async Task<HttpResponseMessage> CreateAsync([FromBody] VehicleDetail vehical)
            {
            try
            {
                Document document = await VehicleDetailRepository<VehicleDetail>.CreateAsync(vehical);
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
        [Route("request/get/detail/lot")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetVehicalBaseOnLotId([FromUri] string lot_id = "")
        {
            try
            {
                Document record = VehicleDetailRepository<VehicleDetail>.GetVehicaleBasesOnLotNoAsyn(lot_id);
                // if recevice record doesnt have id then its mean record not found
                if (string.IsNullOrEmpty(record.Id) == false)
                {   //  id not null then its mean record found

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

        [Route("request/get/detail/buyerId")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetVehicalBaseOnBuyerId([FromUri] string buyerId = "")
        {
            try
            {
                Document record = VehicleDetailRepository<VehicleDetail>.GetVehicalBaseOnBuyerIdAsyn(buyerId);
                // if recevice record doesnt have id then its mean record not found
                if (string.IsNullOrEmpty(record.Id) == false)
                {   //  id not null then its mean record found

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
