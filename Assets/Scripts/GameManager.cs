using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;


public class GameManager : MonoBehaviour {      //Objet théorique qui regroupe tous les objets théoriques Cartes, ainsi que tous les objets physiques GameCard. Toutes les initalisations et comparaison de Cartes se font dans cette classe.

    public Sprite[] cardFace;                   //Liste de Sprites de face (initialisés depuis Unity, on ne les modifie pas dans le code)
    public Sprite cardBack;
    public List <GameCard> cards;               //Liste de toutes les GameCards (objets physiques). Déja initialisés depuis Unity, il n'est pas nécessaire de créér des nouveaux.
    public GameCard[] defosseGameCard;          //Tableau des GameCard de la défosse et de leurs coordonées. Servent simplement à deplacer les cartes en cas de non paire.
    public List<GameCard> gameCard3choix;


    public List<Carte> jeuHumain;                 //Liste théorique des cartes de mon jeu. N'a pas d'influence directe sur les GameCard.
    public List<Carte> jeuIA;              
    public List<Carte> defosse;
    public List<Carte> pioche;
    private List<Carte> cardsShuffled;          //Liste des cartes mélangées. Sert à attribuer les bons sprites au GameCard.
    public List<Carte> cartesVitrine { get; private set; }           //Liste des Cartes de base. Non mélangée, elle est initialisée au début et ne bougera jamais.

    public List<GameCard> cartesSupp;
    public Vector3[] coordSupp;
    public Vector3[] coordInitialesCartes;
    public Vector3[] coordInitialesDefosse;     //stocke les coordonnées initiales des emplacements possibles de la défosse. Ne bouge pas.
    public Vector3[] coordDefosse;              //stocke les coordonnées des cartes actuellement en défosse.

    public static GameCard gameCard2choix;

    public static bool pauseJeu2Choix;
    public static bool choixPaireClick;
    public static int cpt2 = 0;

    public static Joueur J1_humain;
    public static Joueur J2_IA;

    public static bool J1_humain_plays;
    public bool Pli2Plantes = false;
    public bool Pli3Plantes = false;
    public bool Pli3Choix = false;
    public bool Pli2Choix = true;
    public static bool escape = false;

    public int Pli3ChoixTourPlante1 = 0;
    public int Pli3ChoixTourPlante2 = 0;
    public int Pli3ChoixTourPlante3 = 0;


    public AudioSource audioCardFlip;
    public AudioSource audioPoseCard;
    public AudioSource audioGlisseCard;
    public AudioSource musique;

    public int cartesRangees;

    private IA ia;
    

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if ((Partie.modeSoutenance == true) && (MenuManager.typeJeu == 2))
                Time.timeScale = Partie.speed;
            else
                Time.timeScale = 1f;



            if (escape == true)
            {
                Joueur.pauseJeu2 = false;
                StartCoroutine(J1_humain.FermerPanelPause(false, false));
                escape = false;
                if((Partie.modeSoutenance == true) && (MenuManager.typeJeu == 2))
                    Time.timeScale = Partie.speed;
                else
                    Time.timeScale = 1f;
            }

            else
            {
                escape = true;
                StartCoroutine(Joueur.PausePanelJeu());
            }
        }

    }

    void Start() {                  //1ere fonction appelée du jeu. Initalise les Cartes en dur, ne touche pas encore au graphique.

        //Time.timeScale = 5f;
        //musique.Play();

        

        DontDestroyOnLoad(musique);
        if (Partie.numeroPartie == 1)
            Partie.LancementJeu();

        if((Partie.modeSoutenance == true) && (MenuManager.typeJeu == 2))
            Joueur.PanelPointsGraphe();
        
        bool isFullScreen = false;
        int desiredFPS = 60;

        Screen.SetResolution(1190, 700, isFullScreen, desiredFPS);
        
        cartesVitrine = new List<Carte>();

        J1_humain = gameObject.AddComponent<Joueur>() as Joueur;
        J2_IA = gameObject.AddComponent<Joueur>() as Joueur;
        

        for (int id = 1; id <= 12; id++)  {         //Initialise les valeurs des cartes en fonction de leurs mois.
            switch (id)
            {
                
                case (1):
                    cartesVitrine.Add(new Lumiere(id, 1));
                    cartesVitrine.Add(new Ruban(id, 2));
                    break;

                case (2):
                    cartesVitrine.Add(new Animal(id, 1));
                    cartesVitrine.Add(new Ruban(id, 2));
                    break;

                case (3):
                    cartesVitrine.Add(new Lumiere(id, 1));
                    cartesVitrine.Add(new Ruban(id, 2));
                    break;

                case (4):
                    cartesVitrine.Add(new Animal(id, 1));
                    cartesVitrine.Add(new Ruban(id, 2));
                    break;

                case (5):
                    cartesVitrine.Add(new Animal(id, 1));
                    cartesVitrine.Add(new Ruban(id, 2));
                    break;

                case (6):
                    cartesVitrine.Add(new Animal(id, 1));
                    cartesVitrine.Add(new Ruban(id, 2));
                    break;

                case (7):
                    cartesVitrine.Add(new Animal(id, 1));
                    cartesVitrine.Add(new Ruban(id, 2));
                    break;

                case (8):
                    cartesVitrine.Add(new Animal(id, 1));
                    cartesVitrine.Add(new Lumiere(id, 2));
                    break;

                case (9):
                    cartesVitrine.Add(new Animal(id, 1));
                    cartesVitrine.Add(new Ruban(id, 2));
                    break;

                case(10):
                    cartesVitrine.Add(new Animal(id, 1));
                    cartesVitrine.Add(new Ruban(id, 2));
                    break;

                case(11):
                    cartesVitrine.Add(new Animal(id, 1));
                    cartesVitrine.Add(new Ruban(id, 2));
                    break;

                case (12):
                    cartesVitrine.Add(new Lumiere(id, 1)); //Y a 3 plantes pour decembre
                    cartesVitrine.Add(new Plante(id, 2));
                    break;

            }

            cartesVitrine.Add(new Plante(id, 3));

            if (id != 11)
            { //toutes les mois sauf novembre (rain man)
                cartesVitrine.Add(new Plante(id, 4));
            }
            else
            { //Rain man dans Lumieres (et non pas plante)
                cartesVitrine.Add(new Lumiere(id, 4));
            }
        }
        
        

        StartCoroutine(Distribuer(cards));
    }


    IEnumerator Distribuer(List <GameCard> cards) {     //Mélange les cartes et créé jeuMoi, jeuEnnemi, pioche et défosse.

        bool reboot = false;

        bool crash = false;
        cardsShuffled = new List<Carte>();
        coordInitialesDefosse = new Vector3[12];
        coordInitialesCartes = new Vector3[48];
        coordDefosse = new Vector3[12];
        defosseGameCard = new GameCard[12];
        coordSupp = new Vector3[12];
        int[] compteurMois = new int[12];
        int[] compteurMoisJoueur = new int[12];
        int[] compteurMoisIA = new int[12];
        int valeur = 1;

        for (int i = 0; i < 12; i++)
        {           //Initialise coordInitialesDefosse, coordDefosse et defosseGameCard à 0 ou null (normal, on a pas encore déterminé quelques cartes allaient dans la défosse.)

            coordInitialesDefosse[i] = Vector3.zero;
            coordDefosse[i] = Vector3.zero;
            defosseGameCard[i] = null;
            compteurMois[i] = 0;
            compteurMoisJoueur[i] = 0;
            compteurMoisIA[i] = 0;
            coordSupp[i] = cartesSupp[i].transform.position;
        }


        for (int i = 0; i < cards.Count; i++)
        {  //les cartes mélangées sont des références de Cartes Vitrines (qui ne bouge pas pendant tout le programme)

            cardsShuffled.Add(cartesVitrine[i]);
            coordInitialesCartes[i] = cards[i].transform.position;
            cards[i].transform.position =  cards[25].transform.position;
            //StartCoroutine(Pause(1.0f, cards[i]));
        }
        
        StartCoroutine(Joueur.DebutPartie());
        
        yield return new WaitWhile(() => Joueur.pauseJeuDebut);
        if(!((Partie.modeSoutenance == true) && (MenuManager.typeJeu == 2)))
            audioGlisseCard.Play();
        for (int i = 0; i < cards.Count; i++)
        {  
            iTween.MoveTo(cards[i].gameObject, coordInitialesCartes[i], 2.0f);
            
        }



        do
        {

            System.Random rng = new System.Random();
            int n = cardsShuffled.Count;
           while (n > 1)                         //Mélange les cartes
             {
                int k;
                 n--;
                 if(((Partie.modeSoutenance == true) && (MenuManager.typeJeu == 2)))
                    k = Partie.rng.Next(n + 1);
                else
                    k = rng.Next(n + 1);
                Carte value = cardsShuffled[k];
                 cardsShuffled[k] = cardsShuffled[n];
                 cardsShuffled[n] = value;
             }

            
            jeuHumain = new List<Carte>();

            jeuIA = new List<Carte>();

            defosse = new List<Carte>();

            pioche = new List<Carte>();


            for (int i = 0; i < 24; i += 3)
            {     //on distribue 1 carte à moi, une carte à l'ennemi, une carte à la défosse (x8), sans remise

                jeuHumain.Add(cardsShuffled[i]);
                jeuIA.Add(cardsShuffled[i + 1]);
                defosse.Add(cardsShuffled[i + 2]);
            }

            for (int i = 24; i < cardsShuffled.Count; i++)
            { //le reste va à la pioche

                pioche.Add(cardsShuffled[i]);
            }

            /*
            jeuIA[0] = cardsShuffled[0];
            jeuIA[1] = cardsShuffled[1];
            jeuIA[2] = cardsShuffled[2];
            jeuIA[3] = cardsShuffled[3];
            jeuIA[4] = cardsShuffled[23];
            jeuIA[5] = cardsShuffled[9];
            jeuIA[6] = cardsShuffled[12];
            jeuIA[7] = cardsShuffled[13];*/
            

            //defosse[2] = cardsShuffled[22];
            //jeuEnnemi[7] = cardsShuffled[8];
            //defosse[3] = cardsShuffled[21];
            //jeuMoi[7] = cardsShuffled[10];

            //defosse[2] = cardsShuffled[21];
            //jeuMoi[7] = cardsShuffled[8];

            //jeuHumain[0] = cardsShuffled[19];
            //jeuIA[6] = cardsShuffled[0];

            reboot = false;
            
            foreach (Carte d in defosse)                 //pour empecher qu'il y ai 4 cartes de la meme famille dans la defosse
            {
                compteurMois[((int)d.Mois) - 1]++;
                
                if (compteurMois[((int)d.Mois) - 1] == 4)
                {
                    Debug.Log("4 cartes meme mois defosse");
                    reboot = true;
                }

            }
        } while (reboot == true);





        for (int i = 0; i < 8; i++)
        {
            valeur = ((int)jeuHumain[i].Mois - 1) * 4 + ((int)jeuHumain[i].Rang - 1); //On prend la 1ere carte dans la liste GameCard, on lui donne la premiere valeur de mon jeu
            cards[i].GetComponent<GameCard>().CopyInformations(jeuHumain[i], valeur, true, true);    //on lui affiche le sprite correspondant (initialisation des GameCard)
        }
        J1_humain.jeu = jeuHumain;

        for (int i = 8; i < 16; i++)
        {

            valeur = ((int)jeuIA[i - 8].Mois - 1) * 4 + ((int)jeuIA[i - 8].Rang - 1); //On prend la 8eme carte dans la liste GameCard, on lui donne la premiere valeur du jeu ennemi
            if(MenuManager.typeJeu == 1)
                cards[i].GetComponent<GameCard>().CopyInformations(jeuIA[i - 8], valeur, false, false);
            else
                cards[i].GetComponent<GameCard>().CopyInformations(jeuIA[i - 8], valeur, true, true);
        }
        J2_IA.jeu = jeuIA;

        for (int i = 16; i < 24; i++)
        {

            valeur = ((int)defosse[i - 16].Mois - 1) * 4 + ((int)defosse[i - 16].Rang - 1); //On prend la 16eme carte dans la liste GameCard, on lui donne la premiere valeur de la defosse
            cards[i].GetComponent<GameCard>().CopyInformations(defosse[i - 16], valeur, true, false);
            coordInitialesDefosse[i - 16] = coordInitialesCartes[i];                  //Maintenant qu'on sait ce qu'il y a dans la défosse, on peut enregistrer les coordonées des cartes
            coordDefosse[i - 16] = cards[i].transform.position;
            defosseGameCard[i - 16] = cards[i];
        }

        for (int i = 24; i < 48; i++)
        {
            valeur = ((int)pioche[i - 24].Mois - 1) * 4 + ((int)pioche[i - 24].Rang - 1);
            cards[i].GetComponent<GameCard>().CopyInformations(pioche[i - 24], valeur, false, false);
        }

        int cpt = 0;

        foreach (Carte d in jeuHumain)                 
        {
            compteurMoisJoueur[((int)d.Mois) - 1]++;

            if (compteurMoisJoueur[((int)d.Mois) - 1] == 4)
            {
                crash = true;
                Debug.Log("4 cartes meme mois joueur");
                if(MenuManager.typeJeu == 1)
                    DesactiverOuActiverBoutons(true);
                yield return new WaitForSeconds(3f);
                J1_humain.FinManche(J1_humain.scoreTotal, J2_IA.scoreTotal, false, 1, false, false, true, false);
                yield return new WaitForSeconds(12f);

            }

            if (compteurMoisJoueur[((int)d.Mois) - 1] == 2)
            {
                cpt++;
            }

        }
        if (cpt == 4)
        {
            crash = true;
            Debug.Log("4 paires joueur");
            if (MenuManager.typeJeu == 1)
                DesactiverOuActiverBoutons(true);
            yield return new WaitForSeconds(3f);
            J1_humain.FinManche(J1_humain.scoreTotal, J2_IA.scoreTotal, false, 1, false, false, false, true);
            yield return new WaitForSeconds(12f);
        }


        cpt = 0;
        foreach (Carte d in jeuIA)                 
        {
            compteurMoisIA[((int)d.Mois) - 1]++;

            if (compteurMoisIA[((int)d.Mois) - 1] == 4)
            {
                crash = true;
                Debug.Log("4 cartes meme mois IA");
                if (MenuManager.typeJeu == 1)
                    DesactiverOuActiverBoutons(true);
                yield return new WaitForSeconds(3f);
                J2_IA.FinManche(J1_humain.scoreTotal, J2_IA.scoreTotal, false, 2, false, false, true, false);
                yield return new WaitForSeconds(12f);
            }
            if (compteurMoisIA[((int)d.Mois) - 1] == 2)
            {
                cpt++;
            }

        }
        if (cpt == 4)
        {
            crash = true;
            Debug.Log("4 paires IA");
            if (MenuManager.typeJeu == 1)
                DesactiverOuActiverBoutons(true);
            yield return new WaitForSeconds(3f);
            J2_IA.FinManche(J1_humain.scoreTotal, J2_IA.scoreTotal, false, 2, false, false, false, true);
            yield return new WaitForSeconds(12f);
        }



        coordInitialesDefosse[8] = coordSupp[0];         //on a pu récupérer que les 8 premieres coordonnées de la défosse (car il n'y a que 8 cartes en défosse au début), on rajoute dans les 4 potentielles suivantes à la main
        coordInitialesDefosse[9] = coordSupp[1];
        coordInitialesDefosse[10] = coordSupp[2];
        coordInitialesDefosse[11] = coordSupp[3];


        ia = gameObject.AddComponent<IA>() as IA;

        if (crash == false)
        {
            if (Partie.dealer == 1)
            {
                J1_humain_plays = true;
                GameCard.J1_humain_plays = true;
                if (MenuManager.typeJeu == 1)
                    DesactiverOuActiverBoutons(false);
                else
                    StartCoroutine(Pause2());
            }
            else if (Partie.dealer == 2)
            {
                J1_humain_plays = false;
                if (MenuManager.typeJeu == 1)
                    DesactiverOuActiverBoutons(true);
                GameCard.J1_humain_plays = false;
                StartCoroutine(Pause2());
            }
        }

        

    }






    public void RangerPlis(int TypePli)
    {
        int cpt = 0;
        

            switch (TypePli) {
            

                case (1):
                    if (J1_humain.PlisLumiere.Count >= 2)
                    {
                        for (int i = 0; i<J1_humain.PlisLumiere.Count; i++)
                        {
                            foreach (GameCard c in cards)
                            {
                                if (c.Proprietes.Id == J1_humain.PlisLumiere[i].Id)
                                {
                                    cpt++;
                                    c.endPosition = new Vector3(coordSupp[4].x + (75 / J1_humain.PlisLumiere.Count) * (cpt - 1), coordSupp[4].y, 0);
                                    iTween.MoveTo(c.gameObject, c.endPosition, 1f);
                                }
                            }
                        }
                    }
                    break;

                case (2):
                    if (J2_IA.PlisLumiere.Count >= 2)
                    {
                        for (int i = 0; i < J2_IA.PlisLumiere.Count; i++)
                        {
                            foreach (GameCard c in cards)
                            {
                                if (c.Proprietes.Id == J2_IA.PlisLumiere[i].Id)
                                {
                                    cpt++;
                                    c.endPosition = new Vector3(coordSupp[8].x - (75 / J2_IA.PlisLumiere.Count) * (cpt - 1), coordSupp[8].y, 0);
                                    iTween.MoveTo(c.gameObject, c.endPosition, 1f);

                                }
                            }
                        }

                    }
                    break;


                case (3):
                    if (J1_humain.PlisRuban.Count >= 2)
                    {
                        for (int i = 0; i < J1_humain.PlisRuban.Count; i++)
                        {
                            foreach (GameCard c in cards)
                            {
                                if (c.Proprietes.Id == J1_humain.PlisRuban[i].Id)
                                {
                                    cpt++;
                                    c.endPosition = new Vector3(coordSupp[7].x + (200 / J1_humain.PlisRuban.Count) * (cpt - 1), coordSupp[7].y, 0);
                                    iTween.MoveTo(c.gameObject, c.endPosition, 1f);
                                }
                            }
                        }
                    }
                    break;

                case (4):
                    if (J2_IA.PlisRuban.Count >= 2)
                    {
                        for (int i = 0; i < J2_IA.PlisRuban.Count; i++)
                        {
                            foreach (GameCard c in cards)
                            {
                                if (c.Proprietes.Id == J2_IA.PlisRuban[i].Id)
                                {
                                    cpt++;
                                    c.endPosition = new Vector3(coordSupp[11].x - (200 / J2_IA.PlisRuban.Count) * (cpt - 1), coordSupp[11].y, 0);
                                    iTween.MoveTo(c.gameObject, c.endPosition, 1f);
                                }
                            }
                        }
                    }
                    break;

                case (5):
                    if (J1_humain.PlisPlante.Count >= 2)
                    {
                        for (int i = 0; i < J1_humain.PlisPlante.Count; i++)
                        {
                            foreach (GameCard c in cards)
                            {
                                if (c.Proprietes.Id == J1_humain.PlisPlante[i].Id)
                                {
                                    cpt++;
                                    c.endPosition = new Vector3(coordSupp[5].x + (225 / J1_humain.PlisPlante.Count) * (cpt - 1), coordSupp[5].y, 0);
                                    iTween.MoveTo(c.gameObject, c.endPosition, 1f);
                                }
                            }
                        }
                    }
                    break;

                case (6):
                    if (J2_IA.PlisPlante.Count >= 2)
                    {
                        for (int i = 0; i < J2_IA.PlisPlante.Count; i++)
                        {
                            foreach (GameCard c in cards)
                            {
                                if (c.Proprietes.Id == J2_IA.PlisPlante[i].Id)
                                {
                                    cpt++;
                                    c.endPosition = new Vector3(coordSupp[9].x - (225 / J2_IA.PlisPlante.Count) * (cpt - 1), coordSupp[9].y, 0);
                                    iTween.MoveTo(c.gameObject, c.endPosition, 1f);
                                }
                            }
                        }
                    }
                    break;

                case (7):
                    if (J1_humain.PlisAnimal.Count >= 2)
                    {
                        for (int i = 0; i < J1_humain.PlisAnimal.Count; i++)
                        {
                            foreach (GameCard c in cards)
                            {
                                if (c.Proprietes.Id == J1_humain.PlisAnimal[i].Id)
                                {
                                    cpt++;
                                    c.endPosition = new Vector3(coordSupp[6].x + (200 / J1_humain.PlisAnimal.Count) * (cpt - 1), coordSupp[6].y, 0);
                                    iTween.MoveTo(c.gameObject, c.endPosition, 1f);
                                }
                            }
                        }
                    }
                    break;

                case (8):
                    if (J2_IA.PlisAnimal.Count >= 2)
                    {
                        for (int i = 0; i < J2_IA.PlisAnimal.Count; i++)
                        {
                            foreach (GameCard c in cards)
                            {
                                if (c.Proprietes.Id == J2_IA.PlisAnimal[i].Id)
                                {
                                    cpt++;
                                    c.endPosition = new Vector3(coordSupp[10].x - (200 / J2_IA.PlisAnimal.Count) * (cpt - 1), coordSupp[10].y, 0);
                                    iTween.MoveTo(c.gameObject, c.endPosition, 1f);
                                }
                            }
                        }
                    }
                    break;

            }
        
    }



    public void ResizeJeu()
    {
        int rang = 0;
        int cpt = 0;


        if (J1_humain_plays)
        {
            for (int i = 0; i < jeuHumain.Count; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (cards[j].Proprietes.Id == jeuHumain[i].Id)
                    {
                        if (cards[j].transform.position != coordInitialesCartes[i])
                        {
                            cards[j].endPosition = new Vector3(coordInitialesCartes[i].x, coordInitialesCartes[i].y, 0);
                            iTween.MoveTo(cards[j].gameObject, cards[j].endPosition, 1f);
                        }
                    }
                }
            }
        }

        else
        {

            for (int i = 0; i < jeuIA.Count; i++)
            {
                for (int j = 8; j < 16; j++)
                {
                    if (cards[j].Proprietes.Id == jeuIA[i].Id)
                    {
                        if (cards[j].transform.position != coordInitialesCartes[i])
                        {
                            cards[j].endPosition = new Vector3(coordInitialesCartes[i+8].x, coordInitialesCartes[i+8].y, 0);
                            iTween.MoveTo(cards[j].gameObject, cards[j].endPosition, 1f);
                        }
                    }
                }
            }

        }
    }

    
    


    public GameCard checkCards(GameCard c, int tour) {      //Parcoure la défosse pour savoir on peut faire une paire avec la carte du Joueur ou la carte de la Pioche (tour 1 pour le joueur, tour 2 pour la pioche)
        
        
        cartesRangees = J1_humain.PlisLumiere.Count + J1_humain.PlisRuban.Count + J1_humain.PlisAnimal.Count + J1_humain.PlisPlante.Count + J2_IA.PlisLumiere.Count + J2_IA.PlisRuban.Count + J2_IA.PlisAnimal.Count + J2_IA.PlisPlante.Count;
        Pli2Plantes = false;
        Pli3Choix = false;
        Pli2Choix = false;
        Pli3Plantes = false;
        choixPaireClick = false;

        int cpt = 0;
        List<Carte> paires;
        paires = new List<Carte>();

        foreach (Carte d in defosse) {            //on regarde si y a une paire possible
            if (c.Proprietes.Mois == d.Mois) {
                paires.Add(d);
                cpt++;
            }
        }

        
        if (cpt == 0)  {                                     //Si y a pas de paires possibles
            defosse.Add(c.Proprietes);                      //On rajoute la carte actuelle à la défosse

            if (tour == 1)
            {
                if (J1_humain_plays)
                    jeuHumain.Remove(c.Proprietes);
                else
                    jeuIA.Remove(c.Proprietes);
            }
            return null;
        }

        else if ((cpt == 1)) {                              //Si il y a une paire possible

            if (J1_humain_plays) {
                J1_humain.AjouterPlis(c.Proprietes);    //on rajoute la carte aux plis du joueur
                J1_humain.AjouterPlis(paires[0]);       //on rajoute la carte paire aux plis du joueur
                if (tour == 1)
                    jeuHumain.Remove(c.Proprietes);                        //Si on est au tour du joueur, on enlève la carte jouée de son jeu
            }

            else {
                J2_IA.AjouterPlis(c.Proprietes);
                J2_IA.AjouterPlis(paires[0]);
                if (tour == 1)
                    jeuIA.Remove(c.Proprietes);

            }
            

            defosse.Remove(paires[0]);                                  //on retire la carte paire de la défosse


            if ((c.Proprietes is Plante) && (paires[0] is Plante))      //si c'est une paire avec deux plantes, il faut gérer l'affichage différemment (voir RangerCartes)
                Pli2Plantes = true;

            foreach (GameCard e in cards)  {                             //on va trouver la GameCard qui correspond à notre carte paire
           
                if (e.Proprietes.Id == paires[0].Id) {

                    for (int i = 0; i < 12; i++) {
                        if (defosseGameCard[i] != null) {
                            if (e.Proprietes.Id == defosseGameCard[i].Proprietes.Id) {
                                coordDefosse[i] = Vector3.zero;         //puisqu'on enlève une carte de la défosse, son emplacement devient libre
                                defosseGameCard[i] = null;
                                paires.Clear();
                                return e;
                            }
                        }
                    }
                }
            }
        }

        else if (cpt == 2)
        {

            Debug.Log("On est passé ici");
            Pli2Choix = true;
           
        }




        else if (cpt == 3) {

            gameCard3choix = CheckCards3choix(c, paires, tour);     //on récupère la liste avec les 3 cartes possibles
                                                                    //Il faut maintenant gérér le problème de Pli2Plantes : il faut savoir quelle plante a été posée dans quelle ordre
            if (c.Proprietes.Mois == Mois.Decembre) {               //Pour le mois de décembre il y a 3 plantes
                Pli3Plantes = true;
                if (!(c.Proprietes is Plante)) {
                    Pli3ChoixTourPlante1 = 2;
                    Pli3ChoixTourPlante2 = 3;
                    Pli3ChoixTourPlante3 = 4;

                }
                else if (!(gameCard3choix[0].Proprietes is Plante)) {
                    Pli3ChoixTourPlante1 = 1;
                    Pli3ChoixTourPlante2 = 3;
                    Pli3ChoixTourPlante3 = 4;

                }
                else if (!(gameCard3choix[1].Proprietes is Plante)) {
                    Pli3ChoixTourPlante1 = 1;
                    Pli3ChoixTourPlante2 = 2;
                    Pli3ChoixTourPlante3 = 4;

                }
                else if (!(gameCard3choix[2].Proprietes is Plante)) {
                    Pli3ChoixTourPlante1 = 1;
                    Pli3ChoixTourPlante2 = 2;
                    Pli3ChoixTourPlante3 = 3;

                }
            }

            else {
                if(c.Proprietes.Mois != Mois.Novembre)
                    Pli2Plantes = true;
                if (c.Proprietes is Plante) {
                    Pli3ChoixTourPlante1 = 1;
                    if (gameCard3choix[0].Proprietes is Plante)
                        Pli3ChoixTourPlante2 = 2;
                    else if (gameCard3choix[1].Proprietes is Plante)
                        Pli3ChoixTourPlante2 = 3;
                    else
                        Pli3ChoixTourPlante2 = 4;
                }

                else if (gameCard3choix[0].Proprietes is Plante) {
                    Pli3ChoixTourPlante1 = 2;
                    if (gameCard3choix[1].Proprietes is Plante)
                        Pli3ChoixTourPlante2 = 3;
                    else
                        Pli3ChoixTourPlante2 = 4;
                }

                else if (gameCard3choix[1].Proprietes is Plante) {
                    Pli3ChoixTourPlante1 = 3;
                    Pli3ChoixTourPlante2 = 4;
                }
            }
            
            Pli3Choix = true;
            GameCard.isRangee3 = false;
            GameCard.isRangee4 = false;
            return gameCard3choix[0];                                   //on retourne la premiere carte des 3 pour en faire la paire, et on rangera les 2 autres depuis la classe GameCard

        }
        
        return c;   //inutile
        
    }



    public IEnumerator CheckCards2choix(GameCard c, int tour)
    {
        Debug.Log("PLI 2 CHOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOIX");
        Debug.Log(tour);
        pauseJeu2Choix = true;
        cartesRangees = J1_humain.PlisLumiere.Count + J1_humain.PlisRuban.Count + J1_humain.PlisAnimal.Count + J1_humain.PlisPlante.Count + J2_IA.PlisLumiere.Count + J2_IA.PlisRuban.Count + J2_IA.PlisAnimal.Count + J2_IA.PlisPlante.Count;
        Pli2Plantes = false;
        Pli3Choix = false;
        Pli2Choix = false;
        Pli3Plantes = false;
        choixPaireClick = false;

        int cpt = 0;
        List<Carte> paires;
        paires = new List<Carte>();

        foreach (Carte d in defosse)
        {            //on regarde si y a une paire possible
            if (c.Proprietes.Mois == d.Mois)
            {
                paires.Add(d);
                cpt++;
            }
        }



        

        if ((J1_humain_plays == false) || (MenuManager.typeJeu == 2))
        {
            if (MenuManager.typeJeu == 1)
            {
                J2_IA.AjouterPlis(c.Proprietes);
                if (tour == 1)
                {
                    J2_IA.AjouterPlis(IA.carte_defosse);
                    jeuIA.Remove(c.Proprietes);
                }
                else
                    J2_IA.AjouterPlis(paires[0]);
            }
            else
            {
                if (J1_humain_plays)
                {
                    J1_humain.AjouterPlis(c.Proprietes);

                    if (tour == 1)
                    {
                        J1_humain.AjouterPlis(IA.carte_defosse);
                        jeuHumain.Remove(c.Proprietes);
                    }
                    else
                        J1_humain.AjouterPlis(paires[0]);
                }
                else
                {
                    J2_IA.AjouterPlis(c.Proprietes);
                    if (tour == 1)
                    {
                        J2_IA.AjouterPlis(IA.carte_defosse);
                        jeuIA.Remove(c.Proprietes);
                    }
                    else
                        J2_IA.AjouterPlis(paires[0]);
                }
            }

            if(tour == 1)
                defosse.Remove(IA.carte_defosse);

            else
                defosse.Remove(paires[0]);


            if (tour == 1)
            {
                foreach (GameCard e in cards)
                {                             //on va trouver la GameCard qui correspond à notre carte paire
                    if (pauseJeu2Choix == true)
                    {
                        if (e.Proprietes.Id == IA.carte_defosse.Id)
                        {

                            for (int i = 0; i < 12; i++)
                            {
                                if (defosseGameCard[i] != null)
                                {
                                    if (e.Proprietes.Id == defosseGameCard[i].Proprietes.Id)
                                    {
                                        coordDefosse[i] = Vector3.zero;         //puisqu'on enlève une carte de la défosse, son emplacement devient libre
                                        defosseGameCard[i] = null;
                                        paires.Clear();
                                        gameCard2choix = e;
                                        pauseJeu2Choix = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            else
            {
                foreach (GameCard e in cards)
                {                             //on va trouver la GameCard qui correspond à notre carte paire
                    if (pauseJeu2Choix == true)
                    {
                        if (e.Proprietes.Id == paires[0].Id)
                        {

                            for (int i = 0; i < 12; i++)
                            {
                                if (defosseGameCard[i] != null)
                                {
                                    if (e.Proprietes.Id == defosseGameCard[i].Proprietes.Id)
                                    {
                                        coordDefosse[i] = Vector3.zero;         //puisqu'on enlève une carte de la défosse, son emplacement devient libre
                                        defosseGameCard[i] = null;
                                        paires.Clear();
                                        gameCard2choix = e;
                                        pauseJeu2Choix = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
        }

        else
        {

            
            foreach (GameCard e in cards)
            {
                if (e.Proprietes.Id == paires[0].Id)
                {
                    e.GetComponent<Button>().enabled = true;
                    e.GetComponent<Button>().interactable = true;
                }


                else if (e.Proprietes.Id == paires[1].Id)
                {
                    e.GetComponent<Button>().enabled = true;
                    e.GetComponent<Button>().interactable = true;
                }
            }
            choixPaireClick = true;
            yield return new WaitWhile(() => choixPaireClick);


            J1_humain.AjouterPlis(c.Proprietes);    //on rajoute la carte aux plis du joueur
            if (tour == 1)
                jeuHumain.Remove(c.Proprietes);

            foreach (GameCard e in cards)
            {
                if (e.Proprietes.Id == paires[0].Id)
                {
                    
                    e.GetComponent<Button>().interactable = false;
                    e.GetComponent<Button>().enabled = false;
                }


                else if (e.Proprietes.Id == paires[1].Id)
                {
                    
                    e.GetComponent<Button>().interactable = false;
                    e.GetComponent<Button>().enabled = false;
                }
            }

            foreach (Carte f in cartesVitrine)
            {

                if (f.Id == GameCard.paire2choix.Proprietes.Id)
                {
                    J1_humain.AjouterPlis(f);
                    defosse.Remove(f);
                    for (int i = 0; i < 12; i++)
                    {
                        if (defosseGameCard[i] != null)
                        {
                            if (GameCard.paire2choix.Proprietes.Id == defosseGameCard[i].Proprietes.Id)
                            {
                                coordDefosse[i] = Vector3.zero;         //puisqu'on enlève une carte de la défosse, son emplacement devient libre
                                defosseGameCard[i] = null;
                                paires.Clear();
                                gameCard2choix = GameCard.paire2choix;
                                pauseJeu2Choix = false;
                            }
                        }
                    }
                }
            }
        }
               
    }

   

    public List<GameCard> CheckCards3choix(GameCard c, List<Carte> paires, int tour) {

        gameCard3choix = new List<GameCard>();
        Debug.Log("Pli 3 choix!");

        if (J1_humain_plays) {
            J1_humain.AjouterPlis(c.Proprietes);
            J1_humain.AjouterPlis(paires[0]);
            J1_humain.AjouterPlis(paires[1]);
            J1_humain.AjouterPlis(paires[2]);
            if (tour == 1)
                jeuHumain.Remove(c.Proprietes);
        }

        else {
            J2_IA.AjouterPlis(c.Proprietes);
            J2_IA.AjouterPlis(paires[0]);
            J2_IA.AjouterPlis(paires[1]);
            J2_IA.AjouterPlis(paires[2]);
            if (tour == 1)
                jeuIA.Remove(c.Proprietes);
        }

        defosse.Remove(paires[0]);
        defosse.Remove(paires[1]);
        defosse.Remove(paires[2]);
        


        foreach (GameCard e in cards)  {     //on trouve les GameCard qui correspondent aux 3 cartes trouvées
        
            for (int i = 0; i < 3; i++){
                if (e.Proprietes.Id == paires[i].Id)
                    gameCard3choix.Add(e);
                
            }
        }

        for (int j = 0; j < 12; j++) {    //on parcoure defosseGameCard pour lui enlever les 3 GameCard trouvées
            for (int k = 0; k < 3; k++)  {   //on trouve les GameCard qui correspondent aux defosseGameCard
            
                if (defosseGameCard[j] != null) {
                    if (gameCard3choix[k].Proprietes.Id == defosseGameCard[j].Proprietes.Id) {
                        coordDefosse[j] = Vector3.zero;
                        defosseGameCard[j] = null;
                    }
                }
            }
            
        }
        
        paires.Clear();
        return gameCard3choix;
    }



    public GameCard AfficherPioche() {           //Va chercher la premiere carte de la pioche et retourne sa GameCard

        int cpt = 0;
        foreach (GameCard c in cards) {
            if (c.Proprietes.Id == pioche[0].Id) {
                if (defosse.Count >= 12)
                {
                    foreach (Carte d in defosse)
                    {
                        if (pioche[0].Mois != d.Mois)
                        {
                            cpt++;
                        }

                    }
                    if(cpt == defosse.Count)
                    {
                        GameCard.defosse13 = true;
                        pioche.Remove(pioche[0]);
                        AfficherPioche();
                    }
                    else
                    {
                        GameCard.defosse13 = false;
                        pioche.Remove(pioche[0]);
                        return (c);
                    }
                }

                else
                {
                    GameCard.defosse13 = false;
                    pioche.Remove(pioche[0]);
                    return (c);
                }
            }
            
        }
        
        return null;        //inutile
    }


    public Sprite getCardBack()
    {
        return cardBack;
    }



    public Sprite getCardFace(int i)
    {
        return cardFace[i];
    }






    public GameCard IACarteToGameCard()
    {
        if(MenuManager.typeJeu == 1)
            J1_humain_plays = false;

        else
        {
            if (GameCard.J1_humain_plays == true)
                J1_humain_plays = true;
            else
                J1_humain_plays = false;
        }

        Carte c = ia.DemarrerIA();
        
        foreach(GameCard d in cards)
        {
            if (d.Proprietes.Id == c.Id)
            {
                 return d;
            }
        }
        return null;        //inutile
    }



    public void DesactiverOuActiverBoutons(bool desactiver)
    {
        int cpt = 0;
        foreach(GameCard c in cards)
        {
            foreach (Carte jeu in jeuHumain)
            {
                cpt = 0;
                if (c.Proprietes.Id == jeu.Id)
                {
                    

                    if (desactiver == true)
                    {
                        //c.GetComponent<Button>().enabled = false;
                        c.GetComponent<Button>().interactable = false;
                    }
                    else
                    {
                        //c.GetComponent<Button>().enabled = true;
                        c.GetComponent<Button>().interactable = true;
                        J1_humain_plays = true;
                    }


                    if (defosse.Count >= 12)         //pour empecher de rajouter une 13e carte à la defosse
                    {
                        foreach (Carte defausse in defosse)
                        {
                            if (c.Proprietes.Mois != defausse.Mois)
                            {
                                cpt++;
                                
                            }
                            if(cpt == defosse.Count)
                            {
                                c.GetComponent<Button>().interactable = false;
                            }

                        }
                    }
                }
                    
            }

        }

    }


    public void GriserBouttons(int joueur, bool desactiver)
    {
        if(joueur == 1)
        {
            foreach (GameCard c in cards)
            {
                foreach(Carte k in jeuHumain)
                {
                    if(c.Proprietes.Id == k.Id)
                    {

                        if (desactiver == true)
                        {
                            c.GetComponent<Button>().enabled = true;
                            c.GetComponent<Button>().interactable = false;
                        }
                        else
                        {
                            c.GetComponent<Button>().enabled = true;
                            c.GetComponent<Button>().interactable = true;
                            c.GetComponent<Button>().enabled = false;
                        }
                    }
                        
                }
            }
        }

        else if (joueur == 2)
        {
            foreach (GameCard c in cards)
            {
                foreach (Carte k in jeuIA)
                {
                    if (c.Proprietes.Id == k.Id)
                    {
                        c.GetComponent<Button>().enabled = true;
                        if (desactiver == true)
                        {
                            c.GetComponent<Button>().enabled = true;
                            c.GetComponent<Button>().interactable = false;
                        }
                        else
                        {
                            c.GetComponent<Button>().enabled = true;
                            c.GetComponent<Button>().interactable = true;
                            c.GetComponent<Button>().enabled = false;
                        }
                    }
                       
                }
            }
        }
    }



    private IEnumerator Pause(float waitTime, GameCard c)     //pour que le jeu fasse une pause
    {

        //audioGlisseCard.Play();
        iTween.MoveFrom(c.gameObject, cards[25].transform.position, 2.0f);
        yield return new WaitForSeconds(waitTime);
        
        //Time.timeScale = 0;

    }

    private IEnumerator Pause2()     //pour que le jeu fasse une pause
    {

        //audioGlisseCard.Play();
        
        yield return new WaitForSeconds(2f);

        if (Partie.dealer == 2)
            StartCoroutine(cards[0].TourIA());

        else
            cards[0].TourJoueurIA();

        //Time.timeScale = 0;

    }


}
