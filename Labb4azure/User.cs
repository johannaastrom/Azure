using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Labb4azure
{
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public string email { get; set; }
        public string profilePicture { get; set; }
    }
}
