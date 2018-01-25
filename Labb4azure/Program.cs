using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace Labb4azure
{
    public class Picture
    {

    }

    public class Program
    {
        private const string EndpointUrl = "https://labb4server.documents.azure.com:443/";
        private const string PrimaryKey = "VLUD2P8PI5IRSZFJhgTpUWnPa8N1iFksQbExla4bRHLb661nhdiTyRLXIVv9WzJ2e5jTQzdrFtyjy8CB1HYPkA==";
        private DocumentClient client;

        static void Main(string[] args)
        {
            ConcurrentQueue<Picture> reviewQueue = new ConcurrentQueue<Picture>(); 

            Console.WriteLine("Enter your email adress: ");
            string email = Console.ReadLine();
            Console.WriteLine("Upload your profile picture: ");
            string picture = Console.ReadLine();

            
            
        }
    }
}
