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
        ///<response code="200">Llave privada y pública generadas</response>
        ///<response code="500">Números son valores erroneos</response>
        /// <returns></returns>
        [HttpGet, Route("{p}/{q}")]
        public ActionResult Get_Key(int p, int q)
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
                    fileHandling.Get_Keys(p, q);
                    return Ok($"Llaves generadas para {p},{q}");
                }
                return StatusCode(500, $"El valor de p:{p} y el valor de q:{q} deben de ser menores a 1,000.");
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
