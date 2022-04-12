using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Cloud
{
    public enum FolderType
    {
        TEST_CASE,
        TEST_PLAN,
        TEST_CYCLE
    }

    public class FolderSearchRequest : SearchRequestBase
    {
        [JsonProperty("folderType")]
        public FolderType folderType { get; set; }

    }
}
