using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Lab7_EDII.RSA
{
    public class CipherDecipher
    {
        List<char> list_Bin = new List<char>();
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
                using (var Writer = new BinaryWriter(File.OpenWrite(Path.Combine($"RSA", newName + ".txt"))))
                {
                    using (var reader = new BinaryReader(file_Cipher))
                    {
                        var max_Length = Convert.ToString(n, 2).Length;

                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            buffer = reader.ReadBytes(bufferLength);
                            var buffer_write = new byte[buffer.Length];
                            foreach (var bytetxt in buffer)
                            {
                                var new_Text = module_Text(bytetxt, key, n);
                                if (new_Text < 0)
                                {
                                    new_Text += n;
                                }
                                var bin_Length = Convert.ToString((byte)new_Text, 2);
                                if (max_Length > bin_Length.Length)
                                {
                                    bin_Length = Complete_bin(max_Length, bin_Length);
                                }
                                uint result = Convert.ToUInt32(bin_Length, 2);

                                var text_ASCII = (char)result;
                                list_Bin.Add(text_ASCII);
                            }
                        }
                        reader.ReadBytes(bufferLength);
                        foreach (var item in list_Bin)
                        {
                            Writer.Write(item);
                        }
                    }
                }
            }
        }
        public string[] Create_Keys(int primo1, int primo2)
        {
            int phi = (primo1 - 1) * (primo2 - 1);
            int n = primo1 * primo2;
            int e = get_E(phi, n);
            int d = modInverse(e, phi);

            if (d < 0)
            {
                d += phi;
            }
            string llavePrivada = d.ToString() + "," + n.ToString();
            string llavePublica = e.ToString() + "," + n.ToString();
            return new string[] { llavePrivada, llavePublica };
        }

        private string Complete_bin(int size, string actual)
        {
            while (size > actual.Length)
            {
                StringBuilder stringBuilder = new StringBuilder("0");
                stringBuilder.Append(actual);
                actual = stringBuilder.ToString();
            }
            return actual;
        }

        private long module_Text(byte text, int key, int mod)
        {
            long module = (long)BigInteger.ModPow(text, key, mod);
            return module;
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

        private int get_E(int phi, int n)
        {
            var rand = new Random();
            int value = rand.Next(2, n);
            for (int i = value; i < n + 100; i++)
            {
                if (isPrime(i))
                {
                    if (EsPrimoRelativo(i, phi - 2))
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

    }
}