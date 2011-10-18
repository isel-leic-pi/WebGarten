using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PI.WebGarten.MethodBasedCommands
{
    public class MethodBasedCommand : ICommand
    {
        private readonly MethodInfo _mi;
        private readonly HttpCmdAttribute _attr;
        private readonly Func<RequestInfo, object>[] _binders;

        public MethodBasedCommand(MethodInfo mi, HttpCmdAttribute attr, IEnumerable<Func<RequestInfo, object>> binders)
        {
            _mi = mi;
            _attr = attr;
            _binders = binders.ToArray();
        }

        public UriTemplate UriTemplate
        {
            get { return _attr.UriTemplate; }
        }

        public string HttpMethod
        {
            get { return _attr.HttpMethod; }
        }

        public HttpResponse Execute(RequestInfo req)
        {
            var o = Activator.CreateInstance(_mi.DeclaringType);
            var prms = new object[_binders.Length];
            for(var i = 0 ; i<_binders.Length ; ++i)
            {
                prms[i] = _binders[i](req);
            }
            try
            {
                return _mi.Invoke(o, prms) as HttpResponse;
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }
    }
}