using Lab7_EDII.RSA;
using System.IO;

namespace API_RSA.Models
{
    public class FileHandling
    {
        public void Create_Files()
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
        public void Get_Keys(int p, int q)
        {
            Create_Files();
            CipherDecipher cipherDecipher = new CipherDecipher();
            cipherDecipher.CreacionLlaves(p, q);
            Delete_Files();
        }
    }
}
