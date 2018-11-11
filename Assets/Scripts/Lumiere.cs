public enum NomLumiere { Grue, Baniere, Lune, Parapluie, Phenix };

class Lumiere : Carte
{
    public NomLumiere NomLumiere { get; set; }

    public Lumiere (Mois m, Rang r) : base(m, r)
    {
        switch (m) {
        case Mois.Janvier:
            NomLumiere = NomLumiere.Grue;
            break;
        case Mois.Mars:
            NomLumiere = NomLumiere.Baniere;
            break;
        case Mois.Aout:
            NomLumiere = NomLumiere.Lune;
            break;
        case Mois.Novembre:
            NomLumiere = NomLumiere.Parapluie;
            break;
        case Mois.Octobre:
            NomLumiere = NomLumiere.Phenix;
            break;
        default:
            //throw;
            break;
        }
    }

    public Lumiere (int m, int r) : this((Mois) m, (Rang) r) { }
}
