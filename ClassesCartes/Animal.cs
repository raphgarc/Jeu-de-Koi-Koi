enum NomAnimal { Rossignol, Coucou, Pont, Papillon, Sanglier, Oie, Sake, Cerf, Hirondelle };

class Animal : Carte
{
    public NomAnimal NomAnimal { get; }

    public Animal (Mois m, Rang r) : base(m, r)
    {
        switch (m) {
        case Mois.Fevrier:
            NomAnimal = NomAnimal.Rossignol;
            break;
        case Mois.Avril:
            NomAnimal = NomAnimal.Coucou;
            break;
        case Mois.Mai:
            NomAnimal = NomAnimal.Pont;
            break;
        case Mois.Juin:
            NomAnimal = NomAnimal.Papillon;
            break;
        case Mois.Juillet:
            NomAnimal = NomAnimal.Sanglier;
            break;
        case Mois.Aout:
            NomAnimal = NomAnimal.Oie;
            break;
        case Mois.Septembre:
            NomAnimal = NomAnimal.Sake;
            break;
        case Mois.Octobre:
            NomAnimal = NomAnimal.Cerf;
            break;
        case Mois.Novembre:
            NomAnimal = NomAnimal.Hirondelle;
            break;
        default:
            throw;
            break;
        }
    }

    public Animal (int m, int r) : this((Mois) m, (Rang) r) { }
}
