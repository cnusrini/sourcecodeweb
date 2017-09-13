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
    public class EmployeeRepository<Employee> where Employee : class
    {
        private static readonly string DatabaseId = ConfigurationManager.AppSettings["database"];
        private static readonly string CollectionId = ConfigurationManager.AppSettings["collection"];
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

        public static async Task<Document> CreateAsync(Employee value)
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

        public static Task DeleteAsyn(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Employee> GetAsyn(string id)
        {
            try
            {
                var uri = UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id);
                Document document = await client.ReadDocumentAsync(uri);
                return (Employee)(dynamic)document;
            }
            catch (DocumentClientException e)
            { throw e; }
        }



        public static Task<Document> UpdateAsyn(string id, Employee value)
        {
            throw new NotImplementedException();
        }
    }
}