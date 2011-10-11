using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PI.WebGarten.MethodBasedCommands
{
    public class MethodBasedCommandFactory : ICommandFactory
    {
        private readonly Type[] _types;
        private readonly IParameterBinder _binder;

        public MethodBasedCommandFactory(IParameterBinder binder, params Type[] types)
        {
            _types = types;
            _binder = binder;
        }

        public IEnumerable<ICommand> Create()
        {
            return 
                _types.SelectMany(t => t.GetMethods())
                    .Where(mi => mi.ReturnType == typeof(HttpResponse))
                    .Select(mi => new
                                      {
                                          MethodInfo = mi,
                                          Attributes = mi.GetCustomAttributes(typeof(HttpCmdAttribute), false) as HttpCmdAttribute[]
                                      })
                    .Where(x => x.Attributes.Length == 1)
                    .Select(x => new MethodBasedCommand(x.MethodInfo, x.Attributes[0], ResolveBindersFor(x.MethodInfo, x.Attributes[0])));
        }

        private IEnumerable<Func<RequestInfo, object>> ResolveBindersFor(MethodInfo mi, HttpCmdAttribute attr)
        {
            return mi.GetParameters().Select(p => GetBinder(p, attr));
        }

        private Func<RequestInfo, object> GetBinder(ParameterInfo pi, HttpCmdAttribute attr)
        {
            var b  = _binder.TryGetBinder(pi, attr);
            if(b == null) throw new Exception("Unable to find binder to "+pi.Name);
            return b;
        }
    }
}