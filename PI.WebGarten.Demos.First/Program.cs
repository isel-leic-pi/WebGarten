using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using PI.WebGarten;
using PI.WebGarten.Html;
using PI.WebGarten.MethodBasedCommands;

namespace PI.WebGarten.Demos.First
{
    class Controller
    {
        [HttpCmd(HttpMethod.Get, "/calc")]
        public HttpResponse Get()
        {
            return new HttpResponse(200, new FormView());
        }

        [HttpCmd(HttpMethod.Post, "/calc")]
        public HttpResponse Post(IEnumerable<KeyValuePair<string,string>> content)
        {
            var a = GetFromContent("a", content);
            var b = GetFromContent("b", content);
            if (!a.HasValue || !b.HasValue)
            {
                return new HttpResponse(HttpStatusCode.BadRequest, new FormView("Erro nos parâmetros"));
            }
            return new HttpResponse(200,new FormView(a.Value + b.Value));
        }

        public int? GetFromContent(string name, IEnumerable<KeyValuePair<string, string>> content)
        {
            string s = content.Where(p => p.Key == name).FirstOrDefault().Value;
            int i;
            if(s == null || !Int32.TryParse(s,out i))
            {
                return null;
            }
            return i;
        }
    }

    internal class FormView : HtmlDoc
    {
        public FormView()
        :base("Form",
            H1(Text("Operação")),
            Form()
        )
        {}

        public FormView(int res)
            : base("Título",
                H1(Text("Operação")),
                H2(Text("O resultado é "+res)),
                Form()
            )
        { }

        public FormView(string msg)
            : base("Título",
                H1(Text("Operação")),
                H2(Text(msg)),
                Form()
            )
        { }

        private static IWritable Form()
        {
            return 
                Form("post","/calc",
                   Label("a","a") ,InputText("a"),
                   Label("b","b") ,InputText("b"),
                   InputSubmit("Submeter"));
            
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var host = new HttpListenerBasedHost("http://localhost:8080/");
            host.Add(DefaultMethodBasedCommandFactory.GetCommandsFor(typeof(Controller)));
            host.OpenAndWaitForever();
        }
    }
}
