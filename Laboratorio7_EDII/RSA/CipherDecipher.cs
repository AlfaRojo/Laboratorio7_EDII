using System;
using System.IO;
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
            int key = int.Parse(claves[0]);
            var n = int.Parse(claves[1]);
            using (var file_Cipher = new FileStream(ArchivoImportado.Name, FileMode.Open, FileAccess.ReadWrite))
            {
                var bufferLength = 80;
                var buffer = new byte[bufferLength];
                using (var Writer = new StreamWriter(Path.Combine($"RSA", newName + ".txt"), true))
                {
                    using (var reader = new BinaryReader(file_Cipher))
                    {
                        var left_over = string.Empty;
                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            buffer = reader.ReadBytes(bufferLength);
                            var buffer_write = new byte[buffer.Length];
                            foreach (var bytetxt in buffer)
                            {
                                BigInteger new_Text = module_Text(bytetxt, key, n);
                                if (new_Text < 0)
                                {
                                    new_Text += n;
                                }
                                var bin_Length = left_over + ToNBase(new_Text, 2);
                                string texter = string.Empty;
                                if (bin_Length.Length > 8)
                                {
                                    texter = bin_Length.Substring(0, 8);
                                    left_over = bin_Length.Replace(texter, string.Empty);
                                    var text_ASCII = (char)Convert.ToInt32(texter, 2);
                                    Writer.Write(text_ASCII);
                                }
                                else
                                {
                                    var text_ASCII = ToNBase(Convert.ToInt32(bin_Length, 2), 2);
                                    var new_text = (char)Convert.ToInt32(text_ASCII, 2);
                                    Writer.Write(new_text);
                                }
                                if (left_over.Length >= 8)
                                {
                                    texter = left_over.Substring(0, 8);
                                    left_over = left_over.Replace(texter, string.Empty);
                                    var text_ASCII = (char)Convert.ToInt32(texter, 2);
                                    Writer.Write(text_ASCII);
                                }
                            }
                            if (!left_over.Equals(""))
                            {
                                var text_ASCII = ToNBase(Convert.ToInt32(left_over, 2), 2);
                                var new_text = (char)Convert.ToInt32(text_ASCII, 2);
                                Writer.Write(new_text);
                            }
                        }
                    }
                }
            }
        }

        private static string ToNBase(BigInteger a, int n)
        {
            StringBuilder sb = new StringBuilder();
            while (a > 0)
            {
                sb.Insert(0, a % n);
                a /= n;
            }
            return sb.ToString();
        }
        public string[] Create_Kyes(int primo1, int primo2)
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
            string[] keys = { llavePrivada, llavePublica };
            return keys;
        }
        private BigInteger module_Text(byte text, int key, int mod)
        {
            return BigInteger.ModPow(text, key, mod);
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
            for (int i = value; i < 10000; i++)
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
