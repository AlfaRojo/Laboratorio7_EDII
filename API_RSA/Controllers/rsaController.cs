using Microsoft.AspNetCore.Mvc;
using API_RSA.Models;
using System.Threading.Tasks;

namespace API_RSA.Controllers
{
    /// <summary>
    /// Cifrado Asimétrico RSA
    /// </summary>
    [Route("api/[controller]")]
    public class rsaController : Controller
    {
        /// <summary>
        /// Generación de llave pública y privada
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        ///<response code="200">Llave privada y pública generadas</response>
        ///<response code="500">Números son valores erroneos</response>
        /// <returns></returns>
        [HttpGet, Route("{p}/{q}")]
        public async Task<ActionResult> Get_KeyAsync(int p, int q)
        {
            if (p != q)
            {
                if (p <= 0 || q <= 0)
                {
                    return StatusCode(500, $"El valor de p:{p} o el valor de q:{q} deben de ser mayores a 0.");
                }
                Numbers numbers = new Numbers();
                if (numbers.Is_Prime(p) && numbers.Is_Prime(q))
                {
                    if (numbers.Is_Big(p) && numbers.Is_Big(q))
                    {
                        FileHandling fileHandling = new FileHandling();
                        fileHandling.Create_Keys(p, q);
                        return File(await System.IO.File.ReadAllBytesAsync($"RSA.zip"), "application/octet-stream", "RSA.zip");
                    }
                    return StatusCode(500, $"El valor de p:{p} y el valor de q:{q} deben de ser menores a 1,000.");
                }
                return StatusCode(500, $"El valor de p:{p} o el valor de q:{q} deben de ser números primos.");
            }
            return StatusCode(500, $"Los valores de p:{p} y q:{q} no pueden ser los mismos");
        }

        /// <summary>
        /// Recibe 
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost, Route("{nombre}")]
        public ActionResult Post_Key(string nombre, Required files)
        {
            FileHandling fileHandling = new FileHandling();
            fileHandling.Cihper_with_Key(files, nombre);
            return Ok();
        }
    }
}
