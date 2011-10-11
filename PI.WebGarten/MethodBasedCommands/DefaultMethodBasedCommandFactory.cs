using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PI.WebGarten.MethodBasedCommands
{
    public class DefaultMethodBasedCommandFactory
    {
        private static IParameterBinder _binder = new CompositeParameterBinder(
                new UriTemplateParameterBinder(),
                new RequestParameterBinder(),
                new FormUrlEncodingParameterBinder()
        );

        public static ICommand[] GetCommandsFor(params Type[] types)
        {
            return new MethodBasedCommandFactory(_binder, types).Create().ToArray();
        }
    }
}
