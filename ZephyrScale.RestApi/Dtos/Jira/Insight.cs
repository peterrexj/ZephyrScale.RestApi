using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Jira
{
    public class Insight
    {
        [JsonProperty("totalIssueCount")]
        public int TotalIssueCount { get; set; }

        [JsonProperty("lastIssueUpdateTime")]
        public DateTime LastIssueUpdateTime { get; set; }
    }
}
