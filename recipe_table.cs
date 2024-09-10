using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace recipeApp2
{
    [JsonObject]
    public class recipe_tableList
    {
        [JsonProperty("List")]
        public List<recipe_tableRow> List { get; set; } = new List<recipe_tableRow>();
    }

    [JsonObject]
    public class recipe_tableRow
    {
        [JsonProperty("recipe_id")]
        public int recipe_id { get; set; }

        [JsonProperty("recipe_name")]
        public string recipe_name { get; set; }

        [JsonProperty("recipe_category1")]
        public string recipe_category1 { get; set; }

        [JsonProperty("recipe_category2")]
        public string recipe_category2 { get; set; }

        [JsonProperty("recipe_category3")]
        public string recipe_category3 { get; set; }

        [JsonProperty("recipe_time")]
        public int recipe_time { get; set; }

        [JsonProperty("recipe_scene1")]
        public string recipe_scene1 { get; set; }

        [JsonProperty("recipe_scene2")]
        public string recipe_scene2 { get; set; }

        [JsonProperty("recipe_scene3")]
        public string recipe_scene3 { get; set; }

        [JsonProperty("recipe_item1")]
        public string recipe_item1 { get; set; }

        [JsonProperty("recipe_item2")]
        public string recipe_item2 { get; set; }

        [JsonProperty("recipe_item3")]
        public string recipe_item3 { get; set; }

        [JsonProperty("recipe_photo")]
        public string recipe_photo { get; set; }



    }
}
