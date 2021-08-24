using System;
using Microsoft.AspNetCore.Mvc;
using Phase11;
using Phase11.model.database;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddDocController : ControllerBase
    {
        private DataService _dataService;
        private DataBase _dataBase;
        public AddDocController(DataService dataService)
        {
            _dataService = dataService;
            _dataBase = dataService.DataBase;
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