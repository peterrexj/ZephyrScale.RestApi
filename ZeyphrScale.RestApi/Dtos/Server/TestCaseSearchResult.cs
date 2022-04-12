using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Server
{
    public class TestCaseSearchResult
    {
        public string name { get; set; }
        public string key { get; set; }
        public string folder { get; set; }
        public string[] issueLinks { get; set; }
        public dynamic customFields { get; set; }
    }
}
