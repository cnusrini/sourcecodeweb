using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication5.Models;

namespace WebApplication5.Classes
{
    public class VehicleDetailRepository<VehicleDteail> where VehicleDteail : class
    {
        private static readonly string DatabaseId = ConfigurationManager.AppSettings["database"];
        private static readonly string CollectionId = ConfigurationManager.AppSettings["collection_vehical"];
        private static DocumentClient client;
        public static void Initialize()
        {
            client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["endpoint"]), ConfigurationManager.AppSettings["authKey"]);
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        private static async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {    // if Collection already exist then switch to that
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)   // else create one
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }
        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {      // if Database already exist then switch to that
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)   // else create one 
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw e;
                }

            }
        }
          // save vehical info
        public static async Task<Document> CreateAsync(VehicleDetail value) 
        {
            try
            {
                return  await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), value);
            }
            catch (DocumentClientException e) 
            {
                Document response = new Document {  };
                response.SetPropertyValue("status", false);
                response.SetPropertyValue("errorMessage", e.Message);
                return response;
            }
        }

        // delete specific vehical base on id
        public static Task DeleteAsyn(string id)
        {
            throw new NotImplementedException();
        }
        // get vehical base on vehical id
        public static Document GetVehicaleBasesOnLotNoAsyn(string LotId)
        {
            try
            {
                Document response;
                bool isVehicalFound = false;
                VehicleDetail vehicle=null;
                if (LotId !="")
                {    // if Lot id not empty the excute  it
                    
                    vehicle = client.CreateDocumentQuery<VehicleDetail>(
                        UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId)).Where(f => f.LotId == LotId).AsEnumerable().FirstOrDefault();
                    if (vehicle != null)
                    {
                        isVehicalFound = true;
                    }
                    else
                    {
                        isVehicalFound = false;
                    }
                }
              
                else  
                {
                    isVehicalFound = false;
                }

              //  check the value of isVehicalFound  if it is true then return custom response document with found vehical else return the not found data response document
                #region
                if (isVehicalFound)
                {

                    response = new Document { Id = vehicle.id };
                    response.SetPropertyValue("status", true);
                    response.SetPropertyValue("vehicle", vehicle);
                    response.SetPropertyValue("errorMessage", "not found");
                }
                else
                {
                    response = new Document { };
                    response.SetPropertyValue("status", false);
                    response.SetPropertyValue("vehicle", "{}");
                    response.SetPropertyValue("errorMessage", "not found");
                } 
                #endregion
            return response;
            }
            catch (DocumentClientException e) 
            {
                Document response = new Document { };
                response.SetPropertyValue("status", false);
                response.SetPropertyValue("vehicle", "{}");
                response.SetPropertyValue("errorMessage", e.Message);
                return response;
            }

        }// end GetAsyn()


        //returned list of vehicle base on buyer id
        public static Document GetVehicalBaseOnBuyerIdAsyn(string buyerId) {
            try
            {
                Document response = null;
                bool isVehicalFound = false;
                
              
                if (buyerId != "")
                {  // if buyer id not empty then excute  it
                    var vehicle = client.CreateDocumentQuery<VehicleDetail>(
                             UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId)).Where(f => f.BuyerID == buyerId).AsEnumerable().ToList();
                    if (vehicle.Count>0)
                    {
                        response = new Document { Id = vehicle[0].id }; // we can give any id to document,so here i giving id to document of the first item of returned result
                        response.SetPropertyValue("status", true);
                        response.SetPropertyValue("vehicle", vehicle);
                        response.SetPropertyValue("errorMessage", "");
                    }
                    else
                    {
                        isVehicalFound = true;
                    }
                }
                else
                {
                    isVehicalFound = true;
                }

                // return the not found data response document
                #region
                if (isVehicalFound)   // if true  means no record found
                
                {
                    response = new Document { };
                    response.SetPropertyValue("status", false);
                    response.SetPropertyValue("vehicle", "[]");
                    response.SetPropertyValue("errorMessage", "not found");
                }
                #endregion
                return response;
            }
            catch (DocumentClientException e)
            {
                Document response = new Document { };
                response.SetPropertyValue("status", false);
                response.SetPropertyValue("vehicle", "{}");
                response.SetPropertyValue("errorMessage", e.Message);
                return response;
            }






        }


        public static Task<Document> UpdateAsyn(string id, VehicleDetail value)
        {
            throw new NotImplementedException();
        }


    }
}