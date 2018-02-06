using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System;
using System.Collections.Concurrent;
using System.Text;

namespace Labb4azure
{
    public static class Function1
    {

        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string id = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "id", true) == 0)
                .Value;

            //// Get request body
            //dynamic data = await req.Content.ReadAsAsync<object>();

            //// Set name to query string or body data
            //name = name ?? data?.name;

            return name == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);

            if (mode == "viewReviewQueue")
            {
                var picture = GetPicture(mode);
                return req.CreateResponse(HttpStatusCode.OK, picture, "application/json");
            }

        }

        private static List<ReviewQueue> GetPicture(string email)
        {
            string EndpointUrl = "https://labb4server.documents.azure.com:443/";
            string PrimaryKey = "VLUD2P8PI5IRSZFJhgTpUWnPa8N1iFksQbExla4bRHLb661nhdiTyRLXIVv9WzJ2e5jTQzdrFtyjy8CB1HYPkA==";
            string databaseName = "Labb4";
            string collectionName = "ReviewQueue";
            //string toCollectionName = "ApprovedPictures";
            var client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
            Console.WriteLine("\nThe review queue: ");
            IQueryable<User> userSql = this.client.CreateDocumentQuery<User>(UriFactory.CreateDocumentCollectionUri("Labb4", "ReviewQueue"),
        "SELECT * FROM User", queryOptions);

            foreach (var item in userSql)
            {
                Console.WriteLine(item.profilePicture);
            }
            var picture = query.ToList();
            return picture;
        }
    }
}
