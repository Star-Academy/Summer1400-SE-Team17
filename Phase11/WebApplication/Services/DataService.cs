using Phase11;

namespace WebApplication.Services
{
    public class DataService
    {
        private const string DataLocation = "EnglishData/";
        private readonly Database _database = new Database();
        private readonly SearchEngine _searcher;
        public Database Database => _database;
        public SearchEngine SearchEngine => _searcher;

        public DataService()
        {
            var indexSearcher= new InvertedIndexSearcher();
            var searchEngine = new SearchEngine();
            indexSearcher.LoadDatabase(DataLocation);
            searchEngine.Searcher = indexSearcher;
            _searcher = searchEngine;
        }
    }
}