using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Phase08.Model
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
    
}