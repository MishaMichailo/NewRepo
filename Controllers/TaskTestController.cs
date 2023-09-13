using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTask.Services;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskTestController : ControllerBase
    {
        AzureBlobService _service;
        public TaskTestController(AzureBlobService service)
        {

            this._service = service;

        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm]EmailModel file)
        {
            try
            {

                if (file == null)
                {
                    return BadRequest("Please select a file to upload.");
                }

                if (!file.File.FileName.EndsWith(".docx"))
                {
                    return BadRequest("Please select a valid .docx file.");
                }

                if (!IsValidEmail(file.Email))
                {
                    return BadRequest("Invalid email address.");
                }
                var response = await _service.UploadFiles(new List<IFormFile>() { file.File });

                return Ok("File uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        
        [HttpGet("area")]
        public IActionResult Area(int altitude, int height)
        {
            double area = altitude * height / 2;
            return Content($"Площадь треугольника с основанием {altitude} и высотой {height} равна {area}");
        }
        [HttpPost]
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
} 
