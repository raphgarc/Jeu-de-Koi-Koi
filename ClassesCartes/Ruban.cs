enum RubanCategorie { Rouge, Poeme, Violet };

class Ruban : Carte
{
    public RubanCategorie Categorie { get; }

    public Ruban (Mois m, Rang r) : base(m, r)
    {
        switch (m) {
        case Mois.Janvier:
            NomAnimal = Categorie.Poeme;
            break;
        case Mois.Fevrier:
            NomAnimal = Categorie.Poeme;
            break;
        case Mois.Mars:
            NomAnimal = Categorie.Poeme;
            break;
        case Mois.Avril:
            NomAnimal = Categorie.Rouge;
            break;
        case Mois.Mai:
            NomAnimal = Categorie.Rouge;
            break;
        case Mois.Juin:
            NomAnimal = Categorie.Violet;
            break;
        case Mois.Juillet:
            NomAnimal = Categorie.Rouge;
            break;
        case Mois.Septembre:
            NomAnimal = Categorie.Violet;
            break;
        case Mois.Octobre:
            NomAnimal = Categorie.Violet;
            break;
        case Mois.Novembre:
            NomAnimal = Categorie.Rouge;
            break;
        default:
            throw;
            break;
        }
    }

    public Ruban (int m, int r) : this((Mois) m, (Rang) r) { }
}
