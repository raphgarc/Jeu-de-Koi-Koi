using System;
using Keiwando.BigInteger;
namespace Transverse_IA
{
    public static class MyMath
    {
        public static BigInteger factoriel(int a)
        {
            if (a < 0)
                throw (new ArgumentOutOfRangeException("a", a, "Impossible to calculate a factorial of a number lower than 0"));
            if (a == 0)
                return (1);
            return (factoriel(a - 1) * a);
        }

        public static int combinaison(int p, int parmi_n)
        {
            if (p < 0)
                throw (new ArgumentOutOfRangeException("p", p, "P can't be lower than 0"));
            else if (parmi_n < 0)
                throw (new ArgumentOutOfRangeException("parmi_n", parmi_n, "Parmi_n can't be lower than 0"));
            else if (p > parmi_n)
            {
                throw (new ArgumentOutOfRangeException("(parmi_n - p)", parmi_n - p, "Parmi_n can't be bigger than p"));
            }
			//else
			int tmp = BigInteger.ToInt32((factoriel(parmi_n) / (factoriel(parmi_n - p) * factoriel(p))));
			return tmp;
        }
    }
}
