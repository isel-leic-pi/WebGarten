using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace PI.WebGarten.Html
{
    public class HtmlElem : IWritable
    {
        private readonly string _name;

        public HtmlElem(String name, params IWritable[] cs)
        {
            _name = name;
            foreach (var c in cs)
            {
                _content.Add(c);
            }
        }

        private readonly IDictionary<string, string> _attrs = new Dictionary<string, string>();
        public HtmlElem WithAttr(string name, string value)
        {
            _attrs.Add(name, value);
            return this;
        }

        private readonly ICollection<IWritable> _content = new List<IWritable>();
        public HtmlElem WithContent(IWritable w)
        {
            _content.Add(w);
            return this;
        }

        public void WriteTo(TextWriter w)
        {
            w.Write(string.Format("<{0}", _name));
            foreach (var entry in _attrs)
            {
                // TODO attributes are not encoded
                w.Write(string.Format(" {0}='{1}'", entry.Key, entry.Value));
            }
            w.Write(">");
            foreach (var c in _content)
            {
                c.WriteTo(w);
            }
            w.Write(string.Format("</{0}>", _name));
        }
    }

    public class HtmlText : IWritable
    {
        private readonly string _text;

        public HtmlText(string text)
        {
            _text = text;
        }

        public void WriteTo(TextWriter w)
        {
            w.Write(HttpUtility.HtmlEncode(_text));
        }
    }


    public class HtmlBase
    {
        public static IWritable Text(String s) { return new HtmlText(s); }
        public static IWritable H1(params IWritable[] c) { return new HtmlElem("h1", c); }
        public static IWritable H2(params IWritable[] c) { return new HtmlElem("h2", c); }
        public static IWritable H3(params IWritable[] c) { return new HtmlElem("h3", c); }
        public static IWritable Form(String method, String url, params IWritable[] c)
        {
            return new HtmlElem("form", c)
                .WithAttr("method", method)
                .WithAttr("action", url);
        }
        public static IWritable Label(String to, String text)
        {
            return new HtmlElem("label", new HtmlText(text))
                .WithAttr("for", to);
        }
        public static IWritable InputText(String name)
        {
            return new HtmlElem("input")
                .WithAttr("type", "text")
                .WithAttr("name", name);
        }

        public static IWritable InputSubmit(String value)
        {
            return new HtmlElem("input")
                .WithAttr("type", "submit")
                .WithAttr("value", value);
        }
        public static IWritable Ul(params IWritable[] c)
        {
            return new HtmlElem("ul", c);
        }
        public static IWritable Li(params IWritable[] c)
        {
            return new HtmlElem("li", c);
        }
        public static IWritable P(params IWritable[] c)
        {
            return new HtmlElem("p", c);
        }
        public static IWritable A(String href, String t)
        {
            return new HtmlElem("a", Text(t))
                .WithAttr("href", href);
        }
        public static IWritable Img(string src, string alt)
        {
            return new HtmlElem("img")
                .WithAttr("src", src)
                .WithAttr("alt", alt);
        }
    }

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
