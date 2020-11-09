using Microsoft.AspNetCore.Mvc;

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
            int privateKey = 0;
            int publicKey = 0;
            return Ok($"{privateKey},{publicKey}");
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
