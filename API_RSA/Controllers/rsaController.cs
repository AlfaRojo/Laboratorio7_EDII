using Microsoft.AspNetCore.Mvc;
using API_RSA.Models;

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
        /// <returns></returns>
        [HttpGet, Route("{p}/{q}")]
        public ActionResult Get_Key(int p, int q)
        {
            if (p <= 0 || q <= 0)
            {
                return StatusCode(500, $"El valor de p:{p} o el valor de q:{q} deben de ser mayores a 0.");
            }
            Numbers numbers = new Numbers();

            FileHandling fileHandling = new FileHandling();
            var p_Prime = numbers.Is_Prime(p);
            var q_Prime = numbers.Is_Prime(q);
            if (p_Prime && q_Prime)
            {
                fileHandling.Get_Keys(p, q);
                return Ok($"Llaves generadas para {p},{q}");
            }
            else
            {
                return StatusCode(500, $"El valor de p:{p} o el valor de q:{q} deben de ser números primos.");
            }
        }

        /// <summary>
        /// Recibe 
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        [HttpPost, Route("{nombre}")]
        public ActionResult Post_Key(string nombre)
        {
            return Ok();
        }
    }
}
