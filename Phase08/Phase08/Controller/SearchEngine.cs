using System.Collections.Generic;
using System.Linq;
using Phase08.Model;
using Phase08.Interfaces;

namespace Phase08
{
    public class SearchEngine : ISearcher<Document>
    {
        private ISearcher<Document> _searcher;

        public SearchEngine(ISearcher<Document> searcher)
        {
            _searcher = searcher;
        }

        public SearchEngine()
        {
        }

        public ISearcher<Document> Searcher
        {
            set => _searcher = value;
        }

        public HashSet<Document> Search(string command)
        {
            string[] splitCommands = command.Trim().Split(' ');
            string[] necessaryWords = splitCommands.Where(s => IsNecessaryWord(s)).ToArray();
            string[] optionalWords = splitCommands.Where(s => IsOptionalWord(s)).Select(x => x.Substring(1)).ToArray();
            string[] forbiddenWords =
                splitCommands.Where(s => IsForbiddenWord(s)).Select(x => x.Substring(1)).ToArray();
            return SearchIndices(necessaryWords, optionalWords, forbiddenWords);
        }

        private HashSet<Document> SearchIndices(string[] necessaryWords, string[] optionalWords,
            string[] forbiddenWords)
        {
            HashSet<Document> result = GetAllWordsMustIncludeSet(necessaryWords);
            result.UnionWith(GetAtLeastOneWordMustIncludeSet(optionalWords));
            result.ExceptWith(GetAtLeastOneWordMustIncludeSet(forbiddenWords));
            return result;
        }

        private HashSet<Document> GetAllWordsMustIncludeSet(string[] words)
        {
            bool isFirstWord = true;
            HashSet<Document> result = new HashSet<Document>();
            foreach (var word in words)
            {
                if (isFirstWord)
                {
                    result = _searcher.Search(word);
                    isFirstWord = false;
                }
                else
                {
                    result.IntersectWith(_searcher.Search(word));
                }
            }

            return result;
        }

        private HashSet<Document> GetAtLeastOneWordMustIncludeSet(string[] words)
        {
            HashSet<Document> result = new HashSet<Document>();
            foreach (var word in words)
            {
                result.UnionWith(_searcher.Search(word));
            }

            return result;
        }

        private static bool IsOptionalWord(string word)
        {
            return word.StartsWith("+");
        }

        private static bool IsForbiddenWord(string word)
        {
            return word.StartsWith("-");
        }

        private bool IsNecessaryWord(string word)
        {
            return !(IsOptionalWord(word) || IsForbiddenWord(word));
        }
    }
}