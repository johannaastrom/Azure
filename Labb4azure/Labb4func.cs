using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace Labb4azure
{
    public static class Function1
    {
        [FunctionName("Function1")] //detta används för att trigga eventet
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log) //denna metod kan man se som main eller en funktion som registrerar ett event
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter - skicka med information till funktionen
            string email = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "email", true) == 0)
                .Value;
            var profilePicture = (from x in req.GetQueryNameValuePairs() // kan även använda LINQ
                            where x.Key == "profilePicture"
                            select x.Value).FirstOrDefault();
            //?name=Johanna&lastname=Astrom


            if (name == null || lastname == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass an email or a profile picture on the query string.");
            }
            else
            {
                return req.CreateResponse(HttpStatusCode.OK, "User email: " + email + ", with profile picture: " + profilePicture + "\n");
            }
        }
    }
}
