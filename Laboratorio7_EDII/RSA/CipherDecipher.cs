using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Numerics;
using System.Text;

namespace Lab7_EDII.RSA
{
    public class CipherDecipher
    {
        List<string> list_Bin = new List<string>();
        public void CifrarDescifrar(FileStream ArchivoImportado, string Llave, string newName)
        {
            ArchivoImportado.Close();
            string[] claves = Llave.Split(',');
            int key = int.Parse(claves[0]);
            var n = int.Parse(claves[1]);
            using (var file_Cipher = new FileStream(ArchivoImportado.Name, FileMode.Open, FileAccess.ReadWrite))
            {
                var bufferLength = 80;
                var buffer = new byte[bufferLength];
                int longer = 0; //Guardará el numero en binario mas largo
                using (var Writer = new BinaryWriter(File.OpenWrite(Path.Combine($"RSA", newName + ".txt"))))
                {
                    using (var reader = new BinaryReader(file_Cipher))
                    {
                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            buffer = reader.ReadBytes(bufferLength);
                            var buffer_write = new byte[buffer.Length];
                            foreach (var bytetxt in buffer)
                            {
                                var binary = Convert.ToString(bytetxt, 2);
                                var test = module_Text(bytetxt, key, n);
                                if (longer < test)
                                {
                                    longer = (int)test;
                                }
                                else
                                {
                                    var maxBit = Convert.ToString(longer, 2);
                                    binary = Complete_bin(maxBit.Length, binary);
                                }
                                list_Bin.Add(binary);
                            }
                            //Escribir la lista en ASCII
                        }
                        reader.ReadBytes(bufferLength);
                    }
                }
            }
        }

        private string Complete_bin(int size, string actual)
        {
            List<string> vs = new List<string>();
            while (size != actual.Length)
            {
                StringBuilder stringBuilder = new StringBuilder("0");
                stringBuilder.Append(actual);
                actual = stringBuilder.ToString();
            }
            foreach (var item in list_Bin)
            {
                var new_value = item;
                while (size != new_value.Length)
                {
                    StringBuilder stringBuilder = new StringBuilder("0");
                    stringBuilder.Append(new_value);
                    new_value = stringBuilder.ToString();
                }
                vs.Add(new_value);
            }
            list_Bin = vs;
            return actual;
        }

        private BigInteger module_Text(byte text, int key, int mod)
        {
            BigInteger module = (BigInteger)Math.Pow(text,key) % mod;
            return module;
        }

        public void CreacionLlaves(int primo1, int primo2)
        {
            int phi = (primo1 - 1) * (primo2 - 1);
            int n = primo1 * primo2;
            int e = get_E(phi);
            int d = modInverse(e, phi);
            if (d < 0)
            {
                d += phi;
            }
            string llavePrivada = e.ToString() + "," + n.ToString();
            string llavePublica = d.ToString() + "," + n.ToString();
            CreateFile(llavePrivada, "private.key");
            CreateFile(llavePublica, "public.key");
            CompressFile();
        }

        private int modInverse(int a, int n)
        {
            int i = n, v = 0, d = 1;
            while (a > 0)
            {
                int t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }

        private int get_E(int phi)
        {
            var rand = new Random();
            int value = rand.Next(10,100);
            for (int i = value; i < 10000; i++)
            {
                if (isPrime(i))
                {
                    if (EsPrimoRelativo(i, phi))
                    {
                        return i;
                    }
                }
            }
            return 0;
        }

        private static bool isPrime(int number)
        {
            for (int i = 2; i < number; i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool EsPrimoRelativo(int numero1, int numero2)
        {
            int resto;
            while (numero2 != 0)
            {
                resto = numero1 % numero2;
                numero1 = numero2;
                numero2 = resto;
            }
            return numero1 == 1 || numero1 == -1;
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
    }
}
