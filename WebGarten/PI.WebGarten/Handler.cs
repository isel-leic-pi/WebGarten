namespace PI.WebGarten
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using PI.WebGarten.Pipeline;

    public partial class Handler : IHttpFilter
    {
        private readonly IDictionary<string, UriTemplateTable> _tables = new Dictionary<string, UriTemplateTable>();
        private readonly string _baseAddress;

        private HttpFilterPipeline _pipeline;


        public Handler(string baseAddress)
        {
            _baseAddress = baseAddress;
            this._pipeline = new HttpFilterPipeline(this);
        }

        public void Add(params ICommand[] cmds)
        {
            foreach(var cmd in cmds)
            {
                UriTemplateTable t;
                if(!_tables.TryGetValue(cmd.HttpMethod, out t))
                {
                    t = new UriTemplateTable(new Uri(_baseAddress));
                    _tables.Add(cmd.HttpMethod, t);
                }

                t.KeyValuePairs.Add(new KeyValuePair<UriTemplate, object>(cmd.UriTemplate, cmd));
            }
        }

        /// <summary>
        /// Gets the <see cref="HttpFilterPipeline"/> instance to register filters.
        /// </summary>
        public HttpFilterPipeline Pipeline
        {
            get
            {
                return this._pipeline;
            }
        }



        public void Handle(HttpListenerContext ctx)
        {
            
            try
            {
                var resp = this.Pipeline.Execute(new RequestInfo(ctx));
                resp.Send(ctx);
            }
            catch (Exception e)
            {
                new HttpResponse(HttpStatusCode.InternalServerError).Send(ctx);
            }
        }

        #region Implementation of IHttpFilter

        public string Name
        {
            get
            {
                return "TerminatorHandlerFilter";
            }
        }

        public void SetNextFilter(IHttpFilter nextFilter)
        {
            // This method should never be called on thoi instance, because it is the
            // Pipeline terminator
            throw new NotImplementedException();
        }

        public HttpResponse Process(RequestInfo requestInfo)
        {
            var ctx = requestInfo.Context;
            UriTemplateTable t;
            if (!_tables.TryGetValue(ctx.Request.HttpMethod, out t))
            {
                return new HttpResponse(HttpStatusCode.MethodNotAllowed);
            }

            var match = t.MatchSingle(ctx.Request.Url);
            if (match == null)
            {
                return new HttpResponse(HttpStatusCode.NotFound, new NotFound());
            }

            requestInfo.Match = match;
            var resp = (match.Data as ICommand).Execute(requestInfo);
            return resp;
        }

        #endregion
    }
}