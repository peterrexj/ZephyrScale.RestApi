using Newtonsoft.Json;
using Pj.Library;
using System;

namespace ZephyrScale.RestApi.Dtos.Cloud
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
        public string PlannedStartDate { get; set; }

        public DateTime? PlannedStartAsDateTime
        {
            get
            {
                if (PlannedStartDate.HasValue())
                {
                    DateTime.TryParse(PlannedStartDate, out var result);
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonProperty("plannedEndDate")]
        public string? PlannedEndDate { get; set; }

        public DateTime? PlannedEndDateAsDateTime
        {
            get
            {
                if (PlannedEndDate.HasValue())
                {
                    DateTime.TryParse(PlannedEndDate, out var result);
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        [JsonProperty("customFields")]
        public dynamic CustomFields { get; set; }

        [JsonProperty("links")]
        public Links Links { get; set; }
    }
}
