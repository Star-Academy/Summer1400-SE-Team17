

using System;
namespace Phase05
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
            SearchEngine searchEngine = new SearchEngine();
            InvertedIndexSearcher invertedIndexSearcher = new InvertedIndexSearcher();
            invertedIndexSearcher.LoadDatabase(DataLocation);
            searchEngine.Searcher = invertedIndexSearcher;
            string input = Console.ReadLine();
            foreach (var document in searchEngine.Search(input))
            {
                Console.WriteLine($"{document.DocumentIndex} : {document.Content}");
            }
        }
    }
}