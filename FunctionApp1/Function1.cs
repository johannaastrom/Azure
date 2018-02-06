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
using Microsoft.Azure.Documents.Client;

namespace Labb4azure
{
    public static class Function1
    {

        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string mode = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "mode", true) == 0)
                .Value;

            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();

            // Set mode to query string or body data
            mode = mode ?? data?.mode;

            if (mode == "viewReviewQueue")
            {
                var picture = UsersInReviewQueue(mode);
                return req.CreateResponse(HttpStatusCode.OK, picture, "application/json");
            }
            else
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "lol noob");
            }
        }

        //Lista på alla rader i reviewQueue i CosmosDb
        private static List<User> UsersInReviewQueue(string email)
        {
            string EndpointUrl = "https://labb4server.documents.azure.com:443/";
            string PrimaryKey = "VLUD2P8PI5IRSZFJhgTpUWnPa8N1iFksQbExla4bRHLb661nhdiTyRLXIVv9WzJ2e5jTQzdrFtyjy8CB1HYPkA==";
            var client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
            Console.WriteLine("\nThe review queue: ");
            IQueryable<User> userSql = client.CreateDocumentQuery<User>(UriFactory.CreateDocumentCollectionUri("Labb4", "ReviewQueue"),
        "SELECT * FROM User", queryOptions);

            var user = userSql.ToList();
            return user;
        }
    }

    public class ReviewQueue
    {
        public string reviewPicture { get; set; }
    }

    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public string email { get; set; }
        public string profilePicture { get; set; }
    }
}
