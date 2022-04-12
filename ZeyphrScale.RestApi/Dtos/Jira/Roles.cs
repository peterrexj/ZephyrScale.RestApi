using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Jira
{
    public class Roles
    {
        [JsonProperty("Developers")]
        public string Developers { get; set; }
    }
}
