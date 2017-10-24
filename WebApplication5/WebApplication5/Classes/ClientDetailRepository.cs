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
    public class ClientDetailRepository<VehicleDteail> where VehicleDteail : class
    {
        private static readonly string DatabaseId = ConfigurationManager.AppSettings["database"];
        private static readonly string CollectionId = ConfigurationManager.AppSettings["collection_client"];
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
          // save client info
        public static async Task<Document> CreateAsync(ClientDetail value) 
        {
            try
            {
                value.CreatedDate = DateTime.Now;
                value.UpdatedDate = DateTime.Now;
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

        // delete specific client base on id
        public static Task DeleteAsyn(string id)
        {
            throw new NotImplementedException();
        }
        // get Client base on client id
        public static Document GetAsyn(string id)
        {
            try
            {
                Document response;
                bool isClientFound = false;
                ClientDetail clientDetail=null;
                if (id != "")
                {    

                    clientDetail = client.CreateDocumentQuery<ClientDetail>(
                        UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId)).Where(f => f.clientId == id).AsEnumerable().FirstOrDefault();
                    if (clientDetail != null)
                    {
                        isClientFound = true;
                    }
                    else
                    {
                        isClientFound = false;
                    }
                }
                
                else  
                {
                    isClientFound = false;
                }

              //  check the value of isClientFound  if it is true then return custom response document with found Client else return the not found data response document
                #region
                if (isClientFound)
                {

                    response = new Document { Id = clientDetail.id };
                    response.SetPropertyValue("status", true);
                    response.SetPropertyValue("client", clientDetail);
                    response.SetPropertyValue("errorMessage", "");
                }
                else
                {
                    response = new Document { };
                    response.SetPropertyValue("status", false);
                    response.SetPropertyValue("client", "{}");
                    response.SetPropertyValue("errorMessage", "not found");
                } 
                #endregion
            return response;
            }
            catch (DocumentClientException e) 
            {
                Document response = new Document { };
                response.SetPropertyValue("status", false);
                response.SetPropertyValue("client", "{ }");
                response.SetPropertyValue("errorMessage", e.Message);
                return response;
            }

        }// end GetAsyn()


        public static Task<Document> UpdateAsyn(string id, VehicleDetail value)
        {
            throw new NotImplementedException();
        }
    }
}