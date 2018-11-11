using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameCard : MonoBehaviour           //L'objet physique d'une Carte. Toutes les modifications graphiques se font dans cette classe. Toutes les modifications qui se font depuis Unity affectent directement les objets.
{
    
    public Carte Proprietes { get; set; }       //Classe Carte
    public SpriteRenderer renderer;             //Nécessaire pour gérér l'ordre d'apparition des sprites
    private static GameCard paire;
    private static GameCard carteJoueeIA;
    private static GameCard carteJoueeJoueur;
    private static GameCard currPioche;
    public static GameCard paire2choix;

    [SerializeField]
    public int cardValue;                       //Correspond à l'index de la GameCard dans la liste Cards. Est visible depuis Unity

    public Sprite cardBack;                     //Image de derriere de carte (s'initialize depuis Unity)
    public Sprite cardFace;                     //Image de devant de carte (s'initialize depuis Unity)

    private GameObject manager;                 //Appelle la classe GameManager et l'objet Unity GameManager: tout ce qui est initialisé dans objet GameManager depuis Unity est instancé au tout début du code
    private bool isCliked = false;
    private bool isRangee = false;
    private bool isRangee2 = false;
    public static bool  isRangee3 = true;
    public static bool  isRangee4 = true;
    public bool aRanger = false;
    private static bool tourPioche = false;
    public static int VousNePasserezPas = 0;
    public static int VousNePasserezPas2 = 0;
    public static int VousNePasserezPas3 = 0;
    public static int VousNePasserezPas4 = 0;
    private static bool NoPaires = false;
    private static bool updateOn = true;
    public static bool defosse13 = false;
    private static bool passage = true;
    private static int cptAudio = 0;
    private static int type = 0;

    private static bool pauseJeu = false;

    private float speed;
    private float journeyLength;
    private static float startTime;
    private Vector3 startPosition;
    public Vector3 endPosition;

    private int cpt;

    private static Joueur J1_humain;
    private static Joueur J2_IA;
    public static bool J1_humain_plays;
    private bool tourIA = false;

    private static int pointsJ1;
    private static int pointsJ2;




    private void Start() {          //en quelques sortes le constructeur d'une GameCard. Est appelé systématiquement au lancement du jeu, et une seule fois. Ne prend pas de paramètres.

        VousNePasserezPas3 = 0;
        manager = GameObject.FindGameObjectWithTag("Manager");
        speed = 100.0f;
        renderer = new SpriteRenderer();
        renderer = GetComponent<SpriteRenderer>();
        J1_humain = GameManager.J1_humain;
        J2_IA = GameManager.J2_IA;
        pointsJ1 = 0;
        pointsJ2 = 0;
        if ((Partie.modeSoutenance == true) && (MenuManager.typeJeu == 2))
            Time.timeScale = Partie.speed;

    }

    //TRES IMPORTANT: Update() est appelée à chaque frame (seule fonction de Unity qui fait ca, elle est indispensable)
    //CONSEQUENCES: Update() va plus vite que le compilateur!!! Il est possible que le programme recommence la fonction avant qu'il ait finit de la parcourir 
    //Cela nécessite beaucoup de booleens et de conditions d'arrets pour éviter que le programme tourne en boucle

        

    private IEnumerator TourJoueur(GameCard c)
    {
        tourPioche = false;
        VousNePasserezPas = 0;

        if (VousNePasserezPas3 == 0)
        {
            if (MenuManager.typeJeu == 2)
            {
                yield return new WaitForSeconds(0.5f);
                manager.GetComponent<GameManager>().GriserBouttons(1, false);
                manager.GetComponent<GameManager>().GriserBouttons(2, true);
            }

            NoPaires = false;
            VousNePasserezPas3++;
            paire = manager.GetComponent<GameManager>().checkCards(c, 1);    //on regarde dans la défosse si y a une paire correspondante 
            if((manager.GetComponent<GameManager>().Pli2Choix == true))
            {
                Debug.Log("Pli 2 choix Joueur");
                StartCoroutine(manager.GetComponent<GameManager>().CheckCards2choix(c, 1));
                yield return new WaitWhile(() => GameManager.pauseJeu2Choix);
                paire = GameManager.gameCard2choix;
            }
            J1_humain = GameManager.J1_humain;
            J2_IA = GameManager.J2_IA;
            c.GetComponent<Button>().enabled = false;
            c.transform.SetSiblingIndex(47);
            if(MenuManager.typeJeu == 1)
                manager.GetComponent<GameManager>().DesactiverOuActiverBoutons(true);

            if (paire != null)
            {
                if (paire.Proprietes.Id == c.Proprietes.Id)
                {
                    Debug.Log("VCBN?QGDHQJKTEYQKGHDQJDS");
                    StartCoroutine(manager.GetComponent<GameManager>().CheckCards2choix(c, 1));
                    yield return new WaitWhile(() => GameManager.pauseJeu2Choix);
                    paire = GameManager.gameCard2choix;
                }
            }


            if (paire == null)
            {                        //Si il n'y en a pas, on va aller poser la carte joueur dans un emplacement vide
                Debug.Log("Pas de paires");
                c.endPosition = PasdePaires(c);
                NoPaires = true;
                

            }

            else
            {
                c.endPosition = paire.gameObject.transform.position;          //Si il y a une combinaison possible, on retourne la position de cette carte
                paire.transform.SetSiblingIndex(46);
                
            }
        }
        
        c.startPosition = c.gameObject.transform.position;
        c.journeyLength = Vector3.Distance(c.startPosition, c.endPosition);
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / c.journeyLength;
        
        if (c.transform.position != c.endPosition)
        {
            iTween.MoveTo(c.gameObject, c.endPosition, 1.0f);
            yield return null;
            StartCoroutine(TourJoueur(c));
            
        }

        else //une fois que le sprite a été bougé
        {      
                    
            manager.GetComponent<GameManager>().ResizeJeu();
            VousNePasserezPas3 = 0;
            if (NoPaires == false) //Si on a fait une combinaison, on doit maintenant ranger les cartes
            {  
                StartCoroutine(RangerCartes(c, 1));
            }

            else    //Si on a pas fait de combinaisons, on ne range rien et on passe au tour de la pioche
            {                  
                tourPioche = true;
                NoPaires = false;
                StartCoroutine(TourPioche());
                
            }

            startTime = Time.time;
        }
    }



    public IEnumerator TourIA()
    {
        tourPioche = false;
        VousNePasserezPas = 0;

        if (VousNePasserezPas3 == 0)
        {
            VousNePasserezPas3++;
            yield return new WaitForSeconds(0.3f);

            if(MenuManager.typeJeu == 2)
            {
                manager.GetComponent<GameManager>().GriserBouttons(1, true);
                manager.GetComponent<GameManager>().GriserBouttons(2, false);
            }

            carteJoueeIA = manager.GetComponent<GameManager>().IACarteToGameCard();
            carteJoueeIA.cardFace = manager.GetComponent<GameManager>().getCardFace(carteJoueeIA.cardValue);
            if (MenuManager.typeJeu == 1)
            {
                Vector3 patate2 = new Vector3(180, 180, 180);
                iTween.RotateTo(carteJoueeIA.gameObject, patate2, 0.5f);
                carteJoueeIA.GetComponent<Image>().sprite = carteJoueeIA.cardFace;
                manager.GetComponent<GameManager>().audioCardFlip.Play();
                yield return new WaitForSeconds(0.5f);
            }
            

            
            NoPaires = false;
            
            paire = manager.GetComponent<GameManager>().checkCards(carteJoueeIA, 1);    //on regarde dans la défosse si y a une paire correspondante 
            if ((manager.GetComponent<GameManager>().Pli2Choix == true)) 
            {
                Debug.Log("Pli 2 choix IA");
                StartCoroutine(manager.GetComponent<GameManager>().CheckCards2choix(carteJoueeIA, 1));
                yield return new WaitWhile(() => GameManager.pauseJeu2Choix);
                paire = GameManager.gameCard2choix;
            }
            J1_humain = GameManager.J1_humain;
            J2_IA = GameManager.J2_IA;
            carteJoueeIA.GetComponent<Button>().enabled = false;
            carteJoueeIA.transform.SetSiblingIndex(47);
            if(MenuManager.typeJeu == 1)
                 manager.GetComponent<GameManager>().DesactiverOuActiverBoutons(true);

            if (paire != null)
            {
                if (paire.Proprietes.Id == carteJoueeIA.Proprietes.Id)
                {
                    Debug.Log("GFHDJKGSHFJDKGFHSJKGHSQJKHQJDK");
                    StartCoroutine(manager.GetComponent<GameManager>().CheckCards2choix(carteJoueeIA, 1));
                    yield return new WaitWhile(() => GameManager.pauseJeu2Choix);
                    paire = GameManager.gameCard2choix;
                }
            }


            if (paire == null)
            {                        //Si il n'y en a pas, on va aller poser la carte joueur dans un emplacement vide
                carteJoueeIA.endPosition = PasdePaires(carteJoueeIA);
                NoPaires = true;


            }

            else
            {
                carteJoueeIA.endPosition = paire.gameObject.transform.position;          //Si il y a une combinaison possible, on retourne la position de cette carte
                paire.transform.SetSiblingIndex(46);
            }
        }

        carteJoueeIA.startPosition = carteJoueeIA.gameObject.transform.position;
        carteJoueeIA.journeyLength = Vector3.Distance(carteJoueeIA.startPosition, carteJoueeIA.endPosition);
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / carteJoueeIA.journeyLength;

        if (carteJoueeIA.transform.position != carteJoueeIA.endPosition)
        {

            iTween.MoveTo(carteJoueeIA.gameObject, carteJoueeIA.endPosition, 1.0f);
            yield return null;
            StartCoroutine(TourIA());
        }

        else    //une fois que le sprite a été bougé
        {      
            manager.GetComponent<GameManager>().ResizeJeu();
            VousNePasserezPas3 = 0;
            isCliked = false;
            if (NoPaires == false)  //Si on a fait une combinaison, on doit maintenant ranger les cartes
            {  
                cptAudio = 0;
                StartCoroutine(RangerCartes(carteJoueeIA, 1));
            }

            else
            {                  //Si on a pas fait de combinaisons, on ne range rien et on passe au tour de la pioche

                tourPioche = true;
                NoPaires = false;
                StartCoroutine(TourPioche());
            }

            startTime = Time.time;
        }
    }



    private IEnumerator TourPioche() {
        
        VousNePasserezPas2 = 0;
        VousNePasserezPas3 = 0;
        

        if (VousNePasserezPas == 0) {   //VousNePasserezPas est là pour faire en sorte que le programme ne recalcule pas la position de fin au 2e passage
            yield return new WaitForSeconds(0.3f);
            currPioche = manager.GetComponent<GameManager>().AfficherPioche();
            currPioche.cardFace = manager.GetComponent<GameManager>().getCardFace(currPioche.cardValue);
            currPioche.transform.SetSiblingIndex(47);
            
            Vector3 patate2 = new Vector3(180, 180, 180);
            iTween.RotateTo(currPioche.gameObject, patate2, 0.5f);
            currPioche.GetComponent<Image>().sprite = currPioche.cardFace;                      //on affiche le sprite de la pioche
            if (!((Partie.modeSoutenance == true) && (MenuManager.typeJeu == 2))) 
                manager.GetComponent<GameManager>().audioCardFlip.Play();
            yield return new WaitForSeconds(0.5f);

            NoPaires = false;
            VousNePasserezPas++;
            paire = manager.GetComponent<GameManager>().checkCards(currPioche, 2);          //on regarde si on peut faire une combinaison avec la carte tirée de la pioche
            if ((manager.GetComponent<GameManager>().Pli2Choix == true))
            {
                Debug.Log("Pli 2 choix Pioche");
                StartCoroutine(manager.GetComponent<GameManager>().CheckCards2choix(currPioche, 2));
                yield return new WaitWhile(() => GameManager.pauseJeu2Choix);
                paire = GameManager.gameCard2choix;
            }
            J1_humain = GameManager.J1_humain;
            J2_IA = GameManager.J2_IA;

            if (paire == null)
            {
                currPioche.endPosition = PasdePaires(currPioche);                           //cas où on ne peut pas faire de combinaisons (retourne un emplacement vide où on pourra poser la carte)
                NoPaires = true;
            }

            else
            {
                currPioche.endPosition = paire.gameObject.transform.position;               //cas où on peut faire combinaison
                paire.transform.SetSiblingIndex(46);
            }
            
        }
        
        currPioche.startPosition = currPioche.gameObject.transform.position;
        currPioche.journeyLength = Vector3.Distance(currPioche.startPosition, currPioche.endPosition);
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / currPioche.journeyLength;
        
        if (currPioche.transform.position != currPioche.endPosition)
        {
            
            iTween.MoveTo(currPioche.gameObject, currPioche.endPosition, 1.0f);
            yield return null;
            StartCoroutine(TourPioche());
        }


        else
        {      //quand c'est terminé
            if (NoPaires == false)   //si on a trouvé une combinaison, on peut ranger les 2 cartes
            {       
                cptAudio = 0;
                StartCoroutine(RangerCartes(currPioche, 1));
            }

            else
            {                          //si y avait pas de combinaisons, on range rien et le tour de la pioche est terminé
                tourPioche = false;
                VousNePasserezPas = 0;
                NoPaires = false;
                if (J1_humain_plays == true)
                {
                    J1_humain.CompterPoints();
                    if ((J1_humain.nbrePoints > 0) && ((pointsJ1 != J1_humain.nbrePoints)))
                    {
                        StartCoroutine(PauseJeu(1));
                    }
                    else if ((manager.GetComponent<GameManager>().jeuHumain.Count == 0) && (manager.GetComponent<GameManager>().jeuIA.Count == 0))
                    {
                        FinMancheMatchNul(1);
                    }

                    else
                    {
                        J1_humain_plays = false;
                        StartCoroutine(TourIA());

                    }
                }
                else
                {
                    J2_IA.CompterPoints();
                    if ((J2_IA.nbrePoints > 0) && ((pointsJ2 != J2_IA.nbrePoints)))
                    {
                        StartCoroutine(PauseJeu(2));
                    }
                    else if ((manager.GetComponent<GameManager>().jeuHumain.Count == 0) && (manager.GetComponent<GameManager>().jeuIA.Count == 0))
                    {
                        FinMancheMatchNul(2);
                    }
                    else
                    {
                        J1_humain_plays = true;
                        if (MenuManager.typeJeu == 1)
                            manager.GetComponent<GameManager>().DesactiverOuActiverBoutons(false);
                        else
                            TourJoueurIA();

                    }
                   
                }
            }
        }
    }

    

    public IEnumerator RangerCartes(GameCard c, int tour) { //determine l'emplacement de la carte dans les plis et la bouger
        
        if (VousNePasserezPas4 == 0)
        {
            type = 0;
            c.startPosition = c.gameObject.transform.position;
            if (c.Proprietes is Lumiere)
            {
                if (J1_humain_plays)
                {
                    type = 1;
                    // manager.GetComponent<GameManager>().RangerPlis(1);
                    cpt = J1_humain.PlisLumiere.Count;    //pour superposer les cartes dans les plis
                    c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[4].x + (75 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[4].y, 0);
                }
                else
                {
                    type = 2;
                    //manager.GetComponent<GameManager>().RangerPlis(2);
                    cpt = J2_IA.PlisLumiere.Count;
                    c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[8].x - (75 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[8].y, 0);
                }


            }

            else if (c.Proprietes is Ruban)
            {
                if (J1_humain_plays)
                {
                    type = 3;
                    //manager.GetComponent<GameManager>().RangerPlis(3);
                    cpt = J1_humain.PlisRuban.Count;
                    c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[7].x + (200 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[7].y, 0);
                }
                else
                {
                    type = 4;
                    //manager.GetComponent<GameManager>().RangerPlis(4);
                    cpt = cpt = J2_IA.PlisRuban.Count;
                    c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[11].x - (200 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[11].y, 0);
                }


            }

            else if (c.Proprietes is Plante)
            {
                if (J1_humain_plays)
                {
                    type = 5;
                    // manager.GetComponent<GameManager>().RangerPlis(5);
                    cpt = J1_humain.PlisPlante.Count;
                }
                else
                {
                    type = 6;
                    //manager.GetComponent<GameManager>().RangerPlis(6);
                    cpt = J2_IA.PlisPlante.Count;
                }




                if (manager.GetComponent<GameManager>().Pli2Plantes == true)
                {              //quand on fait une paire avec 2 plantes, il faut décaler la premiere pour pas qu'elles soient collés

                    if (manager.GetComponent<GameManager>().Pli3Choix == true)
                    {            //sert à determiner à quel tour la 1ere plante et la 2e plante sont posées (cas du Pli 3 choix)


                        if (tour == 1)
                        {
                            if (J1_humain_plays)
                                c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[5].x + (225 / cpt) * (cpt - 2), manager.GetComponent<GameManager>().coordSupp[5].y, 0);
                            else
                                c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[9].x - (225 / cpt) * (cpt - 2), manager.GetComponent<GameManager>().coordSupp[9].y, 0);
                        }


                        else if (tour == 2)
                        {
                            if ((manager.GetComponent<GameManager>().Pli3ChoixTourPlante1 == 2))
                            {
                                if (J1_humain_plays)
                                    c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[5].x + (225 / cpt) * (cpt - 2), manager.GetComponent<GameManager>().coordSupp[5].y, 0);
                                else
                                    c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[9].x - (225 / cpt) * (cpt - 2), manager.GetComponent<GameManager>().coordSupp[9].y, 0);
                            }


                            else if ((manager.GetComponent<GameManager>().Pli3ChoixTourPlante2 == 2))
                            {
                                if (J1_humain_plays)
                                    c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[5].x + (225 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[5].y, 0);
                                else
                                    c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[9].x - (225 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[9].y, 0);
                            }

                        }

                        else if (tour == 3)
                        {

                            if ((manager.GetComponent<GameManager>().Pli3ChoixTourPlante1 == 3))
                            {
                                if (J1_humain_plays)
                                    c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[5].x + (225 / cpt) * (cpt - 2), manager.GetComponent<GameManager>().coordSupp[5].y, 0);
                                else
                                    c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[9].x - (225 / cpt) * (cpt - 2), manager.GetComponent<GameManager>().coordSupp[9].y, 0);
                            }

                            else if ((manager.GetComponent<GameManager>().Pli3ChoixTourPlante2 == 3))
                            {
                                if (J1_humain_plays)
                                    c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[5].x + (225 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[5].y, 0);
                                else
                                    c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[9].x - (225 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[9].y, 0);
                            }

                        }

                        else if (tour == 4)
                        {
                            if (J1_humain_plays)
                                c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[5].x + (225 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[5].y, 0);
                            else
                                c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[9].x - (225 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[9].y, 0);
                        }
                    }

                    else
                    {
                        if (tour == 1)
                        {
                            if (J1_humain_plays)
                                c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[5].x + (225 / cpt) * (cpt - 2), manager.GetComponent<GameManager>().coordSupp[5].y, 0);
                            else
                                c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[9].x - (225 / cpt) * (cpt - 2), manager.GetComponent<GameManager>().coordSupp[9].y, 0);
                        }

                        else
                        {
                            if (J1_humain_plays)
                                c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[5].x + (225 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[5].y, 0);
                            else
                                c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[9].x - (225 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[9].y, 0);
                        }

                    }
                }

                else if (manager.GetComponent<GameManager>().Pli3Plantes == true)
                {                     //seulement dans le cas du Pli 3 choix et du mois de décembre (qui a 3 plantes)

                    if (tour == 1)
                    {
                        if (J1_humain_plays)
                            c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[5].x + (225 / cpt) * (cpt - 3), manager.GetComponent<GameManager>().coordSupp[5].y, 0);
                        else
                            c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[9].x - (225 / cpt) * (cpt - 3), manager.GetComponent<GameManager>().coordSupp[9].y, 0);
                    }


                    else if (tour == 2)
                    {

                        if ((manager.GetComponent<GameManager>().Pli3ChoixTourPlante1 == 2))
                        {
                            if (J1_humain_plays)
                                c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[5].x + (225 / cpt) * (cpt - 3), manager.GetComponent<GameManager>().coordSupp[5].y, 0);
                            else
                                c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[9].x - (225 / cpt) * (cpt - 3), manager.GetComponent<GameManager>().coordSupp[9].y, 0);
                        }


                        else if ((manager.GetComponent<GameManager>().Pli3ChoixTourPlante2 == 2))
                        {
                            if (J1_humain_plays)
                                c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[5].x + (225 / cpt) * (cpt - 2), manager.GetComponent<GameManager>().coordSupp[5].y, 0);
                            else
                                c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[9].x - (225 / cpt) * (cpt - 2), manager.GetComponent<GameManager>().coordSupp[9].y, 0);

                        }

                    }

                    else if (tour == 3)
                    {

                        if ((manager.GetComponent<GameManager>().Pli3ChoixTourPlante2 == 3))
                        {
                            if (J1_humain_plays)
                                c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[5].x + (225 / cpt) * (cpt - 2), manager.GetComponent<GameManager>().coordSupp[5].y, 0);
                            else
                                c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[9].x - (225 / cpt) * (cpt - 2), manager.GetComponent<GameManager>().coordSupp[9].y, 0);
                        }

                        else if ((manager.GetComponent<GameManager>().Pli3ChoixTourPlante3 == 3))
                        {
                            if (J1_humain_plays)
                                c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[5].x + (225 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[5].y, 0);
                            else
                                c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[9].x - (225 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[9].y, 0);
                        }

                    }

                    else if (tour == 4)
                    {
                        if (J1_humain_plays)
                            c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[5].x + (225 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[5].y, 0);
                        else
                            c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[9].x - (225 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[9].y, 0);
                    }

                }

                else                                                                                            //cas ordinaire pour une plante
                {
                    if (J1_humain_plays)
                        c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[5].x + (225 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[5].y, 0);
                    else
                        c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[9].x - (225 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[9].y, 0);
                }

            }

            else if (c.Proprietes is Animal)
            {
                if (J1_humain_plays)
                {
                    type = 7;
                    //manager.GetComponent<GameManager>().RangerPlis(7);
                    cpt = J1_humain.PlisAnimal.Count;
                    c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[6].x + (200 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[6].y, 0);
                }
                else
                {
                    type = 8;
                    //manager.GetComponent<GameManager>().RangerPlis(8);
                    cpt = J2_IA.PlisAnimal.Count;
                    c.endPosition = new Vector3(manager.GetComponent<GameManager>().coordSupp[10].x - (200 / cpt) * (cpt - 1), manager.GetComponent<GameManager>().coordSupp[10].y, 0);
                }


            }

        }
        c.journeyLength = Vector3.Distance(c.startPosition, c.endPosition);
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / c.journeyLength;

        if (c.journeyLength >= 0.1)
        {
            cptAudio++;
            
           
            if (cptAudio == 17)
            {
                if (!((Partie.modeSoutenance == true) && (MenuManager.typeJeu == 2)))
                {
                    manager.GetComponent<GameManager>().audioPoseCard.Play();

                    if (manager.GetComponent<GameManager>().Pli2Plantes != true)
                        manager.GetComponent<GameManager>().RangerPlis(type);
                }
            }
            iTween.MoveTo(c.gameObject, c.endPosition, 0.3f);
            yield return null;
            StartCoroutine(RangerCartes(c, tour));
        }


        else 
        {
            if((Partie.modeSoutenance) && (MenuManager.typeJeu == 2))
                manager.GetComponent<GameManager>().RangerPlis(type);

            if (manager.GetComponent<GameManager>().Pli2Plantes == true)
                manager.GetComponent<GameManager>().RangerPlis(type);
            cptAudio = 0;
            manager.GetComponent<GameManager>().cartesRangees++;
            VousNePasserezPas4 = 0;
            c.transform.SetSiblingIndex((manager.GetComponent<GameManager>().cartesRangees - 1));

            if (tour == 1)
                StartCoroutine(RangerCartes(paire, 2));

            else if (tour == 2)  //on vient de ranger la carte combinaison, on s'arrete là (sauf si on est Pli 3 choix!)
            {

                if (manager.GetComponent<GameManager>().Pli3Choix == true)
                {
                    
                    StartCoroutine(RangerCartes(manager.GetComponent<GameManager>().gameCard3choix[1], 3));
                }

                else
                {
                    if (tourPioche == false)
                    {
                        tourPioche = true;
                        StartCoroutine(TourPioche());
                    }
                    else
                    {
                        if (J1_humain_plays)
                        {
                            J1_humain.CompterPoints();
                            if ((J1_humain.nbrePoints > 0) && ((pointsJ1 != J1_humain.nbrePoints)))
                            {
                                StartCoroutine(PauseJeu(1));
                            }

                            else if ((manager.GetComponent<GameManager>().jeuHumain.Count == 0) && (manager.GetComponent<GameManager>().jeuIA.Count == 0))
                            {
                                FinMancheMatchNul(1);
                            }

                            else
                            {
                                J1_humain_plays = false;
                                StartCoroutine(TourIA());
                            }

                        }
                        else
                        {
                            J2_IA.CompterPoints();
                            if ((J2_IA.nbrePoints > 0) && ((pointsJ2 != J2_IA.nbrePoints)))
                            {
                                StartCoroutine(PauseJeu(2));
                            }

                            else if ((manager.GetComponent<GameManager>().jeuHumain.Count == 0) && (manager.GetComponent<GameManager>().jeuIA.Count == 0))
                            {
                                FinMancheMatchNul(2);
                            }
                            else
                            {
                                J1_humain_plays = true;
                                if (MenuManager.typeJeu == 1)
                                    manager.GetComponent<GameManager>().DesactiverOuActiverBoutons(false);
                                else
                                    TourJoueurIA();
                            }
                            
                        }
                    }
                }
            }
            else if (tour == 3)
            {           //seulement en Pli 3 choix: on a rangé la 3e carte, il faut ranger la 4e
                
                StartCoroutine(RangerCartes(manager.GetComponent<GameManager>().gameCard3choix[2], 4));
            }
            else if (tour == 4)
            {        //seulement en Pli 3 choix: on a rangé la 4e carte, on s'arrete là
                if (tourPioche == false)
                {
                    tourPioche = true;
                    StartCoroutine(TourPioche());
                }
                else
                {
                    if (J1_humain_plays)
                    {
                        J1_humain.CompterPoints();
                        if ((J1_humain.nbrePoints > 0) && ((pointsJ1 != J1_humain.nbrePoints)))
                        {
                            StartCoroutine(PauseJeu(1));
                        }

                        else if ((manager.GetComponent<GameManager>().jeuHumain.Count == 0) && (manager.GetComponent<GameManager>().jeuIA.Count == 0))
                        {
                            FinMancheMatchNul(1);
                        }

                        else
                        {
                            J1_humain_plays = false;
                            StartCoroutine(TourIA());
                        }
                    }
                    else
                    {
                        J2_IA.CompterPoints();
                        if ((J2_IA.nbrePoints > 0) && ((pointsJ2 != J2_IA.nbrePoints)))
                        {
                            StartCoroutine(PauseJeu(2));
                        }

                        else if ((manager.GetComponent<GameManager>().jeuHumain.Count == 0) && (manager.GetComponent<GameManager>().jeuIA.Count == 0))
                        {
                            FinMancheMatchNul(2);
                        }
                        else
                        {
                            J1_humain_plays = true;
                            if (MenuManager.typeJeu == 1)
                                manager.GetComponent<GameManager>().DesactiverOuActiverBoutons(false);
                            else
                                TourJoueurIA();
                        }
                        
                    }
                }

            }
            
        }

        
        
        
    }


    public void TourJoueurIA()
    {
        J1_humain = GameManager.J1_humain;
        J2_IA = GameManager.J2_IA;

        carteJoueeJoueur = manager.GetComponent<GameManager>().IACarteToGameCard();
        
        StartCoroutine(TourJoueur(carteJoueeJoueur));
    }




    public void CardisClicked() {           //Lancement du jeu : le joueur clique sur une de ses cartes
        J1_humain = GameManager.J1_humain;
        J2_IA = GameManager.J2_IA;
        
        carteJoueeJoueur = this;
        manager.GetComponent<GameManager>().DesactiverOuActiverBoutons(true);
        StartCoroutine(TourJoueur(carteJoueeJoueur));
    }
    


    
    public void Choix2Cards()
    {
        paire2choix = this;
        GameManager.choixPaireClick = false;
    }
    
   


    public void CopyInformations(Carte jeu, int valeur, bool affichage, bool boutonActive) {     // Initialisation du jeu (graphique)

        manager = GameObject.FindGameObjectWithTag("Manager");
        this.Proprietes = jeu;
        cardValue = valeur;
        cardBack = manager.GetComponent<GameManager>().getCardBack();       //On donne à chaque GameCard sur le plateau un sprite de derriere et un sprite de devant
        cardFace = manager.GetComponent<GameManager>().getCardFace(cardValue);
        GetComponent<Button>().enabled = false;
        if (affichage == true) {                                           //On affiche que le jeu du joueur et la défosse (mais toutes les cartes sont initalisés avec leur sprite)
            if (boutonActive == true)
            {
                if(MenuManager.typeJeu == 1)
                    GetComponent<Button>().enabled = true;
            }
            
            GetComponent<SpriteRenderer>().sprite = cardFace;
            GetComponent<Image>().sprite = cardFace;
            
        }
    }







    private Vector3 PasdePaires(GameCard c) {                              //Cas où il n'y a pas de combinaison possible (que ce soit tour Joueur ou tour Pioche)

        int defosseCount = manager.GetComponent<GameManager>().defosse.Count;

        Vector3 tmp = new Vector3(0,0,0);    //inutile
        

        for (int i = 0; i < 12; i++) {  //on parcoure toutes les defosseGameCard[] (tableau tjrs avec 12 cases, là où y a des emplacements vides sur le plateau les GameCard sont null)

            if ((defosseCount) <= 8) {  //Pour une question d'affichage, on essaye le plus possible de poser les cartes dans les 8 premiers emplacements
                
                if (manager.GetComponent<GameManager>().coordDefosse[i] == Vector3.zero) {       //si dans les coordonnées des cases de la défosse, y en a une qui est nulle (donc emplacement libre)

                    manager.GetComponent<GameManager>().coordDefosse[i] = manager.GetComponent<GameManager>().coordInitialesDefosse[i];    //coordDefosse[i] est maintenant remplie avec les coordonnées de sa case (qui va etre remplie par le sprite juste après) 
                    manager.GetComponent<GameManager>().defosseGameCard[i] = c;                                                            //DefosseGameCard[i] n'est plus nulle, on vient de la remplir avec une carte (donc emplacement n'est plus libre)
                    return manager.GetComponent<GameManager>().coordDefosse[i];                                                             //on retourne les coordonnées de la case pour pouvoir y déplacer le sprite
                }
            }

            else if ((defosseCount > 8) && (defosseCount <= 10)) {          // idem qu'au dessus (quand les 8 sont remplies, on remplit les 2 suivantes)
                if (manager.GetComponent<GameManager>().coordDefosse[i] == Vector3.zero)
                {
                    manager.GetComponent<GameManager>().coordDefosse[i] = manager.GetComponent<GameManager>().coordInitialesDefosse[i];
                    manager.GetComponent<GameManager>().defosseGameCard[i] = c;
                    return manager.GetComponent<GameManager>().coordDefosse[i];
                }
            }

            else if ((defosseCount > 10) && (defosseCount <= 12)) {        // idem qu'au dessus (quand les 10 sont remplies, on remplit les 2 suivantes -> on essaye le plus possible de ne pas remplir 11 et 12)
                if (manager.GetComponent<GameManager>().coordDefosse[i] == Vector3.zero)
                {
                    manager.GetComponent<GameManager>().coordDefosse[i] = manager.GetComponent<GameManager>().coordInitialesDefosse[i];
                    manager.GetComponent<GameManager>().defosseGameCard[i] = c;
                    return manager.GetComponent<GameManager>().coordDefosse[i];
                }
            }
        }

        return tmp;     //inutile, juste là pour retourner un truc car sinon compilateur rale

    }




    public int CardValue    //ceci ne sert pas à grand chose je pense?
    {
        get { return cardValue; }
        set { cardValue = value; }
    }





   


    public void FinMancheMatchNul(int joueur)
    {
        bool x2continuer = false;


        if ((J1_humain.nbrePoints == 0) && (J2_IA.nbrePoints == 0))
        {
            if(Joueur.dealer == 1)
                 J1_humain.FinManche(J1_humain.scoreTotal, J2_IA.scoreTotal, x2continuer, 1, true, false, false, false);
            else if (Joueur.dealer == 2)
                J2_IA.FinManche(J1_humain.scoreTotal, J2_IA.scoreTotal, x2continuer, 2, true, false, false, false);
        }

        else if (J1_humain.nbrePoints == J2_IA.nbrePoints)
        {
            if (Joueur.dealer == 1)
                J1_humain.FinManche(J1_humain.scoreTotal, J2_IA.scoreTotal, x2continuer, 1, false, true, false, false);
            else if (Joueur.dealer == 2)
                J2_IA.FinManche(J1_humain.scoreTotal, J2_IA.scoreTotal, x2continuer, 2, false, true, false, false);
        }

        else if (J1_humain.nbrePoints > J2_IA.nbrePoints)
        {
            if (Joueur.continuerJ2 == true)
                x2continuer = true;
            else
                x2continuer = false;


            J1_humain.FinManche(J1_humain.scoreTotal, J2_IA.scoreTotal, x2continuer, 1, false, false, false, false);
        }

        else if (J1_humain.nbrePoints < J2_IA.nbrePoints)
        {
            if (Joueur.continuerJ1 == true)
                x2continuer = true;
            else
                x2continuer = false;


            J2_IA.FinManche(J1_humain.scoreTotal, J2_IA.scoreTotal, x2continuer, 2, false, false, false, false);
        }

    }




    public IEnumerator PauseJeu(int joueur)     //pour que le jeu fasse une pause
    {

        bool x2continuer = false;


        if (joueur == 1)
        {
            if (manager.GetComponent<GameManager>().jeuHumain.Count != 0)
            {
                J1_humain.ContinuerOuNon(joueur, false);
                if(MenuManager.typeJeu == 1)
                    yield return new WaitWhile(() => Joueur.pauseJeu);
            }
            pointsJ1 = J1_humain.nbrePoints;
            if ((Joueur.continuerJ1 == false) || (manager.GetComponent<GameManager>().jeuHumain.Count == 0))
            {

                if (Joueur.continuerJ2 == true)
                    x2continuer = true;
                else
                    x2continuer = false;


                J1_humain.FinManche(J1_humain.scoreTotal, J2_IA.scoreTotal, x2continuer, 1, false, false, false, false);
                
            }

            else
            {
                J1_humain_plays = false;
                if(MenuManager.typeJeu == 2)
                    yield return new WaitForSeconds(5f);

                StartCoroutine(TourIA());
            }
        }

        else if (joueur == 2)
        {
            if (manager.GetComponent<GameManager>().jeuIA.Count != 0)
            {
                J2_IA.ContinuerOuNon(joueur, false);
            }
            pointsJ2 = J2_IA.nbrePoints;
            if ((Joueur.continuerJ2 == false) || (manager.GetComponent<GameManager>().jeuIA.Count == 0))
            {

                if (Joueur.continuerJ1 == true)
                    x2continuer = true;
                else
                    x2continuer = false;


                J2_IA.FinManche(J1_humain.scoreTotal, J2_IA.scoreTotal, x2continuer, 2, false, false, false, false);
                
            }
            else
            {
                
                yield return new WaitForSeconds(5f);
                J1_humain_plays = true;

                if (MenuManager.typeJeu == 2)
                    TourJoueurIA();
                else
                    manager.GetComponent<GameManager>().DesactiverOuActiverBoutons(false);

            }

        }


    }

}
