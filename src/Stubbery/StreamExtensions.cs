using System;
using System.IO;
using System.Threading.Tasks;

namespace Stubbery
{
    /// <summary>
    /// Provides some convenience methods to use with <see cref="Stream" /> objects.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads the whole <paramref name="stream" /> as a string asynchronously.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <returns>The <see cref="Task" /> representing the asynchronous operation.</returns>
        public static async Task<string> ReadAsStringAsync(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var sr = new StreamReader(stream))
            {
                return await sr.ReadToEndAsync();
            }
        }

        /// <summary>
        /// Reads the whole <paramref name="stream" /> as a string synchronously.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <returns>The whole content of the stream.</returns>
        public static string ReadAsString(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }
    }
}