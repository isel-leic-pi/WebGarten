namespace PI.WebGarten.HttpContent.Html
{
    using System.IO;

    public class TextContent : IHttpContent
    {
        private readonly string _text;

        public TextContent(string text)
        {
            _text = text;
        }

        public void WriteTo(TextWriter tw)
        {
            tw.Write(_text);
        }

        public string ContentType
        {
            get { return "text/plain"; }
        }
    }
}
