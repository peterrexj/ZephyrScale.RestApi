using Newtonsoft.Json;

namespace ZephyrScale.RestApi.Dtos.Cloud
{
    public class EnvironmentCreateRequest
    {
        [JsonProperty("projectKey")]
        public string ProjectKey { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("index")]
        public int? Index { get; set; }
    }
}