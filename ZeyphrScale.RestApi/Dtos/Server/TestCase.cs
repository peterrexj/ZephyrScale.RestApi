using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Server
{
    public class TestCase
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("projectKey")]
        public string ProjectKey { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("precondition")]
        public string Precondition { get; set; }

        [JsonProperty("objective")]
        public string Objective { get; set; }

        [JsonProperty("folder")]
        public string Folder { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("priority")]
        public string Priority { get; set; }

        [JsonProperty("component")]
        public string Component { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("estimatedTime")]
        public int EstimatedTime { get; set; }

        [JsonProperty("labels")]
        public List<string> Labels { get; set; }

        [JsonProperty("issueLinks")]
        public List<string> IssueLinks { get; set; }

        [JsonProperty("customFields")]
        public dynamic CustomFields { get; set; }

        [JsonProperty("parameters")]
        public Parameters Parameters { get; set; }

        [JsonProperty("testScript")]
        public TestScript TestScript { get; set; }


        public string Id { get; set; }
        public long MajorVersion { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public bool LatestVersion { get; set; }
        public string LastTestResultStatus { get; set; }

        public TestCaseCreateRequest ToTestCaseUpdate()
        {
            return new TestCaseCreateRequest
            {
                Name = Name,
                Precondition = Precondition,
                Objective = Objective,
                Folder = Folder,
                Status = Status,
                Labels = Labels,
                IssueLinks = IssueLinks,
                Owner = Owner,
                CustomFields = CustomFields,
                Parameters = Parameters,
                TestScript = TestScript,
                Component = Component,
                EstimatedTime = EstimatedTime   
            };
        }
    }
}
