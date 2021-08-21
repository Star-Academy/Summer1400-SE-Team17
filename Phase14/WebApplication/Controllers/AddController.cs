using System;
using Microsoft.AspNetCore.Mvc;
using Phase11;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddDocController : ControllerBase
    {
        private DataService _dataService;
        private Database _database;
        public AddDocController(DataService dataService)
        {
            _dataService = dataService;
            _database = dataService.Database;
        }
        [HttpPut]
        public void Put([FromBody] Document1 document)
        {
            Console.WriteLine(document.Content);
            // _database.Documents.Add(document);
            // _database.SaveData();
        }
    }


    public class Document1
    {
        public int DocumentIndex { get; set; }
        public string Content { get; set; }
    }
    
}