using System;

namespace PI.WebGarten.MethodBasedCommands
{
    public class HttpCmdAttribute : Attribute
    {
        public UriTemplate UriTemplate { get; private set; }
        public string HttpMethod { get; private set; }
        public HttpCmdAttribute(string method, string template)
        {
            UriTemplate = new UriTemplate(template);
            HttpMethod = method;
        }
    }
}