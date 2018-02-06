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
            string mode = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "id", true) == 0)
                .Value;

            if (mode == "viewReviewQueue")
            {
                var picture = GetPicture(mode);
                return req.CreateResponse(HttpStatusCode.OK, picture, "application/json");
            }

        }

        private static List<User> GetPicture(string email)
        {
            string EndpointUrl = "https://labb4server.documents.azure.com:443/";
            string PrimaryKey = "VLUD2P8PI5IRSZFJhgTpUWnPa8N1iFksQbExla4bRHLb661nhdiTyRLXIVv9WzJ2e5jTQzdrFtyjy8CB1HYPkA==";
            //string databaseName = "Labb4";
            //string collectionName = "ReviewQueue";
            //string toCollectionName = "ApprovedPictures";
            var client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
            Console.WriteLine("\nThe review queue: ");
            IQueryable<User> userSql = this.client.CreateDocumentQuery<User>(UriFactory.CreateDocumentCollectionUri("Labb4", "ReviewQueue"),
        "SELECT * FROM User", queryOptions);

            var picture = userSql.ToList();
            return picture;
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
