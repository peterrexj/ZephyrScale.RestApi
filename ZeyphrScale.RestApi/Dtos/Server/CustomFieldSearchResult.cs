using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Server
{
    public class CustomFieldSearchResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int ProjectId { get; set; }
        public string Archived { get; set; }
        public string Required { get; set; }
        public List<Option> Options { get; set; }
    }
}
