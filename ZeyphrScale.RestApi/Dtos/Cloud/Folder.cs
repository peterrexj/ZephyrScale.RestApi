using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Cloud
{
    public class Folder
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("parentId")]
        public int? ParentId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("index")]
        public int? Index { get; set; }

        [JsonProperty("folderType")]
        public string FolderType { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }

        public string FullPath { get; set; }
    }
}
