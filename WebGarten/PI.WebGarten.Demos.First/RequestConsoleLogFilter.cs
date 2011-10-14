namespace PI.WebGarten.Demos.First
{
    using System;
    using System.Diagnostics;
    using System.Net;

    using PI.WebGarten.Pipeline;

    public class RequestConsoleLogFilter : IHttpFilter
    {
        private readonly string _filterName;

        private IHttpFilter _nextFilter;

        public RequestConsoleLogFilter(string filterName)
        {
            this._filterName = filterName;
        }

        public string Name
        {
            get {
                return _filterName;
            }
        }

        public void SetNextFilter(IHttpFilter nextFilter)
        {
            _nextFilter = nextFilter;
        }

        public HttpResponse Process(RequestInfo requestInfo)
        {
            var ctx = requestInfo.Context;
            Trace.TraceInformation("[LogFilter]: Request for URI '{0}'", ctx.Request.Url);
            var resp = _nextFilter.Process(requestInfo);
            Trace.TraceInformation("[LogFilter]: User '{0}'", requestInfo.User != null ? requestInfo.User.Identity.Name : String.Empty);
            return resp;
        }
    }
}