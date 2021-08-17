using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Phase05
{
    public class Document
    {
        [Required]
        public int DocumentId { get; set; }
        [Required]
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
        [Required]
        public int WordId { get; set; }
        [Required]
        [MaxLength(64)]
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