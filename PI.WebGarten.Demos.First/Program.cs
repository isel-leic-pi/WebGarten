using PI.WebGarten.MethodBasedCommands;

namespace PI.WebGarten.Demos.First
{
    using System;
    using PI.WebGarten;
    using PI.WebGarten.Html;

    class DummyCommand : ICommand
    {
        public UriTemplate UriTemplate
        {
            get
            {
                return new UriTemplate("/dummy/{xpto}/foo/{ypto}");
            }
        }

        public string HttpMethod
        {
            get
            {
                return PI.WebGarten.HttpMethod.Get;
            }
        }

        public HttpResponse Execute(RequestInfo req)
        {
            return new HttpResponse(200, new HtmlDoc("dummy"));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var host = new HttpListenerBasedHost("http://localhost:8080/");
            host.Add(DefaultMethodBasedCommandFactory.GetCommandsFor(typeof(Controller)));
            host.Pipeline.AddFilterFirst("ConsoleLog", typeof(RequestConsoleLogFilter));
            host.Add(new DummyCommand());
            host.OpenAndWaitForever();
        }
    }
}
