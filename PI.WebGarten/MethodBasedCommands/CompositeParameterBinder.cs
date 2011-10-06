using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PI.WebGarten.MethodBasedCommands
{
    public class CompositeParameterBinder : IParameterBinder
    {
        private readonly ICollection<IParameterBinder> _coll;

        public CompositeParameterBinder(params IParameterBinder[] binders)
        {
            _coll = binders;
        }

        public Func<RequestInfo, object> TryGetBinder(ParameterInfo pi, HttpCmdAttribute attr)
        {
            return _coll.Select(b => b.TryGetBinder(pi, attr)).FirstOrDefault(f => f != null);
        }
    }
}