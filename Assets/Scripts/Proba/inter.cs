using System;
namespace Transverse_IA.MyProba
{
    //https://www.dcode.fr/probabilites-tirage
    // k : Le nombre d'élément à avoir
    // n : Le nombre de carte que le joueur possède dans sa main
    // m : Nombre d'éléments restant dans le jeu
    // pi : Nombre de carte dans la pioche


    public static class inter
    {
        private static double proba_inter(int k_a, int k_b, int m_a, int m_b, int n, int pi)
        {
            double p_b = MyProba.avoir_k_cartes.exactement(k_b, m_b, n, pi);
            // Quand on sait que b va se produire (exemple b = avoir 1 lumière parmi 4) :
            // Et que l'on veut calculer a, : il y a un piège :
            // - Mathématiquement, c'est comme si l'autre joueur n'avait plus 8 cartes mais 7 dans sa main.
            // - Et il n'y a plus 24 cartes dans la pioche mais 20 (il faut cependant ajouter k_b car ici, pi ne représente pas vraiment la pioche mais le nombre de carte inconnu (hors carte de l'adversaire). Ce cas particulier a donc créé une micro erreur de conception dans le nom des variables.

            double p_a = MyProba.avoir_k_cartes.exactement(k_a, m_a, n - k_b, pi - m_b + k_b);


            return (p_b * p_a);
        }
        private static double proba_inter(double p_b, int k_a, int k_b, int m_a, int m_b, int n, int pi)
        {
            double p_a = MyProba.avoir_k_cartes.exactement(k_a, m_a, n - k_b, pi - m_b);

            return (p_b * p_a);
        }

        public static double exact(int k_a, int k_b, int m_a, int m_b, int n, int pi)
        {
            return proba_inter(k_a, k_b, m_a, m_b, n, pi);
        }

        public static double exact(double p_b, int k_a, int k_b, int m_a, int m_b, int n, int pi)
        {
            return proba_inter(p_b, k_a, k_b, m_a, m_b, n, pi);
        }

        public static class moins_de
        {
            public static double strict(int k_a, int k_b, int m_a, int m_b, int n, int pi)
            {
                double toRet = 0;
                for (int i = 0; i < k_a; i++)
                {
                    for (int j = 0; j < k_b; j++)
                    {
                        toRet += proba_inter(i, j, m_a, m_b, n, pi);
                    }
                }
                return toRet;
            }

            public static double inclusif(int k_a, int k_b, int m_a, int m_b, int n, int pi)
            {
                double toRet = 0;
                for (int i = 0; i <= k_a; i++)
                {
                    for (int j = 0; j <= k_b; j++)
                    {
                        toRet += proba_inter(i, j, m_a, m_b, n, pi);
                    }
                }
                return toRet;
            }
        }

        public static class plus_de
        {
            public static double strict(int k_a, int k_b, int m_a, int m_b, int n, int pi)
            {
                double toRet = 0;
                for (int i = k_a + 1; i <= m_a; i++)
                {
                    for (int j = k_b + 1; j <= m_b; j++)
                    {
                        toRet += proba_inter(i, j, m_a, m_b, n, pi);
                    }
                }
                return toRet;
            }

            public static double inclusif(int k_a, int k_b, int m_a, int m_b, int n, int pi)
            {
                double toRet = 0;
                for (int i = k_a; i <= m_a; i++)
                {
                    for (int j = k_b; j <= m_b; j++)
                    {
                        toRet += proba_inter(i, j, m_a, m_b, n, pi);
                    }
                }
                return toRet;
            }
        }
    }
}
