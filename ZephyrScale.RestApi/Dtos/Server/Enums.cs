using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Server
{
    public enum FolderType
    {
        TEST_CASE,
        TEST_RUN
    }

    public enum TestResultStatus
    {
        Pass,
        Fail,
        Blocked
    }
}
