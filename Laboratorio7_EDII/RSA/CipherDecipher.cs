using System.IO;
using System.IO.Compression;
using System.Text;

namespace Lab7_EDII.RSA
{
    public class CipherDecipher
    {
        public void CreacionLlaves(int primo1, int primo2)
        {
            int phi = (primo1 - 1) * (primo2 - 1);
            int N = primo1 * primo2;
            int e = Encontare(phi, (phi - 2));
            int inverso = InversoMultiplicativo(phi, e, 1, 0, 0, 1, 0, 0, 0, 0, 0);
            if (inverso < 0)
            {
                inverso = phi + inverso;
            }
            string llavePrivada = e.ToString();
            CreateFile(llavePrivada, "Llave_Privada");
            string llavePublica = inverso.ToString();
            CreateFile(llavePublica, "Llave_Publica");
            CompressFile();
        }

        public void CreateFile(string textoResultante, string tipo)
        {
            using (FileStream Archivo = File.Create(@"RSA\" + tipo + ".txt"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(textoResultante);
                Archivo.Write(info, 0, info.Length);
                byte[] data = new byte[] { 0x0 };
                Archivo.Write(data, 0, data.Length);
            }
        }
        public void CompressFile()
        {
            string startPath = @"RSA";
            string zipPath = @"RSA.zip";
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }
            ZipFile.CreateFromDirectory(startPath, zipPath);
        }
        private int InversoMultiplicativo(int g0, int g1, int u0, int u1, int v0, int v1, int iteracion, int entero, int ag, int au, int av)
        {
            if (g1 != 0)
            {
                entero = g0 / g1;
                ag = g1;
                g1 = g0 - (entero * g1);
                au = u1;
                u1 = u0 - (entero * u1);
                av = v1;
                v1 = v0 - (entero * v1);
                iteracion++;
                return InversoMultiplicativo(ag, g1, au, u1, av, v1, iteracion, entero, ag, au, av);
            }
            return v0;
        }

        private int Encontare(int phi, int e)
        {
            bool verificacion = VerificacionCoPrimos(phi, e);
            if (!verificacion)
            {
                return Encontare(phi, e - 1);
            }
            return e;
        }

        private bool VerificacionCoPrimos(int phi, int e)
        {
            int sumando = phi % e;
            if (sumando != 0)
            {
                return VerificacionCoPrimos(e, sumando);
            }
            if (e == 1)
            {
                return true;
            }
            return false;
        }
    }
}
