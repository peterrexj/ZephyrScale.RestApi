using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Cloud
{
    public class FolderCreateRequest
    {
        [JsonProperty("parentId")]
        public long? ParentId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("projectKey")]
        public string ProjectKey { get; set; }

        [JsonProperty("folderType")]
        public string FolderType { get; set; }
    }
}
