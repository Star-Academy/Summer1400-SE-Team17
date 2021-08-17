using Microsoft.AspNetCore.Mvc;

namespace Phase11.Controllers
{
    [ApiController]
        [Route("[controller]/[Action]")]
        public class SimpleController : ControllerBase
        {
            [HttpGet]
            public IActionResult Get()
            {
                return base.Content("<!DOCTYPE html>\n<html>\n<body>\n<h1>My First Heading</h1>\n<p>My first paragraph.</p>\n\t\n</body>\n</html>","text/html");
            }
        }
}