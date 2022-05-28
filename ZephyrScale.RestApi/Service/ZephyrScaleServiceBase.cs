using Newtonsoft.Json;
using Pj.Library;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ZephyrScale.RestApi.Service
{
    public abstract class ZephyrScaleServiceBase
    {
        protected string _logPrefix;
        protected string _appName = "Zephyr";
        protected string _appFullEndpoint;

        public string ZeypherUrl { get; set; }
        public string ZephyrApiVersion { get; set; }
        public string JiraApiVersion { get; set; }
        public string FolderSeparator { get; set; }

        protected int PageSizeSearch;
        protected int PageSizeSearchResult;
        protected int RequestRetryTimes;
        protected int TimeToSleepBetweenRetryInMilliseconds;
        protected bool AssertResponseStatusOk;
        protected HttpStatusCode[] ListOfResponseCodeOnFailureToRetry;

        protected void Log(string message) => Console.WriteLine($"{_logPrefix}{message}");
        protected static T ToType<T>(dynamic content) => SerializationHelper.ToType<T>(content);
        protected static T ToType<T>(string content) => SerializationHelper.ToType<T>(content);
        protected static string ToJson(object content) => JsonConvert.SerializeObject(content);
    }
}
