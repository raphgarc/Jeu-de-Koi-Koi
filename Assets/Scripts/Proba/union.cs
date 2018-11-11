using System;
namespace Transverse_IA.MyProba
{
    //https://www.dcode.fr/probabilites-tirage
    // k : Le nombre d'élément à avoir
    // n : Le nombre de carte que le joueur possède dans sa main
    // m : Nombre d'éléments restant dans le jeu
    // pi : Nombre de carte dans la pioche


    public static class union
    {
        public static double exact(int k_a, int k_b, int m_a, int m_b, int n, int pi)
        {
            
            double p_a = MyProba.avoir_k_cartes.exactement(k_a, m_a, n, pi);
            double p_b = MyProba.avoir_k_cartes.exactement(k_b, m_b, n, pi);
            double p_inter = MyProba.inter.exact(k_a, k_b, m_a, m_b, n, pi);


            return (p_a + p_b - p_inter);
        }

        public static class moins_de
        {
            public static double strict(int k_a, int k_b, int m_a, int m_b, int n, int pi)
            {
                double p_a_totale;
                double p_b_totale;
                double p_inter;

                p_a_totale = MyProba.avoir_k_cartes.moins_de.strict(k_a, m_a, n, pi);
                p_b_totale = MyProba.avoir_k_cartes.moins_de.strict(k_b, m_b, n, pi);
                p_inter = inter.moins_de.strict(k_a, k_b, m_a, m_b, n, pi);

                return p_a_totale + p_b_totale - p_inter;
            }

            // La probabilité qu'au moins l'un des a se réalise (somme) + la probabilité qu'au moins l'un des b se réalise (somme) - la probabilité de toutes les interections de a et b
            public static double inclusif(int k_a, int k_b, int m_a, int m_b, int n, int pi)
            {
                double p_a_totale;
                double p_b_totale;
                double p_inter;


                p_a_totale = MyProba.avoir_k_cartes.moins_de.inclusif(k_a, m_a, n, pi);
                p_b_totale = MyProba.avoir_k_cartes.moins_de.inclusif(k_b, m_b, n, pi);
                p_inter = inter.moins_de.inclusif(k_a, k_b, m_a, m_b, n, pi);

                return p_a_totale + p_b_totale - p_inter;
            }
        }

        public static class plus_de
        {
            public static double strict(int k_a, int k_b, int m_a, int m_b, int n, int pi)
            {
                double p_a_totale;
                double p_b_totale;
                double p_inter;

                p_a_totale = MyProba.avoir_k_cartes.plus_de.strict(k_a, m_a, n, pi);
                p_b_totale = MyProba.avoir_k_cartes.plus_de.strict(k_b, m_b, n, pi);
                p_inter = inter.plus_de.strict(k_a, k_b, m_a, m_b, n, pi);

                return p_a_totale + p_b_totale - p_inter;
            }

            public static double inclusif(int k_a, int k_b, int m_a, int m_b, int n, int pi)
            {
                double p_a_totale;
                double p_b_totale;
                double p_inter;

                p_a_totale = MyProba.avoir_k_cartes.plus_de.inclusif(k_a, m_a, n, pi);
                p_b_totale = MyProba.avoir_k_cartes.plus_de.inclusif(k_b, m_b, n, pi);
                p_inter = inter.plus_de.inclusif(k_a, k_b, m_a, m_b, n, pi);

                return p_a_totale + p_b_totale - p_inter;
            }
        }
    }
}
