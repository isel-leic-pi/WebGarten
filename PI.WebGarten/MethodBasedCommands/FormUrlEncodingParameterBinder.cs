using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace PI.WebGarten.MethodBasedCommands
{
    public class FormUrlEncodingParameterBinder : IParameterBinder
    {
        public Func<RequestInfo, object> TryGetBinder(ParameterInfo pi, HttpCmdAttribute attr)
        {
            if (pi.ParameterType == typeof(IEnumerable<KeyValuePair<string, string>>))
            {
                return DecodeFormUrlEncoding;
            }
            return null;
        }

        private static IEnumerable<KeyValuePair<string, string>> DecodeFormUrlEncoding(RequestInfo ri)
        {
            var ct = ri.Context.Request.ContentType;
            if (ct == null || ct != "application/x-www-form-urlencoded")
            {
                throw new Exception("Unable to bind, content type is not application/x-www-form-urlencoded");
            }
            var len = ri.Context.Request.ContentLength64;
            if(len == -1)
            {
                throw new Exception("Unable to bind, content size must be defined");
            }
            var bytes = new byte[len];
            ri.Context.Request.InputStream.Read(bytes, 0, bytes.Length);
            return Encoding.UTF8.GetString(bytes).Split('&').Select(CreateKeyValuePair).ToArray();
        }

        private static KeyValuePair<string,string> CreateKeyValuePair(string s)
        {
            var parts = s.Split('=');
            if(parts.Length <1 || parts.Length > 2)
                throw new Exception("Invalid form url encoded");
            return new KeyValuePair<string, string>(
                HttpUtility.UrlDecode(parts[0]),
                parts.Length == 2 ? HttpUtility.UrlDecode(parts[1]) : "");
        }
    }
}