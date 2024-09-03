using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FunctionAPIApp
{
    [JsonObject]
    public class Task_NewTableList
    {
        [JsonProperty("List")]
        public List<Task_NewTableRow> List { get; set; } = new List<Task_NewTableRow>();
    }

    [JsonObject]
    public class Task_NewTableRow
    {
        [JsonProperty("TaskID")]
        public int TaskID { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("DueDate")]
        public DateTime DueDate { get; set; }
    }
}
