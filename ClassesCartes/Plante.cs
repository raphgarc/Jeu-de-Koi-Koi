class Plante : Carte
{
    public Plante (Mois m, Rang r) : base(m, r) { }

    public Plante (int m, int r) : this((Mois) m, (Rang) r) { }
}
