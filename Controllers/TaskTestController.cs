using Microsoft.AspNetCore.Mvc;
using TestTask.Services;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskTestController : ControllerBase
    {
        public static string LastEmail = "";
        AzureBlobService _blobService;
        public TaskTestController(AzureBlobService service)
        {
            this._blobService = service;
        }

        [HttpGet("email")]
        public async Task<string> GetEmail()
        {
            return LastEmail;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] DataModel file)
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

                LastEmail = file.Email;

                var response = await _blobService.UploadFiles(new List<IFormFile>() { file.File });
                return StatusCode(response.FirstOrDefault().GetRawResponse().Status);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error: {ex.Message}");
            }
        }

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
