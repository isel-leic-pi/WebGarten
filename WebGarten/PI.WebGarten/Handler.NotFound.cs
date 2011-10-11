using System;
using System.Collections.Generic;
using System.Net;

namespace PI.WebGarten
{
    using PI.WebGarten.HttpContent.Html;

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