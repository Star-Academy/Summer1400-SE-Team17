namespace Phase11.model.parser
{
    public class DefaultWordParser : IParser<string>
    {
        private PorterStemmer _stemmer = new PorterStemmer();

        public string Parse(string text)
        {
            
            return _stemmer.StemWord(text);
        }

    }
}