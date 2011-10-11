namespace PI.WebGarten.Pipeline
{
    using System;
    using System.Net;

    public interface IHttpFilter
    {
        String Name { get; }

        void SetNextFilter(IHttpFilter nextFilter);
        HttpResponse Process(HttpListenerContext ctx);
    }
}