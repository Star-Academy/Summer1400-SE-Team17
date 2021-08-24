using Phase11;
using Phase11.model;
using Phase11.model.data;

namespace WebApplication.Services
{
    public class SearchService
    {
        private const string DataLocation = "EnglishData/";
        private readonly ISearcher<Document> _searcher;

        public ISearcher<Document> SearchEngine => _searcher;

        public SearchService()
        {
            var indexSearcher = new InvertedIndexSearcher();
            var searchEngine = new SearchEngine();
            indexSearcher.LoadDatabase(DataLocation);
            searchEngine.Searcher = indexSearcher;
            _searcher = searchEngine;
        }
    }
}