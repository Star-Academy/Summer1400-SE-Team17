using System.IO;

namespace Phase04
{
    public class FileReader
    {
        public static string GetWholeFileAsString(string path)
        {
            byte[] byteFile = File.ReadAllBytes(path);
            return System.Text.Encoding.UTF8.GetString(byteFile);
        }
    }
}