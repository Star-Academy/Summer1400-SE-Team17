

using System;
using System.IO;


namespace Phase05
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            foreach (var fileRelativePath in Directory.EnumerateFiles("resources/EnglishData/"))
            {
                string fileName = fileRelativePath.Substring(fileRelativePath.LastIndexOf('/') + 1);
                string fileContent = File.ReadAllText(fileRelativePath);
                
            }
            
        }
    }
}