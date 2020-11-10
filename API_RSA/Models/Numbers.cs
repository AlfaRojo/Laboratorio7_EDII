namespace API_RSA.Models
{
    public class Numbers
    {
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
            if (a != 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool Is_Big(int num)
        {
            return num < 100 ? true : false;
        }
    }
}
