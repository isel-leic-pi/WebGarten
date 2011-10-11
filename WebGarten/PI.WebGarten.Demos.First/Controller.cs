namespace PI.WebGarten.Demos.First
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using PI.WebGarten.HttpContent.Html;
    using PI.WebGarten.MethodBasedCommands;

    class Controller
    {

        [HttpCmd(HttpMethod.Get, "/xpto/{s}")]
        public HttpResponse Get(string s)
        {
            return new HttpResponse(200, new TextContent(s));
        }

        [HttpCmd(HttpMethod.Get, "/calc")]
        public HttpResponse Get()
        {
            return new HttpResponse(200, new FormView());
        }

        [HttpCmd(HttpMethod.Post, "/calc")]
        public HttpResponse Post(IEnumerable<KeyValuePair<string, string>> content)
        {
            var a = GetFromContent("a", content);
            var b = GetFromContent("b", content);
            if (!a.HasValue || !b.HasValue)
            {
                return new HttpResponse(HttpStatusCode.BadRequest, new FormView("Erro nos parâmetros"));
            }

            return new HttpResponse(200, new FormView(a.Value + b.Value));
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
}