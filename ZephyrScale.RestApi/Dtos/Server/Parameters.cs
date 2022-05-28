using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Server
{
    public partial class Parameters
    {
        public List<object> Variables { get; set; }
        public List<object> Entries { get; set; }
    }
}
