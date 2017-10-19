using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
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
        public static async Task<Document> GetAsyn(string id)
        {
            try
            {
                var uri = UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id);
                Document document = await client.ReadDocumentAsync(uri);
                Document response = new Document {Id = document.Id}; // return document's id use as id in response document

                #region retrun response 
                response.SetPropertyValue("status", true);
                response.SetPropertyValue("vehicle", document);
                response.SetPropertyValue("errorMessage", " "); 
                #endregion
                // return (VehicleDetail)(dynamic)document;
                return response;
            }
            catch (DocumentClientException e) // excute if record not found
            {
                Document response = new Document {  }; 
                response.SetPropertyValue("status", false);
                response.SetPropertyValue("vehicle", null);
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