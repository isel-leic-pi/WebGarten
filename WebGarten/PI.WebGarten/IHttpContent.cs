

namespace PI.WebGarten
{
    public interface IHttpContent : IWritable
    {
        string ContentType { get; }
    }
}