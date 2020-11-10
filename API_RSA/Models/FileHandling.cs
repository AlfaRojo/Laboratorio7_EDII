using Lab7_EDII.RSA;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace API_RSA.Models
{
    public class FileHandling
    {
        private void Create_Files()
        {
            if (!Directory.Exists($"RSA"))
            {
                Directory.CreateDirectory($"RSA");
            }
        }
        private void Delete_Files()
        {
            DirectoryInfo di = new DirectoryInfo(@"RSA");
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            if (Directory.Exists(@"RSA"))
            {
                Directory.Delete(@"RSA");
            }
        }

        /// <summary>
        /// Genera las llaves y son guardadas en archivos separados y compresos
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        public void Create_Keys(int p, int q)
        {
            Create_Files();
            CipherDecipher cipherDecipher = new CipherDecipher();
            cipherDecipher.CreacionLlaves(p, q);
            Delete_Files();
        }

        /// <summary>
        /// Obtiene la llave guardada en el archivo
        /// </summary>
        /// <param name="files"></param>
        public void Get_Key_From_File(Required files)
        {
            Create_Files();

            Delete_Files();
        }
    }
}
