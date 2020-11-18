namespace API_RSA.Models
{
    public class Numbers
    {
        /// <summary>
        /// Calcula si el número ingresado es primo
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool Is_Prime(int num)
        {
            int a = 0;
            for (int i = 1; i < (num + 1); i++)
            {
                if (num % i == 0)
                {
                    a++;
                }
            }
            return a == 2 ? true : false;
        }
        /// <summary>
        /// Retorna si el número es demasiado grande para ser calculado en RSA
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool Is_Big(int num)
        {
            return num < 40 ? true : false;
        }
    }
}
