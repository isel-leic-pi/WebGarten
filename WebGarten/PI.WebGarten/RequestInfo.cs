using System;
using System.Net;

namespace PI.WebGarten
{
    using System.Security.Principal;

    public class RequestInfo
    {
        public RequestInfo(HttpListenerContext ctx)
        {
            Context = ctx;
        }

        public UriTemplateMatch Match { get; internal set; }
        public HttpListenerContext Context { get; private set; }

        public IPrincipal User
        {
            get; set;
        }
    }
}