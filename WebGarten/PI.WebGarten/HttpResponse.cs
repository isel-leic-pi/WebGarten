using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace PI.WebGarten
{
    using System;
    using System.Reflection;

    public class HttpResponse
    {
        private readonly int _status;
        private readonly IHttpContent _content;
        private IDictionary<string, string> _headers;

        private static readonly ISet<string> _HackedHeaders = new HashSet<string> { "WWW-Authenticate" };

        private static readonly MethodInfo _AddInternalMethodInfo;

        static HttpResponse()
        {
            _AddInternalMethodInfo = typeof(WebHeaderCollection).GetMethod("AddInternal", BindingFlags.Instance | BindingFlags.NonPublic);
            if (_AddInternalMethodInfo == null)
            {
                throw new Exception("Hacking HttpListener resctricted headers not supported");
            }
        }

        public HttpResponse WithHeader(string name, string value)
        {
            EnsureHeaders();
            _headers[name] = value;
            return this;
        }

        private void EnsureHeaders()
        {
            if (_headers == null) _headers = new Dictionary<string, string>();
        }

        public HttpResponse(int status)
        {
            _status = status;
        }

        public HttpResponse(HttpStatusCode status) : this((int)status) { }

        public HttpResponse(int status, IHttpContent content)
        {
            _status = status;
            _content = content;
        }

        public HttpResponse(HttpStatusCode status, IHttpContent content) : this((int)status, content) { }

        public void Send(HttpListenerContext c)
        {
            var resp = c.Response;
            resp.StatusCode = _status;
            SetHeaders(resp);
            SendContent(resp);
            resp.Close();
        }

        private void SetHeaders(HttpListenerResponse resp)
        {
            if (_headers == null) return;
            foreach (var p in _headers)
            {
                AddHeaderInternal(p.Key, p.Value, resp.Headers);
            }
        }

        private static void AddHeaderInternal(string key, string value, WebHeaderCollection headers)
        {
            if (_HackedHeaders.Contains(key))
            {
                _AddInternalMethodInfo.Invoke(headers, new object[] { key, value });
            }
            else
            {
                headers[key] = value;
            }

        }

        private void SendContent(HttpListenerResponse resp)
        {
            if (_content == null) return;
            var ms = new MemoryStream();
            var w = new StreamWriter(ms, Encoding.UTF8);
            resp.ContentType = _content.ContentType + "; charset=utf-8";
            _content.WriteTo(w);
            w.Flush();
            resp.ContentLength64 = ms.Length;
            ms.Seek(0, SeekOrigin.Begin);
            using (var cs = resp.OutputStream)
            {
                ms.CopyTo(cs);
            }
        }
    }
}
    


