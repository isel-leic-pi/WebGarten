using System.IO;

namespace PI.WebGarten
{
    public interface IWritable
    {
        void WriteTo(TextWriter tw);
    }
}