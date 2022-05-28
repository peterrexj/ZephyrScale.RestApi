using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Server
{
    public class TestCycleItem
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("testCaseKey")]
        public string TestCaseKey { get; set; }

        [JsonProperty("environment")]
        public string Environment { get; set; }

        [JsonProperty("userKey")]
        public string UserKey { get; set; }

        [JsonProperty("executionDate")]
        public DateTime? ExecutionDate { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("executedBy")]
        public string ExecutedBy { get; set; }

        [JsonProperty("assignedTo")]
        public string AssignedTo { get; set; }

        [JsonProperty("plannedStartDate")]
        public DateTime? PlannedStartDate { get; set; }

        [JsonProperty("plannedEndDate")]
        public DateTime? PlannedEndDate { get; set; }

        [JsonProperty("actualStartDate")]
        public DateTime? ActualStartDate { get; set; }

        [JsonProperty("actualEndDate")]
        public DateTime? ActualEndDate { get; set; }

        [JsonProperty("customFields")]
        public dynamic CustomFields { get; set; }
    }
}
