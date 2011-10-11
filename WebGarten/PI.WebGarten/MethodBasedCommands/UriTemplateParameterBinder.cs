using System;
using System.Reflection;

namespace PI.WebGarten.MethodBasedCommands
{
    public class UriTemplateParameterBinder : IParameterBinder
    {
        public Func<RequestInfo, object> TryGetBinder(ParameterInfo pi, HttpCmdAttribute attr)
        {
            if(pi.ParameterType.IsPrimitive && (attr.UriTemplate.PathSegmentVariableNames.Contains(pi.Name.ToUpper()) || attr.UriTemplate.QueryValueVariableNames.Contains(pi.Name.ToUpper())))
            {
                return ri => Convert.ChangeType(ri.Match.BoundVariables[pi.Name], pi.ParameterType);
            }

            return null;
        }
    }
}