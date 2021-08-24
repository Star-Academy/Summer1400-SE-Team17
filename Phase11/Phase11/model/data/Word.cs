using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Phase11.model.data
{
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
}