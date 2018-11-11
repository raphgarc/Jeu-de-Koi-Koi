using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum IA_v { version_0, version_1, version_2, version_3, version_4 };

public class IA : MonoBehaviour
{
    public static Carte carte_defosse { get; private set; }
    private static IA_v last_IA_use;

    private List<Carte> jeu_IA;
    private List<Carte> jeu_Humain;
    private List<Carte> jeu_current_player;
    private List<Carte> defosse;
    private int nb_cartes_joueur_contre;
    private int nb_carte_pioche;

    private Joueur Joueur_humain;
    private Joueur Joueur_IA;

    private GameObject manager;


    public Carte DemarrerIA()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        jeu_IA = manager.GetComponent<GameManager>().jeuIA;
        jeu_Humain = manager.GetComponent<GameManager>().jeuHumain;
        defosse = manager.GetComponent<GameManager>().defosse;
        nb_carte_pioche = manager.GetComponent<GameManager>().pioche.Count;
        //defosse = manager.GetComponent
        Joueur_humain = GameManager.J1_humain;
        Joueur_IA = GameManager.J2_IA;

        // Si c'est l'IA qui joue
        if (GameManager.J1_humain_plays == false)
        {
            nb_cartes_joueur_contre = manager.GetComponent<GameManager>().jeuHumain.Count;
            jeu_current_player = jeu_IA;
        }
        // Si c'est l'humain qui joue
        else
        {
            nb_cartes_joueur_contre = manager.GetComponent<GameManager>().jeuIA.Count;
            jeu_current_player = jeu_Humain;
        }

        Carte tmp;

        //Debug.Log(jeuEnnemi);
        //Debug.Log(defosse);
        //Debug.Log(J1.PlisLumiere.Count);
        //Debug.Log(J2.PlisPlante.Count);

        // J1, c'est l'humain
        // J2, c'est l'IA
        if (MenuManager.typeJeu == 1)
        {  //jeu normal, il n'y a qu'une seule IA
            switch (MenuManager.niveauIAnormal)
            {
                case 1:
                    last_IA_use = IA_v.version_1;
                    tmp = IA_new(IA_v.version_1);
                    break;

                case 2:
                    last_IA_use = IA_v.version_2;
                    tmp = IA_new(IA_v.version_2);
                    break;

                case 3:
                    last_IA_use = IA_v.version_3;
                    tmp = IA_new(IA_v.version_3);
                    break;
                case 4:
                    last_IA_use = IA_v.version_4;
                    tmp = IA_new(IA_v.version_4);
                    break;
                default:
                    last_IA_use = IA_v.version_3;
                    tmp = IA_new(IA_v.version_3);
                    break;

            }
            if (jeu_IA.Contains(tmp) == false)
                Debug.LogError("L'IA m'a donné une carte qui n'était pas à l'IA");

            
            return tmp;
        }

        else
        {
            if (GameManager.J1_humain_plays == true)
            {
                switch (MenuManager.niveauIA1)
                {
                    case 1:
                        tmp = IA_new(IA_v.version_1);
                        break;

                    case 2:
                        tmp = IA_new(IA_v.version_2);
                        break;

                    case 3:
                        tmp = IA_new(IA_v.version_3);
                        break;
                    case 4:
                        tmp = IA_new(IA_v.version_4);
                        break;
                    default:
                        tmp = IA_new(IA_v.version_3);
                        break;

                }
                if (jeu_Humain.Contains(tmp) == false)
                    Debug.LogError("L'IA m'a donné une carte qui n'appartient pas à J1_humain");
                return tmp;
            }
            else
            {
                switch (MenuManager.niveauIA2)
                {
                    case 1:
                        tmp = IA_new(IA_v.version_1);
                        break;

                    case 2:
                        tmp = IA_new(IA_v.version_2);
                        break;

                    case 3:
                        tmp = IA_new(IA_v.version_3);
                        break;

                    case 4:
                        tmp = IA_new(IA_v.version_4);
                        break;
                    default:
                        tmp = IA_new(IA_v.version_3);
                        break;

                }
                if (jeu_IA.Contains(tmp) == false)
                    Debug.LogError("L'IA m'a donné une carte qui n'appartient pas à J2_IA");
                return tmp;
            }
        }

    }


    public Carte IA_new(IA_v version)
    {
        List<Carte> jeu_player;
        Joueur moi;
        Joueur ennemie;

        IA.carte_defosse = null;

        if (GameManager.J1_humain_plays == true)
        {
            jeu_player = jeu_Humain;
            moi = Joueur_humain;
            ennemie = Joueur_IA;

        }
        else
        {
            jeu_player = jeu_IA;
            moi = Joueur_IA;
            ennemie = Joueur_humain;

        }
        

        if (GameManager.J1_humain_plays == true)
            Debug.LogWarning("IA pour humain");
        else
            Debug.LogWarning("IA pour IA");
        List<Combinaisons> combinaisons;
        List<Carte_IA> proba_par_carte;
        List<Carte_IA_v3> proba_par_paire;
        List<Carte_IA_v4> proba_par_paire_avec_defense;

        if (version == IA_v.version_0)
            return IA_V0_aleatoire();

        combinaisons = IA_V1_load_probabilites(moi, ennemie);
        if (version == IA_v.version_1)
            return IA_V1_choose_best_carte(combinaisons);

        proba_par_carte = IA_V2_load_proba_par_carte(combinaisons);
        if (version == IA_v.version_2)
            return IA_V2_choose_best_card(proba_par_carte);

        proba_par_paire = IA_V3_load_paires(proba_par_carte);
        if (version == IA_v.version_3)
        {
            if (proba_par_paire.Count > 0)
                return IA_V3_choose_best_card_to_play(proba_par_paire);
            // Si jamais il n'y a aucune combinaison qui marche, on prend juste la carte qui a le plus faible poids, c'est à dire on applique l'IA V2
            else
                return IA_V2_choose_best_card(proba_par_carte);
        }
        proba_par_paire_avec_defense = IA_V4_load_paires(proba_par_paire);
        if (version == IA_v.version_4)
        {
            if (proba_par_paire_avec_defense.Count > 0)
                return IA_V4_choose_best_card_to_play(proba_par_paire_avec_defense);
            else
                return IA_V4_defosse_card(proba_par_carte);
        }

        // Juste pour faire taire le compilo mais normalement, on ne devrait jamais sortir de la fonction ici
        return jeu_player[0];

    }








    // Cette IA là va chercher une carte aléatoirement dans le jeu de l'IA
    public Carte IA_V0_aleatoire()
    {
        List<Carte> jeu;
        if (GameManager.J1_humain_plays == true)
            jeu = jeu_Humain;
        else
            jeu = jeu_IA;

        int cpt = 0;
        int rdn = Random.Range(0, jeu.Count);
        if (defosse.Count >= 12)
        {
            foreach (Carte def in defosse)
            {
                if (jeu[rdn].Mois != def.Mois)
                    cpt++;

            }
            if (cpt == defosse.Count)
            {
                //Debug.Log("carteIAChangée");
                IA_V0_aleatoire();
            }
            else
                return jeu[rdn];
        }

        else
            return jeu[rdn];

        return null; //inutile

    }








    // Cette partie de l'IA V1 va chercher toutes les cartes que l'IA n'a pas validé.
    // Elle va ensuite pour chaque combinaison calculer la probabilité de ne pas être bloqué par l'adversaire
        // Chacune de ces combinaisons contiendra les cartes manquante ainsi que la probabilité calculé
    private List<Combinaisons> IA_V1_load_probabilites(Joueur player, Joueur ennemei)
    {
        List<List<Carte>> missing_cards;
        List<Combinaisons> combinaisons = new List<Combinaisons>();
        List<Carte> tmp_plis_adverse = new List<Carte>();

        missing_cards = fonctions_combinaisons.cartes_manquantes(player);
        tmp_plis_adverse.AddRange(ennemei.PlisPlante);
        tmp_plis_adverse.AddRange(ennemei.PlisAnimal);
        tmp_plis_adverse.AddRange(ennemei.PlisLumiere);
        tmp_plis_adverse.AddRange(ennemei.PlisRuban);

        // On initialise chaque combinaison
        for (int i = 0; i < missing_cards.Count; i++)
        {
            combinaisons.Add(new Combinaisons(missing_cards[i], (Combinaison)i, tmp_plis_adverse, nb_cartes_joueur_contre, nb_carte_pioche));
        }

        return combinaisons;
    }


    // Cette partie de l'IA v1 va ordonner chaque combinaison de celle qui a le meilleur ratio proba*points
    // Elle va ensuite regarder dans chacune de ces combinaisons si elle peut jouer la carte ou non
    private Carte IA_V1_choose_best_carte(List<Combinaisons> combinaisons)
    {
        Carte tmp;


        // On les tris de la plus rentable à la moins rentable
        combinaisons.Sort(delegate (Combinaisons a, Combinaisons b)
        {
            double score_a = a.proba_de_faire * a.points_combinaison * (1.0 / a.nb_cartes_manquantes_to_validate);
            double score_b = b.proba_de_faire * b.points_combinaison * (1.0 / b.nb_cartes_manquantes_to_validate);
            // Ok pour le warning car certaines combinaisons peuvent être equiprobable
            if (score_a == score_b)
            {
                if (a.nb_cartes_manquantes_to_validate < b.nb_cartes_manquantes_to_validate)
                    return (-1);
                else
                    return (1);
            }
            if (score_a > score_b)
                return (-1);
            else
                return (1);
        });


        // On peut jouer une carte dans notre main
        foreach (Combinaisons combi in combinaisons)
        {
            foreach (Carte carte in combi.cartes_manquantes)
            {
                tmp = which_card_to_play_this(carte);
                if (tmp != null)
                    return tmp;
            }
        }

        // On doit se défausser d'une carte dans sa main
        for (int i = combinaisons.Count - 1; i >= 0; i--)
        {
            //Cas où on ne peut pas linker ses cartes avec celle de la defosse
            foreach (Carte carte in combinaisons[i].cartes_manquantes)
            {
                if (GameManager.J1_humain_plays == true)
                {
                    if (jeu_Humain.Contains(carte))
                        return (carte);
                }
                else
                {
                    if (jeu_IA.Contains(carte))
                        return (carte);
                }
            }
        }

        // Si on arrive là, c'est que c'est la merde !
        Debug.LogError("Je suis désolé pour toi, tu as assité à un bug, je vais devoir te butter car personne ne doit jamais savoir que l'IA_1 est incapable de choisir une carte !");
        return (jeu_IA[0]);
    }









    // Cette partie de l'IA v2 va reprendre les poids par combinaison calculé précédement et va convertir ça en un poids par carte
    // Si une carte était dans 2 combinaison et avait un poids de 0,25 et 1,33. La carte à un poids total de 1,58
    private List<Carte_IA> IA_V2_load_proba_par_carte(List<Combinaisons> combinaisons)
    {
        List<Carte> cartes = new List<Carte>();
        List<double> valeur_cartes = new List<double>();
        int index_carte;
        List<Carte_IA> to_ret = new List<Carte_IA>(); ;

        foreach (Combinaisons combi in combinaisons)
        {
            foreach (Carte card in combi.cartes_manquantes)
            {
                index_carte = cartes.IndexOf(card);
                // Si on n'a jamais ajouté la carte
                if (index_carte == -1)
                {
                    cartes.Add(card);
                    valeur_cartes.Add(combi.proba_de_faire * combi.points_combinaison * (1.0 / combi.nb_cartes_manquantes_to_validate));
                }
                // Si la carte a déja été ajouté, on augmente sa valeur
                else
                    valeur_cartes[index_carte] += (combi.proba_de_faire * combi.points_combinaison * (1.0 / combi.nb_cartes_manquantes_to_validate));
            }
        }
        if (cartes.Count != valeur_cartes.Count)
            Debug.LogError("C'est la merde, on va tous mourrir, j'ai codé de la merde et l'IA bug, et mon IA V2 n'a pas le même nombre de cartes et de proba !");

        // On converti nos 2 variables en une seule
        // Si toi aussi tu te dis que j'ai été con de faire 2 variables, ne perd pas 1/2h à refaire la fonction
        // Ça va foutre la merde avec le IndexOf
        for (int i = 0; i < cartes.Count; i++)
        {
            to_ret.Add(new Carte_IA(cartes[i], valeur_cartes[i]));
        }
        return to_ret;
    }


    // On va trier les cartes par ordre de poids croissant et on va choisir la carte qui rapporte qui à le plus grand poids
    private Carte IA_V2_choose_best_card(List<Carte_IA> cartes)
    {
        Carte tmp;

        // On les tris de la plus rentable à la moins rentable
        cartes.Sort(delegate (Carte_IA a, Carte_IA b)
        {
            double score_a = a.valeur;
            double score_b = b.valeur;

            if (score_a > score_b)
                return (-1);
            else
                return (1);
        });


        // On peut jouer une carte dans notre main
        foreach (Carte_IA carte in cartes)
        {
            tmp = which_card_to_play_this(carte.carte);
            if (tmp != null)
                return tmp;
        }

        // On doit se défausser d'une carte dans sa main
        for (int i = cartes.Count - 1; i >= 0; i--)
        {
            if (GameManager.J1_humain_plays == true)
            {
                if (jeu_Humain.Contains(cartes[i].carte))
                    return (cartes[i].carte);
            }
            else
            {
                if (jeu_IA.Contains(cartes[i].carte))
                    return (cartes[i].carte);
            }
        }

        // Si on arrive là, c'est que c'est la merde !
        Debug.LogError("Si je retrouve le petit con qui a codé cette fonction, il entendra parler de moi ! Comment ça se fait que mon IA V2 soit incapable de choisir une carte ??");
        return (jeu_IA[0]);      
    }






    // On ne va maintenant plus résonner sur une carte mais sur 2 cartes
    // Pour chacune des cartes, on va regarder avec quelle(s) carte(s) elle peut matcher et on va stocker tout ça...
    private List<Carte_IA_v3> IA_V3_load_paires(List<Carte_IA> proba_par_carte)
    {
        List<Carte_IA_v3> paires = new List<Carte_IA_v3>();
        List<Carte> jeu_player;
        Carte tmp;

        if (GameManager.J1_humain_plays == true)
            jeu_player = jeu_Humain;
        else
            jeu_player = jeu_IA;

        foreach (Carte carte in jeu_player)
        {
            List<Carte> tmp_list_defosse = new List<Carte>(defosse);
            int i = 0;

            // Si ça peut matcher avec plusieurs cartes de la défosse il faut relancer la detection pour toutes les trouver
            do
            {
                tmp = carte.can_match_with(tmp_list_defosse);
                if (tmp != null)
                {
                    Carte_IA tmp_joueur;
                    Carte_IA tmp_defosse;

                    tmp_joueur = proba_par_carte[Carte_IA.find_index_of(carte, proba_par_carte)];
                    tmp_defosse = proba_par_carte[Carte_IA.find_index_of(tmp, proba_par_carte)];
                    paires.Add(new Carte_IA_v3(tmp_joueur, tmp_defosse));
                }
                tmp_list_defosse.Remove(tmp);
                i++;
                if (i == 4) // Quand y'a un pli 3 choix, on passe une 4ème fois dans la boucle pour recevoir le null
                    Debug.Log("L'IA peut faire un pli 3 choix. Pour l'instant, j'ai la flemme de programmer cette exception donc tant pis, l'IA va faire de la merde");

            } while (tmp != null);
        }

        return paires;
    }


    private Carte IA_V3_choose_best_card_to_play(List<Carte_IA_v3> paires)
    {
        paires.Sort(delegate (Carte_IA_v3 a, Carte_IA_v3 b)
        {
            double score_a = a.val;
            double score_b = b.val;

            if (score_a > score_b)
                return (-1);
            else
                return (1);
        });
        IA.carte_defosse = paires[0].defosse.carte;
        return paires[0].main.carte;
    }




    // Dans cette partie de l'IA v4, on ne va plus réfléchir que pour nous mais aussi pour l'adversaire.
    // Ainsi, on va essayer de défendre
    private List<Carte_IA_v4> IA_V4_load_paires(List<Carte_IA_v3> proba_par_paire)
    {
        List<Carte_IA_v4> toRet = new List<Carte_IA_v4>();
        List<Carte_IA> proba_par_carte_ennemie;
        if (GameManager.J1_humain_plays == true)
            proba_par_carte_ennemie = IA_V2_load_proba_par_carte(IA_V1_load_probabilites(GameManager.J2_IA, GameManager.J1_humain));
        else
            proba_par_carte_ennemie = IA_V2_load_proba_par_carte(IA_V1_load_probabilites(GameManager.J1_humain, GameManager.J2_IA));

        foreach (Carte_IA_v3 paire in proba_par_paire)
        {
            double val_main_ennemie;
            double val_defosse_ennemie;
            Carte_IA_v4 tmp;
            int tmp1;
            int tmp2;

            // Ici, on trouvera forcément une occurence de la carte car si je l'ai ou que la défosse l'a, l'adversaire ne l'a pas.
            tmp1 = Carte_IA.find_index_of(paire.main.carte, proba_par_carte_ennemie);
            tmp2 = Carte_IA.find_index_of(paire.defosse.carte, proba_par_carte_ennemie);
            val_main_ennemie = proba_par_carte_ennemie[tmp1].valeur;
            val_defosse_ennemie = proba_par_carte_ennemie[tmp2].valeur;

            tmp = new Carte_IA_v4(paire, val_main_ennemie, val_defosse_ennemie);
            toRet.Add(tmp);
        }
        return toRet;
    }

    private Carte IA_V4_choose_best_card_to_play(List<Carte_IA_v4> paires)
    {
        paires.Sort(delegate (Carte_IA_v4 a, Carte_IA_v4 b)
        {
            double score_a = a.get_valeur_paire();
            double score_b = b.get_valeur_paire();

            if (score_a > score_b)
                return (-1);
            else
                return (1);
        });
        IA.carte_defosse = paires[0].defosse.carte;
        return paires[0].main.carte;
    }

    private Carte IA_V4_defosse_card(List<Carte_IA> proba_par_carte)
    {
        List<Carte_IA> main = new List<Carte_IA>();

        List<Carte_IA> proba_par_carte_ennemie;
        if (GameManager.J1_humain_plays == true)
            proba_par_carte_ennemie = IA_V2_load_proba_par_carte(IA_V1_load_probabilites(GameManager.J2_IA, GameManager.J1_humain));
        else
            proba_par_carte_ennemie = IA_V2_load_proba_par_carte(IA_V1_load_probabilites(GameManager.J1_humain, GameManager.J2_IA));
        

        foreach (Carte carte in this.jeu_current_player)
        {
            double val;
            int index_carte_mes_probas;
            int index_carte_probas_ennemie;

            index_carte_mes_probas = Carte_IA.find_index_of(carte, proba_par_carte);
            index_carte_probas_ennemie = Carte_IA.find_index_of(carte, proba_par_carte_ennemie);

            val = 0.0 - proba_par_carte[index_carte_mes_probas].valeur - (proba_par_carte_ennemie[index_carte_probas_ennemie].valeur / 2.0);
            main.Add(new Carte_IA(carte, val));
        }

        main.Sort(delegate (Carte_IA a, Carte_IA b)
        {
            double score_a = a.valeur;
            double score_b = b.valeur;

            if (score_a > score_b)
                return (-1);
            else
                return (1);
        });

        return main[0].carte;
    }









    private Carte which_card_to_play_this(Carte carte)
    {
        List<Carte> jeu_player;

        if (GameManager.J1_humain_plays == true)
            jeu_player = jeu_Humain;
        else
            jeu_player = jeu_IA;


        Carte tmp;
        if (jeu_player.Contains(carte))
        {
            tmp = carte.can_match_with(defosse);
            if (tmp != null)
            {
                IA.carte_defosse = tmp;
                return carte;
            }
        }
        else if (defosse.Contains(carte))
        {
             tmp = carte.can_match_with(jeu_player);
            if (tmp != null)
            {
                IA.carte_defosse = carte;
                return (tmp);
            }
        }
        return null;
    }





    public static bool continuer()
    {
        Joueur              autre_joueur;
        Joueur              joueur_moi;
        List<List<Carte>>   carte_manquante_autre_joueur;
        //string              debug;
        List<Carte>         defosse;
        List<int>           nb_cartes_to_validate_autre_joueur = new List<int>();

        defosse = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>().defosse;

        if (GameManager.J1_humain_plays == true)
        {
            //debug = "humain";
            Debug.Log("L'humain veut savoir si il peut continuer");
            autre_joueur = GameManager.J2_IA;
            joueur_moi = GameManager.J1_humain;
        }
        else
        {
            //debug = "IA";
            Debug.Log("L'IA veut savoir si il peut continuer");
            autre_joueur = GameManager.J1_humain;
            joueur_moi = GameManager.J2_IA;
        }

        carte_manquante_autre_joueur = fonctions_combinaisons.cartes_manquantes(autre_joueur);

        nb_cartes_to_validate_autre_joueur.Add(10 - 25 + carte_manquante_autre_joueur[(int)Combinaison.Plante].Count);
        nb_cartes_to_validate_autre_joueur.Add(5 - 9 + carte_manquante_autre_joueur[(int)Combinaison.Animal].Count);
        nb_cartes_to_validate_autre_joueur.Add(3 - 5 + carte_manquante_autre_joueur[(int)Combinaison.Lumiere].Count);
        nb_cartes_to_validate_autre_joueur.Add(5 - 10 + carte_manquante_autre_joueur[(int)Combinaison.Ruban].Count);
        nb_cartes_to_validate_autre_joueur.Add(3 - 3 + carte_manquante_autre_joueur[(int)Combinaison.Ruban_poeme].Count);
        nb_cartes_to_validate_autre_joueur.Add(3 - 3 + carte_manquante_autre_joueur[(int)Combinaison.Ruban_bleu].Count);
        nb_cartes_to_validate_autre_joueur.Add(2 - 2 + carte_manquante_autre_joueur[(int)Combinaison.Autre_sake_lune].Count);
        nb_cartes_to_validate_autre_joueur.Add(2 - 2 + carte_manquante_autre_joueur[(int)Combinaison.Autre_sake_cerisier].Count);
        nb_cartes_to_validate_autre_joueur.Add(2 - 2 + carte_manquante_autre_joueur[(int)Combinaison.Autre_inoshikacho].Count);

        // Si l'autre joueur a une lumière, c'est potentiellement la merde. Mais si il a uniquement le raining man, on s'en fiche
        if (fonctions_combinaisons.contain_raining_man(autre_joueur.PlisLumiere))
            nb_cartes_to_validate_autre_joueur[(int)Combinaison.Lumiere]++;


        // Plantes
        if ((nb_cartes_to_validate_autre_joueur[(int)Combinaison.Plante] == 2) && (Carte.nb_plante_combinaison_in(defosse) > 0))
                return false;
        if (nb_cartes_to_validate_autre_joueur[(int)Combinaison.Plante] < 2)
            return false;

        // Animaux
        if (nb_cartes_to_validate_autre_joueur[(int)Combinaison.Animal] < 2)
        {
            // Si l'autre à 4 animaux mais que j'en ai déjà 5, il ne pourra jamais finir cette combinaison car il n'y a plus de carte disponible
            if ((joueur_moi.PlisAnimal.Count + Carte.nb_animal_combinaison_in(joueur_moi.jeu)) < 5)
                return false;
        }

        // Rubans
        if (nb_cartes_to_validate_autre_joueur[(int)Combinaison.Ruban] < 2)
        {
            if ((joueur_moi.PlisRuban.Count + Carte.nb_ruban_combinaison_in(joueur_moi.jeu)) < 5)
                return false;
        }

        if (last_IA_use != IA_v.version_4)
            return true;

        // Lumières
        if (nb_cartes_to_validate_autre_joueur[(int)Combinaison.Lumiere] < 2)
        {
            if (fonctions_combinaisons.contain_raining_man(joueur_moi.PlisLumiere) || (fonctions_combinaisons.contain_raining_man(joueur_moi.jeu)))
            {
                if ((joueur_moi.PlisLumiere.Count + Carte.nb_lumiere_combinaison_in(joueur_moi.jeu)) < 3)
                    return false;
            }
            else
            {
                if ((joueur_moi.PlisLumiere.Count + Carte.nb_lumiere_combinaison_in(joueur_moi.jeu)) < 2)
                    return false;
            }
        }

       

        // Rubans poeme
        if (nb_cartes_to_validate_autre_joueur[(int)Combinaison.Ruban_poeme] < 2)
        {
            if ((Carte.nb_ruban_poeme_combinaison_in(joueur_moi.PlisRuban) + Carte.nb_ruban_poeme_combinaison_in(joueur_moi.jeu)) < 1)
                return false;
        }

        // Rubans bleu
        if (nb_cartes_to_validate_autre_joueur[(int)Combinaison.Ruban_bleu] < 2)
        {
            if ((Carte.nb_ruban_bleu_combinaison_in(joueur_moi.PlisRuban) + Carte.nb_ruban_bleu_combinaison_in(joueur_moi.jeu)) < 1)
                return false;
        }

        // Autre Sake Lune
        if (nb_cartes_to_validate_autre_joueur[(int)Combinaison.Autre_sake_lune] < 2)
        {
            if ((Carte.nb_autre_sake_lune_combinaison_in(joueur_moi.PlisAnimal) + Carte.nb_autre_sake_lune_combinaison_in(joueur_moi.PlisLumiere) + Carte.nb_autre_sake_lune_combinaison_in(joueur_moi.jeu)) < 1)
                return false;
        }

        // Autre sake cerisier
        if (nb_cartes_to_validate_autre_joueur[(int)Combinaison.Autre_sake_cerisier] < 2)
        {
            if ((Carte.nb_autre_sake_cerisier_combinaison_in(joueur_moi.PlisAnimal) + Carte.nb_autre_sake_cerisier_combinaison_in(joueur_moi.PlisLumiere) + Carte.nb_autre_sake_cerisier_combinaison_in(joueur_moi.jeu)) < 1)
                return false;
        }

        // Autre inoshikacho
        if (nb_cartes_to_validate_autre_joueur[(int)Combinaison.Autre_inoshikacho] < 2)
        {
            if ((Carte.nb_autre_inoshikacho_in(joueur_moi.PlisAnimal) + Carte.nb_autre_inoshikacho_in(joueur_moi.jeu)) < 1)
                return false;
        }

        return true;
    }
}