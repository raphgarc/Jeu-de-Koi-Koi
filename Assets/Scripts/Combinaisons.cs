using System;
using Transverse_IA.MyProba;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Combinaison { Plante = 0, Animal, Lumiere, Ruban, Ruban_poeme, Ruban_bleu, Autre_sake_lune, Autre_sake_cerisier, Autre_inoshikacho };
public delegate bool Carte_combinaison(Carte carte);

public class Combinaisons
{
	public List<Carte>  cartes_manquantes { get; private set; }                  // Ici, on a toutes les cartes qui nous permettrai de faire la combinaison (exemple : les 25 plantes)
	List<Carte>         cartes_adverse_dans_combinaison;
	public int          nb_cartes_manquantes_to_validate { get; private set; }               // Ici, on a le nombre de carte qu'il faut pour valider le prochain niveau de la combinaison
	int                 nb_cartes_existante;

	public double       proba_de_faire { get; private set; }
	public int          points_combinaison { get; private set; }                 //Le nombre de point en plus qui sera marqué à la prochaine étape de validation de la combinaaison
	public Combinaison  nom_combinaison{get; private set;}
	Carte_combinaison   is_in_my_combinaison;               //Pointeur sur fonction

	public Combinaisons(List<Carte> manquantes, Combinaison nom, List<Carte> jeu_adversaire, int nb_carte_main_adversaire, int nb_carte_pioche)
	{
		this.cartes_manquantes = manquantes;
		this.nom_combinaison = nom;

		if (manquantes.Count == 0)
		{
			this.points_combinaison = 0;
			this.nb_cartes_manquantes_to_validate = 0;
		}
		else
		{
			switch (nom)
			{
				case (Combinaison.Plante):
					this.points_combinaison = 1;
					this.nb_cartes_existante = 25;
					this.nb_cartes_manquantes_to_validate = calculate_nb_cartes_manquante(this.nb_cartes_existante, 10, manquantes.Count);
					is_in_my_combinaison = Carte.is_in_plante_combinaison;
					break;
				case (Combinaison.Animal):
					this.points_combinaison = 1;
					this.nb_cartes_existante = 9;
					this.nb_cartes_manquantes_to_validate = calculate_nb_cartes_manquante(this.nb_cartes_existante, 5, manquantes.Count);
					is_in_my_combinaison = Carte.is_in_animal_combinaison;
					break;
                    
				case (Combinaison.Lumiere):
					initialise_lumiere();
					is_in_my_combinaison = Carte.is_in_lumiere_combinaison;
					break;
				case (Combinaison.Ruban) :
					this.nb_cartes_existante = 10;
					this.nb_cartes_manquantes_to_validate = calculate_nb_cartes_manquante(this.nb_cartes_existante, 5, manquantes.Count);
					this.points_combinaison = 1;
					is_in_my_combinaison = Carte.is_in_ruban_combinaison;
					break;
				case(Combinaison.Ruban_poeme) :
					this.nb_cartes_existante = 3;
					this.nb_cartes_manquantes_to_validate = calculate_nb_cartes_manquante(this.nb_cartes_existante, 3, manquantes.Count);
					this.points_combinaison = 6;
					is_in_my_combinaison = Carte.is_in_ruban_poeme_combinaison;
					break;
				case (Combinaison.Ruban_bleu):
					this.nb_cartes_existante = 3;
					this.nb_cartes_manquantes_to_validate = calculate_nb_cartes_manquante(this.nb_cartes_existante, 3, manquantes.Count);
                    this.points_combinaison = 6;
					is_in_my_combinaison = Carte.is_in_ruban_bleu_combinaison;
                    break;
				case (Combinaison.Autre_sake_lune):
					this.nb_cartes_existante = 2;
					this.nb_cartes_manquantes_to_validate = calculate_nb_cartes_manquante(this.nb_cartes_existante, 2, manquantes.Count);
                    this.points_combinaison = 5;
					is_in_my_combinaison = Carte.is_in_autre_sake_lune_combinaison;
                    break;
				case (Combinaison.Autre_sake_cerisier):
					this.nb_cartes_existante = 2;
					this.nb_cartes_manquantes_to_validate = calculate_nb_cartes_manquante(this.nb_cartes_existante, 2, manquantes.Count);
                    this.points_combinaison = 5;
					is_in_my_combinaison = Carte.is_in_autre_sake_cerisier_combinaison;
                    break;
				case (Combinaison.Autre_inoshikacho):
					this.nb_cartes_existante = 3;
					this.nb_cartes_manquantes_to_validate = calculate_nb_cartes_manquante(this.nb_cartes_existante, 3, manquantes.Count);
                    this.points_combinaison = 5;
					is_in_my_combinaison = Carte.is_in_autre_inoshikacho_combinaison;
                    break;
			}

			cartes_adverse_dans_combinaison = new List<Carte>();
			foreach (Carte carte in jeu_adversaire)
			{
				if (is_in_my_combinaison(carte))
					cartes_adverse_dans_combinaison.Add(carte);
			}
            // On va considérer les lumières comme une combinaison classique même si c'est pas vrai

			// nb_cartes pour me faire chier = manquante - to_validate
            //                              -= nombre de carte qu'il a deja
            //                              += 1 sinon il me fait pas chier
			    int nb_carte_non_distribué = this.cartes_manquantes.Count - cartes_adverse_dans_combinaison.Count;
				int nb_carte_pour_me_faire_chier = nb_carte_non_distribué- this.nb_cartes_manquantes_to_validate + 1;            
				this.proba_de_faire = avoir_k_cartes.moins_de.strict(nb_carte_pour_me_faire_chier, nb_carte_non_distribué, nb_carte_main_adversaire, nb_carte_pioche);
		}
	}
    

    // Les lumières c'est chiant, c'est une exception...
    // Donc on va faire une fonction à part pour rentre le contructeur plus beau
	private void initialise_lumiere()
	{
		this.nb_cartes_existante = 5;
		if (fonctions_combinaisons.contain_raining_man(this.cartes_manquantes) == false)
        {
			this.nb_cartes_manquantes_to_validate = calculate_nb_cartes_manquante(this.nb_cartes_existante, 3, this.cartes_manquantes.Count);
			if (this.cartes_manquantes.Count > 1)
                this.points_combinaison = 8;
            else
                // On a deja 8 points, et si on a la dernière lumière, on passe à 15 points. On gagne donc 7 points
                this.points_combinaison = 7;
        }
        else
        {
			this.nb_cartes_manquantes_to_validate = calculate_nb_cartes_manquante(this.nb_cartes_existante, 4, this.cartes_manquantes.Count);
            // Les lumières posent un problème sur comment gérer le raining man dans le comptage des points. Actuellement, on prend la combinaison qui rapporte le moins de points
			if (this.cartes_manquantes.Count > 2)
                this.points_combinaison = 6;
			else if (this.cartes_manquantes.Count == 2)
                this.points_combinaison = 2; //Il a 3 lumières sans le raining man, il a donc 6 points. Il va donc pouvoir avoir 10 points si il a la 4ème lumière ou 8 points si il a le raining man. On suppose la combinaison qui rapporte le moins : Il peut donc gagner 2 points
            else
                this.points_combinaison = 5; // Il a 4 lumières sans le raining man, il a donc 10 points, si il prend le raining man, il va avoir 15 points. Donc on gagne 5 points
        }
	}


	private int calculate_nb_cartes_manquante(int nb_carte_existante, int nb_carte_necessaire, int nb_carte_manquante)
    {
        return Math.Max(1, (nb_carte_necessaire - nb_carte_existante + nb_carte_manquante));
    }
}
