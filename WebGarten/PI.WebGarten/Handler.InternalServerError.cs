using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PI.WebGarten.HttpContent.Html;

namespace PI.WebGarten
{
    public partial class Handler
    {
        class InternalServerError : HtmlDoc
        {
            public InternalServerError()
                : base("NotFound",
                       H1(Text("Erro a processar o pedidos, as nossas desculpas")))
        {}
        }
    }
}
