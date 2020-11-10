using Microsoft.AspNetCore.Http;

namespace API_RSA.Models
{
    public class Required
    {
        public IFormFile keyFile { get; set; }
        public IFormFile cipherFile { get; set; }

    }
}
