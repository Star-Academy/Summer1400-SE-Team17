

using System;
using System.IO;

namespace Phase05
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            SearchEngine searchEngine = new SearchEngine();
            InvertedIndexSearcher invertedIndexSearcher = new InvertedIndexSearcher();
            invertedIndexSearcher.LoadDictionary("");
            searchEngine.Searcher = invertedIndexSearcher;
            string input = Console.ReadLine();
            int count = 0;
            foreach (var document in searchEngine.Search(input))
            {
                Console.WriteLine($"{document.DocumentIndex} : {document.Content}");
            }
        }
    }
}