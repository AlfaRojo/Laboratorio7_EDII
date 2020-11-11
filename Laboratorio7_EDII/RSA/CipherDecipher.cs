using System;
using System.IO;
using System.IO.Compression;
using System.Numerics;
using System.Text;

namespace Lab7_EDII.RSA
{
    public class CipherDecipher
    {
        public void CifrarDescifrar(FileStream ArchivoImportado, string Llave, string newName)
        {
            ArchivoImportado.Close();
            string[] claves = Llave.Split(',');
            int e = int.Parse(claves[0]);
            BigInteger mod = new BigInteger(int.Parse(claves[1]));
            using (FileStream archivo = new FileStream(ArchivoImportado.Name, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                var bufferLength = 80;
                var buffer = new byte[bufferLength];
                using (var file = new FileStream(Path.Combine($"RSA", newName + ".txt"), FileMode.OpenOrCreate))
                {
                    using (var reader = new BinaryReader(archivo))
                    {
                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            buffer = reader.ReadBytes(bufferLength);
                            foreach (var item in buffer)
                            {
                                BigInteger valor = new BigInteger(item);
                                var toWrite = Convert.ToByte(Metodo(valor, e, mod));
                                file.WriteByte(toWrite);
                            }
                        }
                        reader.ReadBytes(bufferLength);
                    }
                }
            }
        }

        private BigInteger Metodo(BigInteger original, int e, BigInteger mod)
        {
            BigInteger bytecif = new BigInteger();
            bytecif = original;
            BigInteger div = 0;
            bytecif = BigInteger.Pow(original, e);
            div = bytecif / mod;
            bytecif = bytecif - (div * mod);
            if (bytecif == 0)
            {
                bytecif = mod;
            }
            return bytecif;
        }

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
            string llavePrivada = e.ToString() + "," + N.ToString();
            CreateFile(llavePrivada, "private.key");
            string llavePublica = inverso.ToString() + "," + N.ToString();
            CreateFile(llavePublica, "public.key");
            CompressFile();
        }

        public void CreateFile(string textoResultante, string tipo)
        {
            using (FileStream Archivo = File.Create(@"RSA\" + tipo))
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
