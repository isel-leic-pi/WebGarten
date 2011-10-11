namespace PI.WebGarten.Html
{
    using System.IO;
    using System.Web;

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
}