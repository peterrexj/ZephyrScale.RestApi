using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Cloud
{
    public class TestCycle
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }

        [JsonProperty("jiraProjectVersion")]
        public Project JiraProjectVersion { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("folder")]
        public Folder Folder { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("plannedStartDate")]
        public DateTime? PlannedStartDate { get; set; }

        [JsonProperty("plannedEndDate")]
        public DateTime? PlannedEndDate { get; set; }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        [JsonProperty("customFields")]
        public dynamic CustomFields { get; set; }

        [JsonProperty("links")]
        public Links Links { get; set; }
    }
}
