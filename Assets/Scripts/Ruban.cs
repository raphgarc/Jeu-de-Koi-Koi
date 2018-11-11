public enum RubanCategorie { Rouge, Poeme, Violet };

class Ruban : Carte
{
    public RubanCategorie Categorie { get; set; }

    public Ruban (Mois m, Rang r) : base(m, r)
    {
        switch (m) {
        case Mois.Janvier:
            Categorie = RubanCategorie.Poeme;
            break;
        case Mois.Fevrier:
            Categorie = RubanCategorie.Poeme;
            break;
        case Mois.Mars:
            Categorie = RubanCategorie.Poeme;
            break;
        case Mois.Avril:
            Categorie = RubanCategorie.Rouge;
            break;
        case Mois.Mai:
            Categorie = RubanCategorie.Rouge;
            break;
        case Mois.Juin:
            Categorie = RubanCategorie.Violet;
            break;
        case Mois.Juillet:
            Categorie = RubanCategorie.Rouge;
            break;
        case Mois.Septembre:
            Categorie = RubanCategorie.Violet;
            break;
        case Mois.Octobre:
            Categorie = RubanCategorie.Violet;
            break;
        case Mois.Novembre:
            Categorie = RubanCategorie.Rouge;
            break;
        default:
            //throw;
            break;
        }
    }

    public Ruban (int m, int r) : this((Mois) m, (Rang) r) { }
}
