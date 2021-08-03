using Phase05;
using edu.stanford.nlp;
namespace Parser
{
    public class WordParser : IParser<string>
    {
        public string Parse(string text)
        {
           
            throw new System.NotImplementedException();
        }
        
    }
    
    public class SentenceParser : IParser<string[]>
    {
        private IParser<string> _wordParse = new WordParser();
        public IParser<string> WordParser
        {
            set => _wordParse = value;
        }
        public string[] Parse(string text)
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class DocumentParser : IParser<string[]>
    {
        private IParser<string[]> _sentenceParser = new SentenceParser();
        public IParser<string[]> SentenceParser
        {
            set => _sentenceParser = value;
        }
        public string[] Parse(string text)
        {
            throw new System.NotImplementedException();
        }
    }
}




//Package bandi??
//Resource??
// new Interface
// exception handling
//Export Method is Clean??

//string a = Parse(a: 1, text: "11");

//  public string Parse(string text,int a)
// {
//     throw new System.NotImplementedException();
// }
// public string Parse(int a,string text)
// {
//     throw new System.NotImplementedException();
// }

//yield return!?

//wild card generic?

//import java????
//java awt????