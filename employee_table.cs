using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace recipeApp2
{
    [JsonObject]
    public class employee_tableList
    {
        [JsonProperty("List")]
        public List<employee_tableRow> List { get; set; } = new List<employee_tableRow>();
    }

    [JsonObject]
    public class employee_tableRow
    {
        [JsonProperty("employee_id")]
        public int employee_id { get; set; }

        [JsonProperty("employee_password")]
        public string employee_password { get; set; }

       


    }
}
