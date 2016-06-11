using System.IO;
using System.Threading.Tasks;

namespace Stubbery
{
    public static class StreamExtensions
    {
        public static Task<string> ReadAsStringAsync(this Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return sr.ReadToEndAsync();
            }
        }

        public static string ReadAsString(this Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }
    }
}