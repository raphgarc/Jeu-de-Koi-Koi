using System;
namespace Transverse_IA.MyProba
{
    //https://www.dcode.fr/probabilites-tirage
    // k : Le nombre d'élément à avoir
    // n : Le nombre de carte que le joueur possède dans sa main
    // m : Nombre d'éléments restant dans le jeu
    // pi : Nombre de carte dans la pioche

    public static class avoir_k_cartes
    {
        public static double exactement(int k, int m, int n, int pi)
        {
            int N = n + pi;
            int numerateur = 0;
            int denominateur = 0;
            try
            {
                numerateur = MyMath.combinaison(k, m) * MyMath.combinaison(n - k, N - m);
                denominateur = MyMath.combinaison(n, N);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Error.WriteLine("\t" + e.Message.Replace("\n", "\n\t"));
                return (0);
            }
            return ((double)numerateur / denominateur);
        }

        public static class moins_de
        {
            public static double strict(int k, int m, int n, int pi)
            {
                double toRet = 0;
                for (int i = 0; i < k; i++)
                {
                    toRet += MyProba.avoir_k_cartes.exactement(i, m, n, pi);
                }
                return (toRet);
            }

            public static double inclusif(int k, int m, int n, int pi)
            {
                double toRet = 0;

                toRet = MyProba.avoir_k_cartes.moins_de.strict(k, m, n, pi);
                toRet += MyProba.avoir_k_cartes.exactement(k, m, n, pi);

                return toRet;
            }
        }

        public static class plus_de
        {
            public static double strict(int k, int m, int n, int pi)
            {
                return (1 - moins_de.inclusif(k, m, n, pi));
            }

            public static double inclusif(int k, int m, int n, int pi)
            {
                return (1 - moins_de.strict(k, m, n, pi));
            }
        }
    }
}