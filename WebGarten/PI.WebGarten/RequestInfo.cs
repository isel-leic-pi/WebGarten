using System;
using System.Net;

namespace PI.WebGarten
{
    public class RequestInfo
    {
        public RequestInfo(HttpListenerContext ctx, UriTemplateMatch match)
        {
            Context = ctx;
            Match = match;
        }
        public UriTemplateMatch Match { get; private set; }
        public HttpListenerContext Context { get; private set; }
    }
}