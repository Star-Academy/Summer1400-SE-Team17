using System.Collections.Generic;
using Phase11.model.data;
using Phase11.model.database;
using Phase11.model.file_loader;
using Phase11.model.parser;

namespace Phase11.model
{
    public class InvertedIndexSearcher : ISearcher<Document>
    {
        private IDataBase<Document, string, Document> _dictionary;
        private IParser<string> _wordParser = new DefaultWordParser();
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

        public void LoadDatabase(string path)
        {
            var dictionary = new DataBase();
            _dictionary = dictionary;
            if (dictionary.Database.EnsureCreated())
            {
                InitData(path);
            }
        }

        private void InitData(string path)
        {
            HashSet<Document> rawData = _fileLoader.Load(path);
            foreach (var document in rawData)
            {
                AddDocumentIndexToDatabase(document);
            }

            _dictionary.SaveData();
        }

        private void AddDocumentIndexToDatabase(Document document)
        {
            string documentContent = document.Content;
            string[] parsedDocument = _documentParser.Parse(documentContent);
            foreach (var word in parsedDocument)
            {
                if (!_dictionary.ContainsKey(word))
                    _dictionary.AddWord(word);
                _dictionary.AddDocumentWord(word, document);
            }
        }

        public HashSet<Document> Search(string word)
        {
            word = _wordParser.Parse(word);
            return _dictionary.Get(word);
        }
    }
}