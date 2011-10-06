using System;
using System.Net;
using System.Reflection;

namespace PI.WebGarten.MethodBasedCommands
{
    public class RequestParameterBinder : IParameterBinder
    {
        public Func<RequestInfo, object> TryGetBinder(ParameterInfo pi, HttpCmdAttribute attr)
        {
            if(pi.ParameterType == typeof(HttpListenerRequest))
            {
                return ri => ri.Context.Request;
            }
            return null;
        }
    }
}