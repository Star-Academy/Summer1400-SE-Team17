using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace Phase05
{
       public class Database : DbContext, IDataBase<Document, string, Document>
    {
        private static string _connectionString = !RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? @"Server=localhost,1433; Database=Phase08Db; User=sa; Password=0150107021@;"
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

}