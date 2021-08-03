using System.Collections.Generic;
using com.sun.org.apache.regexp.@internal;

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
            throw new System.NotImplementedException();
        }
    }

    public class InvertedIndexSearcher : ISearcher<int>
    {
        private Dictionary<string, HashSet<int>> _dictionary;
        private IFileLoader<Dictionary<string,HashSet<int>>> _fileLoader = new DictionaryLoader();
        
        public IFileLoader<Dictionary<string,HashSet<int>>> FileLoader
        {
            set => _fileLoader = value;
        }

        public void LoadDictionary(string path)
        {
            _dictionary = _fileLoader.Load(path);
        }
        
        public HashSet<int> Search(string word)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Document
    {
        public int DocumentIndex { get; }
        public string Content { get; }

        public Document(int documentIndex,string content)
        {
            DocumentIndex = documentIndex;
            Content = content;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType().IsInstanceOfType(typeof(Document)))
            {
                Document doc = (Document) obj;
                return DocumentIndex == doc.DocumentIndex;
            }
            return false;

        }
    }

    
    public class DictionaryLoader : IFileLoader<Dictionary<string,HashSet<int>>>
    {
        public Dictionary<string, HashSet<int>> Load(string path)
        {
            throw new System.NotImplementedException();
        }
    }
    
    

}