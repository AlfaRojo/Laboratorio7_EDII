using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace API_RSA.Models
{
    public class Required
    {
        [Required]
        public IFormFile keyFile { get; set; }
        [Required]
        public IFormFile cipherFile { get; set; }

    }
}
