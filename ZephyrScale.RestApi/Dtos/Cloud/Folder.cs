using Newtonsoft.Json;

namespace ZephyrScale.RestApi.Dtos.Cloud
{
    public class Folder
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("parentId")]
        public long? ParentId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("index")]
        public int? Index { get; set; }

        [JsonProperty("folderType")]
        public string FolderType { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }

        [JsonProperty("fullPath")]
        public string FullPath { get; set; }
    }
}
