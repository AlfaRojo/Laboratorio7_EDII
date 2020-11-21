using Lab7_EDII.RSA;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace API_RSA.Models
{
    public class FileHandling
    {
        /// <summary>
        /// Genera las llaves y son guardadas en archivos separados y compresos
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        public void Create_Keys(int p, int q)
        {
            Create_Files_RSA();
            CipherDecipher cipherDecipher = new CipherDecipher();
            var keys = cipherDecipher.Create_Keys(p, q);
            Create_File(keys[0], "private.key");
            Create_File(keys[1], "public.key");
            CompressFile();
            Delete_Files_RSA();
        }

        /// <summary>
        /// Obtiene la llave guardada en el archivo
        /// </summary>
        /// <param name="files"></param>
        /// <param name="fileName"></param>
        public void Cihper_with_Key(Required files, string fileName)
        {
            Create_Files_RSA();
            Create_Files_Upload();
            CipherDecipher cipherDecipher = new CipherDecipher();
            var new_path = Import_FileAsync(files.CipherFile);
            using (var cipherFile = new FileStream(new_path.Result, FileMode.Open))
            {
                cipherDecipher.CifrarDescifrar(cipherFile, get_Key(files.KeyFile), fileName);
            }
            Delete_Files_Upload();
        }

        private void Create_Files_Upload()
        {
            if (!Directory.Exists($"Upload"))
            {
                Directory.CreateDirectory($"Upload");
            }
        }
        private void Create_Files_RSA()
        {
            if (!Directory.Exists($"RSA"))
            {
                Directory.CreateDirectory($"RSA");
            }
        }
        private void Delete_Files_RSA()
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
        private void Delete_Files_Upload()
        {
            DirectoryInfo di = new DirectoryInfo(@"Upload");
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            if (Directory.Exists(@"Upload"))
            {
                Directory.Delete(@"Upload");
            }
        }
        private async Task<string> Import_FileAsync(IFormFile formFile)
        {
            var new_Path = string.Empty;
            var path = Path.Combine($"Upload", formFile.FileName);
            using (var this_file = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(this_file);
                new_Path = Path.GetFullPath(this_file.Name);
            }
            return new_Path;
        }
        private void CompressFile()
        {
            string startPath = @"RSA";
            string zipPath = @"RSA.zip";
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }
            ZipFile.CreateFromDirectory(startPath, zipPath);
        }
        private string get_Key(IFormFile keyFile)
        {
            string key = string.Empty;
            using (var key_file = new StreamReader(keyFile.OpenReadStream()))
            {
                key = key_file.ReadLine();
            }
            return key;
        }
        private void Create_File(string textoResultante, string tipo)
        {
            using (FileStream Archivo = File.Create(@"RSA\" + tipo))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(textoResultante);
                Archivo.Write(info, 0, info.Length);
                byte[] data = new byte[] { 0x0 };
                Archivo.Write(data, 0, data.Length);
            }
        }

    }
}
