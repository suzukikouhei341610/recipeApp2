using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FunctionAPIApp
{

    //水谷
    [JsonObject]
    public class user_tableList
    {
        [JsonProperty("List")]
        public List<user_tableRow> List { get; set; } = new List<user_tableRow>();
    }

    [JsonObject]
    public class user_tableRow
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("user_password")]
        public string Title { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("DueDate")]
        public DateTime DueDate { get; set; }
    }
}
