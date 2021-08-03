
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Parser;

namespace Phase05
{
    public class SearchEngine : ISearcher<Document>
    {
        private ISearcher<int> _searcher = new InvertedIndexSearcher();

        public ISearcher<int> Searcher
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
            return GetDocumentsFromIndices(SearchIndices(necessaryWords, optionalWords, forbiddenWords));
        }

        private HashSet<Document> GetDocumentsFromIndices(HashSet<int> indices)
        {
            HashSet<Document> result = new HashSet<Document>();
            foreach (var index in indices)
            {
                Document document = new Document(index, "");
                result.Add(document);
            }

            return result;
        }

        private HashSet<int> SearchIndices(string[] necessaryWords, string[] optionalWords, string[] forbiddenWords)
        {
            HashSet<int> result = GetAllWordsMustIncludeSet(necessaryWords);
            result.UnionWith(GetAtLeastOneWordMustIncludeSet(optionalWords));
            result.ExceptWith(GetAtLeastOneWordMustIncludeSet(forbiddenWords));
            return result;
        }

        private HashSet<int> GetAllWordsMustIncludeSet(string[] words)
        {
            bool isFirstWord = true;
            HashSet<int> result = new HashSet<int>();
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

        private HashSet<int> GetAtLeastOneWordMustIncludeSet(string[] words)
        {
            HashSet<int> result = new HashSet<int>();
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

    public class InvertedIndexSearcher : ISearcher<int>
    {
        private Dictionary<string, HashSet<int>> _dictionary = new Dictionary<string, HashSet<int>>();
        private IParser<string> _wordParser = new WordParser();
        private IParser<string[]> _documentParser = new DocumentParser();
        private IFileLoader<HashSet<Document>> _fileLoader = new DictionaryLoader();

        public IFileLoader<HashSet<Document>> FileLoader
        {
            set => _fileLoader = value;
        }

        public IParser<string> WordParser
        {
            set => _wordParser = value;
        }

        public IParser<string[]> DocumentParser
        {
            set => _documentParser = value;
        }

        public void LoadDictionary(string path)
        {
            HashSet<Document> rawData = _fileLoader.Load(path);
            foreach (var document in rawData)
            {
                AddDocumentIndexToDictionary(document);
            }
        }

        private void AddDocumentIndexToDictionary(Document document)
        {
            string documentContent = document.Content;
            int documentIndex = document.DocumentIndex;
            string[] parsedDocument = _documentParser.Parse(documentContent);
            foreach (var word in parsedDocument)
            {
                if (!_dictionary.ContainsKey(word))
                    _dictionary.Add(word, new HashSet<int>());
                _dictionary[word].Add(documentIndex);
            }
        }

        public HashSet<int> Search(string word)
        {
            word = _wordParser.Parse(word);
            return _dictionary[word];
        }
    }

    public class Document
    {
        protected bool Equals(Document other)
        {
            return DocumentIndex == other.DocumentIndex;
        }

        public int DocumentIndex { get; }
        public string Content { get; }

        public Document(int documentIndex, string content)
        {
            DocumentIndex = documentIndex;
            Content = content;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Document) obj);
        }

        public override int GetHashCode()
        {
            return DocumentIndex;
        }
    }


    public class DictionaryLoader : IFileLoader<HashSet<Document>>
    {
        public HashSet<Document> Load(string path)
        {
            HashSet<Document> result = new HashSet<Document>();
            foreach (var fileRelativePath in Directory.EnumerateFiles(path))
            {
                string fileName = fileRelativePath.Substring(fileRelativePath.LastIndexOf('/') + 1);
                string fileContent = File.ReadAllText(fileRelativePath);
                result.Add(new Document(int.Parse(fileName), fileContent));
            }
            return result;
        }
    }
}