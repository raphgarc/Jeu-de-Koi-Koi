using System.Collections.Generic;

public enum Mois { Janvier = 1, Fevrier, Mars, Avril, Mai, Juin, Juillet, Aout, Septembre, Octobre, Novembre, Decembre };

public enum Rang { Un = 1, Deux, Trois, Quatre };

abstract public class Carte
{
    public Mois Mois { get; set; }
    public Rang Rang { get; set; }
    public int Id { get; set; }

    public Carte (Mois m, Rang r)
    {
        Mois = m;
        Rang = r;
        Id = ((int) Mois * 10) + (int) Rang;
    }

    public static bool is_sake(Carte carte)
    {
        if ((carte is Animal) && ((Animal)carte).NomAnimal == NomAnimal.Sake)
            return true;
        return false;
    }

    public static bool is_in_plante_combinaison(Carte carte)
    {
        if ((carte is Plante) || is_sake(carte))
            return true;
        return false;
    }
    
    public static bool is_in_animal_combinaison(Carte carte)
    {
        if (carte is Animal)
            return true;
        return false;
    }
    
    public static bool is_in_lumiere_combinaison(Carte carte)
    {
        if (carte is Lumiere)
            return true;
        return false;
    }
    
    public static bool is_in_ruban_combinaison(Carte carte)
    {
        if (carte is Ruban)
            return true;
        return false;
    }

    public static bool is_in_ruban_poeme_combinaison(Carte carte)
    {
        if ((carte is Ruban) && (((Ruban)carte).Categorie == RubanCategorie.Poeme))
            return true;
        return false;
    }
    
    public static bool is_in_ruban_bleu_combinaison(Carte carte)
    {
        if ((carte is Ruban) && (((Ruban)carte).Categorie == RubanCategorie.Violet))
            return true;
        return false;
    }
    
    public static bool is_in_autre_sake_lune_combinaison(Carte carte)
    {
        if (is_sake(carte) || ((carte is Lumiere) && ((Lumiere)carte).NomLumiere == NomLumiere.Lune))
            return true;
        return false;
    }
    
    public static bool is_in_autre_sake_cerisier_combinaison(Carte carte)
    {
        if (is_sake(carte) || ((carte is Lumiere) && ((Lumiere)carte).NomLumiere == NomLumiere.Baniere))
            return true;
        return false;
    }
    
    public static bool is_in_autre_inoshikacho_combinaison(Carte carte)
    {
        if (carte is Animal)
        {
            if (((Animal)carte).NomAnimal == NomAnimal.Cerf)
                return true;
            else if (((Animal)carte).NomAnimal == NomAnimal.Sanglier)
                return true;
            else if (((Animal)carte).NomAnimal == NomAnimal.Papillon)
                return true;
        }
        return false;
    }

    public static int nb_plante_combinaison_in(List<Carte> cartes)
    {
        int i = 0;

        foreach (Carte carte in cartes)
        {
            if (is_in_plante_combinaison(carte))
                i++;
        }

        return i;
    }

    public static int nb_animal_combinaison_in(List<Carte> cartes)
    {
        int i = 0;

        foreach (Carte carte in cartes)
        {
            if (is_in_animal_combinaison(carte))
                i++;
        }

        return i;
    }

    public static int nb_lumiere_combinaison_in(List<Carte> cartes)
    {
        int i = 0;

        foreach (Carte carte in cartes)
        {
            if (is_in_lumiere_combinaison(carte))
                i++;
        }

        return i;
    }

    public static int nb_ruban_combinaison_in(List<Carte> cartes)
    {
        int i = 0;

        foreach (Carte carte in cartes)
        {
            if (is_in_ruban_combinaison(carte))
                i++;
        }

        return i;
    }

    public static int nb_ruban_poeme_combinaison_in(List<Carte> cartes)
    {
        int i = 0;

        foreach (Carte carte in cartes)
        {
            if (is_in_ruban_poeme_combinaison(carte))
                i++;
        }

        return i;
    }

    public static int nb_ruban_bleu_combinaison_in(List<Carte> cartes)
    {
        int i = 0;

        foreach (Carte carte in cartes)
        {
            if (is_in_ruban_bleu_combinaison(carte))
                i++;
        }

        return i;
    }

    public static int nb_autre_sake_lune_combinaison_in(List<Carte> cartes)
    {
        int i = 0;

        foreach (Carte carte in cartes)
        {
            if (is_in_autre_sake_lune_combinaison(carte))
                i++;
        }

        return i;
    }

    public static int nb_autre_sake_cerisier_combinaison_in(List<Carte> cartes)
    {
        int i = 0;

        foreach (Carte carte in cartes)
        {
            if (is_in_autre_sake_cerisier_combinaison(carte))
                i++;
        }

        return i;
    }

    public static int nb_autre_inoshikacho_in(List<Carte> cartes)
    {
        int i = 0;

        foreach (Carte carte in cartes)
        {
            if (is_in_autre_inoshikacho_combinaison(carte))
                i++;
        }

        return i;
    }


	public Carte can_match_with(List<Carte> with)
	{
		foreach (Carte carte in with)
		{
			if (carte.Mois == this.Mois)
				return carte;
		}
		return null;
	}

    
    public Carte (int m, int r) : this((Mois) m, (Rang) r) { }
}
