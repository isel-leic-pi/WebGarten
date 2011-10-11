using System.IO;

namespace PI.WebGarten.Html
{
    public class HtmlDoc : HtmlBase, IHttpContent
    {
        private readonly IWritable[] _c;
        private readonly string _t;

        public HtmlDoc(string t, params IWritable[] content)
        {
            _t = t;
            _c = content;
        }

        public void WriteTo(TextWriter tw)
        {
            new HtmlElem("html",
                    new HtmlElem("head", new HtmlElem("title", new HtmlText(_t))),
                    new HtmlElem("body", _c)
                ).WriteTo(tw);
        }

        public string ContentType
        {
            get { return "text/html"; }
        }
    }
}
