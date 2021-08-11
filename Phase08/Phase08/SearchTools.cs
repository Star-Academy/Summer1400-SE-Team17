using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Parser;

namespace Phase05
{
    public class SearchEngine : ISearcher<Document>
    {
        private ISearcher<Document> _searcher = new InvertedIndexSearcher();

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

    public class InvertedIndexSearcher : ISearcher<Document>
    {
        private IDataBase<Document, string, Document> _dictionary;
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

        public void LoadDatabase(string path)
        {
            var dictionary = new Database();
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

    public class Database : DbContext, IDataBase<Document, string, Document>
    {
        private static string _connectionString = !RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? @"Server=localhost,1433; Database=Phase08Db; User=sa; Password=0150107021@;Trusted_Connection=True;"
            : @"Server=localhost;Database=Phase08;Trusted_Connection=True;";

        public DbSet<Document> Documents { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<DocumentWord> DocumentsWords { get; set; }
        private Dictionary<string, Word> NameToWord = new Dictionary<string, Word>();

        public bool ContainsKey(string o)
        {
            return NameToWord.ContainsKey(o);
        }

        public HashSet<Document> Get(string o)
        {
            var wordIds = Words.Where(x => x.Content == o).Select(x => x.WordId).ToList();
            if (wordIds.Any())
            {
                var wordId = wordIds[0];
                return getDocumentsOfId(wordId);
            }

            return new HashSet<Document>();
        }

        public void AddWord(string word)
        {
            Word w = new Word(word);
            NameToWord[word] = w;
            Words.Add(w);
        }

        public void AddDocumentWord(string wordName, Document document)
        {
            Word word = NameToWord[wordName];
            DocumentWord documentWord = new DocumentWord(document, word);
            document.DocumentWords.Add(documentWord);
            word.DocumentWords.Add(documentWord);
        }

        public void SaveData()
        {
            SaveChanges();
        }

        private HashSet<Document> getDocumentsOfId(int wordId)
        {
            return DocumentsWords.Where(d => d.WordId == wordId).Select(d => d.Document).ToHashSet();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>().HasKey(d => new {d.DocumentId});
            modelBuilder.Entity<Word>().HasKey(w => new {w.WordId});
            modelBuilder.Entity<DocumentWord>().HasKey(word => new {word.DocumentId, word.WordId});
        }
    }

    public class Document
    {
        public int DocumentId { get; set; }
        public int DocumentIndex { get; set; }
        public string Content { get; set; }
        public IList<DocumentWord> DocumentWords { get; set; }

        public Document(int documentIndex, string content)
        {
            DocumentIndex = documentIndex;
            Content = content;
            DocumentWords = new List<DocumentWord>();
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

        protected bool Equals(Document other)
        {
            return DocumentIndex == other.DocumentIndex;
        }
    }

    public class Word
    {
        public int WordId { get; set; }
        public string Content { get; set; }
        public IList<DocumentWord> DocumentWords { get; set; }

        public Word(string content)
        {
            DocumentWords = new List<DocumentWord>();
            Content = content;
        }
    }

    public class DocumentWord
    {
        public int DocumentId { get; set; }
        public Document Document { get; set; }
        public int WordId { get; set; }
        public Word Word { get; set; }

        public DocumentWord(Document document, Word word)
        {
            Document = document;
            Word = word;
        }

        public DocumentWord()
        {
        }
    }
}