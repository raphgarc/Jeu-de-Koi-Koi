using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carte_IA
{
	public Carte    carte;
	public double   valeur;

	public Carte_IA(Carte carte, double val)
	{
		this.carte = carte;
		this.valeur = val;
	}

    public static int find_index_of(Carte to_find, List<Carte_IA> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].carte == to_find)
                return i;
        }

        return -1;
    }
}

public class Carte_IA_v3
{
    public Carte_IA main { get; private set; }
    public Carte_IA defosse { get; private set; }
    public double val { get; private set; }

	public Carte_IA_v3(Carte_IA jeu, Carte_IA defosse)
	{
		this.main = jeu;
		this.defosse = defosse;

        if (this.defosse != null)
            this.val = jeu.valeur + defosse.valeur;
        else
            this.val = -jeu.valeur;
	}
}

public class Carte_IA_v4
{
    public Carte_IA main { get; private set; }
    public Carte_IA defosse { get; private set; }

    private double val_pour_adv_carte_main;
    private double val_pour_adv_carte_defosse;

    public Carte_IA_v4(Carte_IA_v3 paire, double val_adverse_main, double val_adverse_defosse)
    {
        this.main = paire.main;
        this.defosse = paire.defosse;
        //this.val_pour_adv_carte_main = val_adverse_main;
        this.val_pour_adv_carte_defosse = val_adverse_defosse;
    }

    public double get_valeur_paire()
    {
        // On divise par 2 sinon, ça prend trop de place et on fait que défendre
        return (this.main.valeur + this.defosse.valeur + (val_pour_adv_carte_defosse / 2.0));
    }
}
