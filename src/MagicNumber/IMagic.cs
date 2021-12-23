using System.IO;

namespace MagicNumber
{
    public interface IMagic
    {
        bool Match(Stream stream, string ext);
    }
}
