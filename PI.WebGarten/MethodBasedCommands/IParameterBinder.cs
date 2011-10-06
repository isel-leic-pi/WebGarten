using System;
using System.Reflection;

namespace PI.WebGarten.MethodBasedCommands
{
    public interface IParameterBinder
    {
        Func<RequestInfo,object> TryGetBinder(ParameterInfo pi, HttpCmdAttribute attr);
    }
}