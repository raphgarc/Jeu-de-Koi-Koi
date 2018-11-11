enum Mois { Janvier, Fevrier, Mars, Avril, Mai, Juin, Juillet, Aout, Septembre, Octobre, Novembre, Decembre };

enum Rang { Un, Deux, Trois, Quatre };

abstract class Carte
{
    public Mois Mois { get; }
    public Rang Rang { get; }
    public int Id { get; }

    public Carte (Mois m, Rang r)
    {
        Mois = m;
        Rang = r;
        Id = ((int) Mois * 10) + (int) Rang;
    }

    public Carte (int m, int r) : this((Mois) m, (Rang) r) { }
}
