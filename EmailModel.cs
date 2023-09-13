using Microsoft.AspNetCore.Http;

namespace TestTask
{
    public class EmailModel
    {
        public IFormFile File { get; set; }
        public string? Email { get; set; }



    }
}
