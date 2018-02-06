using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System.Data.SqlClient;


namespace Labb4azure
{
    public class Program
    {
        private const string EndpointUrl = "https://labb4server.documents.azure.com:443/";
        private const string PrimaryKey = "VLUD2P8PI5IRSZFJhgTpUWnPa8N1iFksQbExla4bRHLb661nhdiTyRLXIVv9WzJ2e5jTQzdrFtyjy8CB1HYPkA==";
        private DocumentClient client;

        //public const string connectionString = @"AccountEndpoint=https://labb4server.documents.azure.com:443/;AccountKey=VLUD2P8PI5IRSZFJhgTpUWnPa8N1iFksQbExla4bRHLb661nhdiTyRLXIVv9WzJ2e5jTQzdrFtyjy8CB1HYPkA==;";

        static void Main(string[] args)
        {
            try
            {
                Program p = new Program();
                p.GetStartedDemo().Wait();
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
            finally
            {
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }

        public void ViewReviewQueue()
        {
            //        IQueryable<User> query = client.CreateDocumentQuery<User>(
            //        UriFactory.CreateDocumentCollectionUri("Labb4", "ReviewQueue")).Where(m => m.email == email && m.profilePicture == "");

            //        foreach (var item in collection)
            //        {
            //            Console.WriteLine();
            //        }
        }


        private async Task GetStartedDemo()
        {
            Console.WriteLine("Enter your email adress: ");
            string email = Console.ReadLine();
            Console.WriteLine("Upload your profile picture: ");
            string picture = Console.ReadLine();

            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = "Labb4" });
            await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("Labb4"), new DocumentCollection { Id = "User" });
            await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("Labb4"), new DocumentCollection { Id = "ReviewQueue" });
            await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("Labb4"), new DocumentCollection { Id = "ApprovedPictures" });

            User newUser = new User
            {
                email = email,
                profilePicture = picture
            };

            await this.CreateUserDocumentIfNotExists("Labb4", "User", newUser);
            await this.CreateReviewDocumentIfNotExists("Labb4", "ReviewQueue", newUser);

            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
            Console.WriteLine("\nThe review queue: ");
            IQueryable<User> userSql = this.client.CreateDocumentQuery<User>(
        UriFactory.CreateDocumentCollectionUri("Labb4", "ReviewQueue"),
        "SELECT * FROM User", queryOptions);

            foreach (var item in userSql)
            {
                Console.WriteLine(item.profilePicture);
            }
        }

        private void WriteToConsoleAndPromptToContinue(string format, params object[] args)
        {
            Console.WriteLine(format, args);
            Console.WriteLine("Success!");
            Console.ReadKey();
        }

        private async Task CreateUserDocumentIfNotExists(string databaseName, string collectionName, User user)
        {
            try
            {
                await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, user.email));
                this.WriteToConsoleAndPromptToContinue("Found {0}", user.email);
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), user);
                    this.WriteToConsoleAndPromptToContinue("Created user {0}", user.email);
                }
                else
                {
                    throw;
                }
            }
        }
        private async Task CreateReviewDocumentIfNotExists(string databaseName, string collectionName, User user)
        {
            try
            {
                await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, user.profilePicture));
                this.WriteToConsoleAndPromptToContinue("Found {0}", user.profilePicture);
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), user);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}