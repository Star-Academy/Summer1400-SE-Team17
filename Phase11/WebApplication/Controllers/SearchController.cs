using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Phase11;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        
        private ISearcher<Document> _searcher;

        public SearchController(SearchService searchService)
        {
            _searcher = searchService.SearchEngine;
        }
        
        [HttpGet]
        public IEnumerable<KeyValuePair<int , string>> Get([FromQuery] string command)
        {
            var searchResults = _searcher.Search(command);
            return searchResults.Take(10).Select(x => new KeyValuePair<int, string>(x.DocumentId, x.Content));
        }
    }
    
}