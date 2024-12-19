using Microsoft.AspNetCore.Mvc;
using TxtServerAsAPIDocker.Models;

namespace TxtServerAsAPIDocker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly ILogger<DocumentController> _logger;

        public DocumentController(ILogger<DocumentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("List")]
        public IEnumerable<DocumentData> List()
        {
            var files = Directory.GetFiles("App_Data");
            return files.Select(file => new DocumentData
            {
                Name = Path.GetFileName(file),
            });
        }

        [HttpGet]
        [Route("Load")]
        public ActionResult<DocumentData> Load(string name)
        {
            var file = Path.Combine("App_Data", name);
            if (!System.IO.File.Exists(file))
            {
                return NotFound();
            } 

            var data = new DocumentData 
            {
                Name = name,
                Data = Convert.ToBase64String(System.IO.File.ReadAllBytes(file)),
            };
            return data;
        }
        
        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody] DocumentData documentData)
        {
            if (documentData == null || string.IsNullOrEmpty(documentData.Name) || string.IsNullOrEmpty(documentData.Data))
            {
                return BadRequest();
            }

            var file = Path.Combine("App_Data", documentData.Name);
            System.IO.File.WriteAllBytes(file, Convert.FromBase64String(documentData.Data));
            return Ok();
        }
    }
}
