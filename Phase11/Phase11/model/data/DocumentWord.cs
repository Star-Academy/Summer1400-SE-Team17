namespace Phase11.model.data
{
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