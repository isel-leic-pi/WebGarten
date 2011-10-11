namespace PI.WebGarten.HttpContent.Html
{
    using System;
    using System.Collections.Generic;
    using System.IO;

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
}