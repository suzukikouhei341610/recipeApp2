using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace recipeApp2
{
    [JsonObject]
    public class user_tableList
    {
        [JsonProperty("List")]
        public List<user_tableRow> List { get; set; } = new List<user_tableRow>();
    }

    [JsonObject]
    public class user_tableRow
    {
        [JsonProperty("user_id")]
        public int user_id { get; set; }

        [JsonProperty("user_name")]
        public string user_name { get; set; }

        [JsonProperty("user_password")]
        public string user_password { get; set; }

       
    }
}
