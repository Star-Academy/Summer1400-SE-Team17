

using System;
using Phase08.Interfaces;
using Phase08.Model;

namespace Phase08
{
    internal class Program
    {
        private const string DataLocation = "EnglishData/";
        public static void Main(string[] args)
        {
            Run();
        }

        private static void Run()
        {
            ISearcher<Document> searchEngine = (ISearcher<Document>) HostService.ServiceProvider.GetService(typeof(ISearcher<Document>));
            
            string input = Console.ReadLine();
            foreach (var document in searchEngine.Search(input))
            {
                Console.WriteLine($"{document.DocumentIndex} : {document.Content}");
            }
        }
        
        
    }
}