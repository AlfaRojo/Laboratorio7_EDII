using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace API_RSA.Models
{
    public class Required
    {
        [Required]
        public IFormFile KeyFile { get; set; }
        [Required]
        public IFormFile CipherFile { get; set; }

    }
}
