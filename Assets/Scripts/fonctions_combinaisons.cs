using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public static class fonctions_combinaisons
{
	private static List<Carte> vitrine = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>().cartesVitrine;

	public static bool is_sake(Carte carte)
	{
		if ((carte is Animal) && ((Animal)carte).NomAnimal == NomAnimal.Sake)
			return true;
		return false;
	}

	public static bool contain_raining_man(List<Carte> cartes)
	{
        foreach (Carte carte in cartes)
		{
            if (carte is Lumiere)
            {
                if (((Lumiere)carte).NomLumiere == NomLumiere.Parapluie)
                {
                    return true;
                }
            }
		}
		return false;
	}

	public static List<List<Carte>> cartes_manquantes(Joueur joueur)
	{
		List<Carte> tmp_plis_adverses = new List<Carte>();

		tmp_plis_adverses.AddRange(joueur.PlisPlante);
		tmp_plis_adverses.AddRange(joueur.PlisAnimal);
		tmp_plis_adverses.AddRange(joueur.PlisLumiere);
		tmp_plis_adverses.AddRange(joueur.PlisRuban);

		vitrine = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>().cartesVitrine;

		List<List<Carte>> missing_cards = new List<List<Carte>>(9);
		missing_cards.Add(new List<Carte>());
		missing_cards.Add(new List<Carte>());
		missing_cards.Add(new List<Carte>());
		missing_cards.Add(new List<Carte>());
		missing_cards.Add(new List<Carte>());
		missing_cards.Add(new List<Carte>());
		missing_cards.Add(new List<Carte>());
		missing_cards.Add(new List<Carte>());
		missing_cards.Add(new List<Carte>());


        // A debug !! Je ne vérifie pas que je ne les aient pas déjà !!
		foreach (Carte carte in vitrine)
		{
			if (tmp_plis_adverses.Contains(carte) == false)
			{
				if (Carte.is_in_plante_combinaison(carte))
					missing_cards[(int)Combinaison.Plante].Add(carte);

				if (Carte.is_in_animal_combinaison(carte))
					missing_cards[(int)Combinaison.Animal].Add(carte);

				if (Carte.is_in_lumiere_combinaison(carte))
					missing_cards[(int)Combinaison.Lumiere].Add(carte);

				if (Carte.is_in_ruban_combinaison(carte))
					missing_cards[(int)Combinaison.Ruban].Add(carte);

				if (Carte.is_in_ruban_poeme_combinaison(carte))
					missing_cards[(int)Combinaison.Ruban_poeme].Add(carte);

				if (Carte.is_in_ruban_bleu_combinaison(carte))
					missing_cards[(int)Combinaison.Ruban_bleu].Add(carte);

				if (Carte.is_in_autre_sake_lune_combinaison(carte))
					missing_cards[(int)Combinaison.Autre_sake_lune].Add(carte);

				if (Carte.is_in_autre_sake_cerisier_combinaison(carte))
					missing_cards[(int)Combinaison.Autre_sake_cerisier].Add(carte);

				if (Carte.is_in_autre_inoshikacho_combinaison(carte))
					missing_cards[(int)Combinaison.Autre_inoshikacho].Add(carte);
			}
		}

		return missing_cards;
	}

	public static List<int> nb_points(Joueur joueur)
	{
		List<List<Carte>> missing_cards = cartes_manquantes(joueur);
		List<int> points = new List<int>(9);

		points.Add(0);
		points.Add(0);
		points.Add(0);
		points.Add(0);
		points.Add(0);
		points.Add(0);
		points.Add(0);
		points.Add(0);
		points.Add(0);

		// Il y a 25 plantes et il m'en faut 10 pour valider
		int nb_plante = 25 - missing_cards[(int)Combinaison.Plante].Count;
		if (nb_plante >= 10)
			points[(int)Combinaison.Plante] = nb_plante - 9;
		else
			points[(int)Combinaison.Plante] = 0;


		// Il y a 9 aminaux et il m'en faut 5 pour valider
		int nb_animaux = 9 - missing_cards[(int)Combinaison.Animal].Count;
		if (nb_animaux >= 5)
			points[(int)Combinaison.Animal] = nb_animaux - 4;
		else
			points[(int)Combinaison.Animal] = 0;


		//Il y a 5 lumières et il me faut 3 ou 4 lumière normale (+ bonus pour le raining man)
		int nb_lumieres = 4 - missing_cards[(int)Combinaison.Lumiere].Count;
		int raining_man = 1;
		// Si on a le raining man
		if (contain_raining_man(missing_cards[(int)Combinaison.Lumiere]) == true)
		{
			raining_man = 0;
			nb_lumieres++;
		}
		if (nb_lumieres == 3)
			points[(int)Combinaison.Lumiere] = 6 + 1 * raining_man;
		else if (nb_lumieres == 4)
			points[(int)Combinaison.Lumiere] = 8 + 2 * raining_man;
		else
			points[(int)Combinaison.Lumiere] = 0;


		// Il y a 10 rubans et il m'en faut 5 pour valider
		int nb_rubans = 10 - missing_cards[(int)Combinaison.Ruban].Count;
		if (nb_rubans >= 5)
			points[(int)Combinaison.Ruban] = nb_rubans - 4;
		else
			points[(int)Combinaison.Ruban] = 0;

		// Il y a 3 rubans bleu et il m'en faut 3 pour valider
		int nb_rubans_bleu = 3 - missing_cards[(int)Combinaison.Ruban_bleu].Count;
		if (nb_rubans_bleu == 3)
			points[(int)Combinaison.Ruban_bleu] = 6;
		else
			points[(int)Combinaison.Ruban_bleu] = 0;


		// Il y a 3 rubans poeme et il m'en faut 3 pour valider
		int nb_rubans_poeme = 3 - missing_cards[(int)Combinaison.Ruban_poeme].Count;
		if (nb_rubans_poeme == 3)
			points[(int)Combinaison.Ruban_poeme] = 6;
		else
			points[(int)Combinaison.Ruban_poeme] = 0;


		// Il y a 2 cartes dans le couple sake/lune et il me faut les 2 pour valider
		int nb_sake_lune = 2 - missing_cards[(int)Combinaison.Autre_sake_lune].Count;
		if (nb_sake_lune == 2)
			points[(int)Combinaison.Autre_sake_lune] = 5;
		else
			points[(int)Combinaison.Autre_sake_lune] = 0;


		// Il y a 2 cartes dans le couple sake/cerisier et il me faut les 2 pour valider
		int nb_sake_cerisier = 2 - missing_cards[(int)Combinaison.Autre_sake_cerisier].Count;
		if (nb_sake_cerisier == 2)
			points[(int)Combinaison.Autre_sake_cerisier] = 5;
		else
			points[(int)Combinaison.Autre_sake_cerisier] = 0;


		// Il y a 3 cartes dans la combinaison inoshikacho et il me faut les 3 pour valider
		int nb_inoshikacho = 3 - missing_cards[(int)Combinaison.Autre_inoshikacho].Count;
		if (nb_inoshikacho == 3)
			points[(int)Combinaison.Autre_inoshikacho] = 5;
		else
			points[(int)Combinaison.Autre_inoshikacho] = 0;

		// Debug
		int point_A = points[0];
		int point_B = points[1];
		int point_C = points[2];
		int point_D = points[3];
		int point_E = points[4];
		int point_F = points[5];
		int point_G = points[6];
		int point_H = points[7];
		int point_I = points[8];


		return points;
	}
}