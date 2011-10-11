using System;
using System.Collections.Generic;
using System.Net;
using PI.WebGarten.Html;

namespace PI.WebGarten
{
    public partial class Handler
    {
        class NotFound : HtmlDoc
        {
            public NotFound()
                :base("NotFound",
                      Img("https://a248.e.akamai.net/assets.github.com/images/modules/404/parallax_errortext.png","404"))
            {}
        }

    }
}