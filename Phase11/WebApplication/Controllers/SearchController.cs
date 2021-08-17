using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Phase11;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("search/[controller]/[action]")]
    public class SearchController : ControllerBase
    {
        private static string DataLocation = "EnglishData/";
        private static ISearcher<Document> _searcher;
        private static ISearcher<Document> _invertedIndexSearcher;
        static SearchController()
        {
            SearchEngine searchEngine = new SearchEngine();
            InvertedIndexSearcher indexSearcher =  new InvertedIndexSearcher();
            indexSearcher.LoadDatabase(DataLocation);
            searchEngine.Searcher = indexSearcher;

            _searcher = searchEngine;
            _invertedIndexSearcher = indexSearcher;
        }

        
        [HttpGet]
        public IEnumerable<KeyValuePair<int , string>> Search([FromQuery] string command)
        {
            var searchResults = _searcher.Search(command);
            return searchResults.Take(10).Select(x => new KeyValuePair<int, string>(x.DocumentId, x.Content));
        }
    }
    
}