using System;
using System.Collections.Generic;
using System.Net;
using PI.WebGarten.Html;

namespace PI.WebGarten
{
    public class Handler
    {
        private readonly IDictionary<string, UriTemplateTable> _tables = new Dictionary<string, UriTemplateTable>();
        private readonly string _baseAddress;

        public Handler(string baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public void Add(params ICommand[] cmds)
        {
            foreach(var cmd in cmds)
            {
                UriTemplateTable t;
                if(!_tables.TryGetValue(cmd.HttpMethod,out t))
                {
                    t = new UriTemplateTable(new Uri(_baseAddress));
                    _tables.Add(cmd.HttpMethod, t);
                }
                t.KeyValuePairs.Add(new KeyValuePair<UriTemplate, object>(cmd.UriTemplate,cmd));
            }
        }

        public void Handle(HttpListenerContext ctx)
        {
            UriTemplateTable t;
            if(!_tables.TryGetValue(ctx.Request.HttpMethod, out t))
            {
                new HttpResponse(HttpStatusCode.MethodNotAllowed).Send(ctx);
                return;
            }
            var match = t.MatchSingle(ctx.Request.Url);
            if (match == null)
            {
                new HttpResponse(HttpStatusCode.NotFound, new NotFound()).Send(ctx);
                return;
            }
            try
            {
                var resp = (match.Data as ICommand).Execute(new RequestInfo(ctx, match));
                resp.Send(ctx);
            }
            catch (Exception)
            {
                new HttpResponse(HttpStatusCode.InternalServerError).Send(ctx);
            }
        }

        class NotFound : HtmlDoc
        {
            public NotFound()
                :base("NotFound",
                      Img("https://a248.e.akamai.net/assets.github.com/images/modules/404/parallax_errortext.png","404"))
            {}
        }

    }
}