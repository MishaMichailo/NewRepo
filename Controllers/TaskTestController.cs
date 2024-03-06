using Microsoft.AspNetCore.Mvc;
using TestTask.Models;
using TestTask.Services;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskTestController : ControllerBase
    {
        AzureBlobService _blobService;
        public TaskTestController(AzureBlobService service)
        {
            this._blobService = service;
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

                var response = await _blobService.UploadFiles(new List<IFormFile>() { file.File });

                await _blobService.AddMetadataToFile(file.File.FileName,file.Email);

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
