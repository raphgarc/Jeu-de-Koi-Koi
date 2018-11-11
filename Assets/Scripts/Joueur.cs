using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Joueur : MonoBehaviour {

    public int nbrePoints;
    public int nbrePointsAnciens;
    public int nbrePlis;
    public int scoreTotal = 0;
    public int[] PointsParPlis;
    public int[] PointsParPlisAutres;

    public List<Carte> PlisLumiere;
    public List<Carte> PlisRuban;
    public List<Carte> PlisPlante;
    public List<Carte> PlisAnimal;
    public List<int> Points_par_combinaison;

    public List<Carte> jeu;

    bool cerisier;
    bool lune;
    static bool rainMan;

    public Text descriptionText;
    public Text pointsText;
    public Text pointsTotalText;
    public Text continuerText;
    public Text decisionIA;
    private Button OuiButton;
    private Button NonButton;

    public static bool pauseJeu = false;
    public static bool pauseJeu2 = false;
    public static bool pauseJeu3 = false;
    public static GameObject PanelContinuer;
    public static GameObject PanelPetitContinuer;
    public static GameObject PanelBoutonsContinuer;

    public static GameObject PanelPause;
    public static GameObject PetitPanelPause;
    public static Text scoreJ1;
    public static Text scoreJ2;

    public static GameObject PanelDebutPartie;
    public static Text debutPartieText;

    public static GameObject PanelFin;
    public static Text textFin;

    public static GameObject PanelGraphe;
    public static Image IndicateurJ1;
    public static Image IndicateurJ2;
    public static Text TextPointsJ1;
    public static Text TextPointsJ2;
    public static Text TextPointsMax;
    public static Text TextNiveauIA1;
    public static Text TextNiveauIA2;
    public static Image Point0;
    public static Image PointMax;

    public static bool continuerJ1;
    public static bool continuerJ2;

    public static int dealer;
    public static bool pauseJeuDebut;

    public static bool TutoPause = false;


    void Start () {
        PlisLumiere = new List<Carte>();
        PlisRuban = new List<Carte>();
        PlisPlante = new List<Carte>();
        PlisAnimal = new List<Carte>();

        PointsParPlis = new int[4];                 //0 pour Lumiere, 1 pour Ruban, 2 pour Animal, 3 pour Plante
        PointsParPlisAutres = new int[5];           //0 pour lune + sake, 1 pour cerisier + sake, 2 pour inoshikacho, 3 pour rubans poemes, 4 pour rubans bleus

        PanelContinuer = GameObject.FindGameObjectWithTag("PanelContinuer");
        //PanelContinuer.GetComponent<RectTransform>().localScale = new Vector3(0, 0);
        PanelPetitContinuer = GameObject.FindGameObjectWithTag("PetitPane");
        PanelBoutonsContinuer = GameObject.FindGameObjectWithTag("PanelBoutonsContinuer");
        descriptionText = GameObject.Find("Description").GetComponent<Text>();
        pointsText = GameObject.Find("Points").GetComponent<Text>();
        pointsTotalText = GameObject.Find("Total").GetComponent<Text>();
        continuerText = GameObject.Find("ContinuerText").GetComponent<Text>();
        decisionIA = GameObject.Find("DecisionIA").GetComponent<Text>();

        PanelDebutPartie = GameObject.FindGameObjectWithTag("PanelDebut");
        PanelDebutPartie.GetComponent<RectTransform>().localScale = new Vector3(0, 0);
        debutPartieText = GameObject.Find("TextDebut").GetComponent<Text>();

        PanelFin = GameObject.FindGameObjectWithTag("PanelFin");
        PanelFin.GetComponent<RectTransform>().localScale = new Vector3(0, 0);
        textFin = GameObject.Find("TextFin").GetComponent<Text>();


        PanelPause = GameObject.FindGameObjectWithTag("PanelPause");
        PanelPause.GetComponent<RectTransform>().localScale = new Vector3(0, 0);
        PetitPanelPause = GameObject.FindGameObjectWithTag("PetitPanelPause");
        //PetitPanelPause.GetComponent<RectTransform>().localScale = new Vector3(0, 0);
        scoreJ1 = GameObject.Find("ScoreJ1").GetComponent<Text>();
        scoreJ2 = GameObject.Find("ScoreJ2").GetComponent<Text>();
        //Time.timeScale = 5f;

        PanelGraphe = GameObject.FindGameObjectWithTag("PanelGraphe");
        if(!((Partie.modeSoutenance == true) && (MenuManager.typeJeu == 2)))
            PanelGraphe.GetComponent<RectTransform>().localScale = new Vector3(0, 0);
        IndicateurJ1 = GameObject.Find("PointJ1").GetComponent<Image>();
        IndicateurJ2 = GameObject.Find("PointJ2").GetComponent<Image>();
        Point0 = GameObject.Find("Point0").GetComponent<Image>();
        PointMax = GameObject.Find("PointMax").GetComponent<Image>();
        TextPointsJ1 = GameObject.Find("TextJ1").GetComponent<Text>();
        TextPointsJ2 = GameObject.Find("TextJ2").GetComponent<Text>();
        TextPointsMax = GameObject.Find("TextMax").GetComponent<Text>();
        TextNiveauIA1 = GameObject.Find("TextNiveauIA1").GetComponent<Text>();
        TextNiveauIA2 = GameObject.Find("TextNiveauIA2").GetComponent<Text>();


        continuerJ1 = false;
        continuerJ2 = false;
        dealer = Partie.dealer;
        //StartCoroutine(DebutPartie());
    }
	
	
    public static IEnumerator DebutPartie()
    {
        PanelPetitContinuer.GetComponent<RectTransform>().localScale = new Vector3(0, 0);
        PanelDebutPartie.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        PanelContinuer.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
       
        pauseJeuDebut = true;
        debutPartieText.text = "";
        debutPartieText.text = "Partie " + Partie.numeroPartie;
        
        
        Vector3 patate2 = new Vector3(1, 1, 1);
        iTween.ScaleTo(PanelDebutPartie.gameObject, patate2, 2f);



        yield return new WaitForSeconds(3f);
        Vector3 patate3 = new Vector3(0, 0, 0);
        iTween.ScaleTo(PanelDebutPartie.gameObject, patate3, 1f);
        yield return new WaitForSeconds(0.2f);
        PanelContinuer.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.2f);
        pauseJeuDebut = false;

        
            if ((Partie.modeSoutenance == true) && (MenuManager.typeJeu == 2))
                PanelPointsGraphe();
        

    }

    public static IEnumerator PausePanelJeu()
    {
        PetitPanelPause.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        PanelPause.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        
        Debug.Log(PetitPanelPause);
        scoreJ1.text = "";
        scoreJ2.text = "";


        scoreJ1.text = (Partie.scoreTotalJ1).ToString();
        scoreJ2.text = (Partie.scoreTotalJ2).ToString();

        Vector3 patate3 = new Vector3(1, 1, 1);
        iTween.ScaleTo(PetitPanelPause.gameObject, patate3, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;
        pauseJeu2 = true;
        yield return new WaitWhile(() => pauseJeu2);

        if ((Partie.modeSoutenance == true) && (MenuManager.typeJeu == 2))
            Time.timeScale = Partie.speed;

        else
            Time.timeScale = 1f;



        Vector3 patate = new Vector3(0, 0, 0);
        iTween.ScaleTo(PetitPanelPause.gameObject, patate, 1f);
        yield return new WaitForSeconds(0.2f);
        PanelPause.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        //yield return new WaitForSeconds(0.2f);
    }


    public static IEnumerator PanelFinJeu(int scoreJ1, int scoreJ2)
    {
        yield return new WaitForSeconds(10);
        Vector3 patate2 = new Vector3(0, 0, 0);
        iTween.ScaleTo(PanelPetitContinuer.gameObject, patate2, 1f);
        yield return new WaitForSeconds(0.5f);
       
        

        //Debug.Log(PetitPanelPause);
        textFin.text = "";

        if (scoreJ1 > scoreJ2)
            textFin.text = "Bravo, vous avez gagné!";
        else
            textFin.text = "Pas de chance... Peut-être une autre fois?";

        Vector3 patate3 = new Vector3(1, 1, 1);
        iTween.ScaleTo(PanelFin.gameObject, patate3, 0.5f);
        yield return new WaitForSeconds(0.5f);
       // Time.timeScale = 0f;
        pauseJeu3 = true;
        yield return new WaitWhile(() => pauseJeu3);

        
    }

    public static void PanelPointsGraphe()
    {
        int ptsMax;
        Vector3 endPosition1;
        Vector3 endPosition2;
        Vector3 endPosition3;
        Vector3 endPosition4;

        TextNiveauIA1.text = MenuManager.niveauIA1.ToString();
        TextNiveauIA2.text = MenuManager.niveauIA2.ToString();

        //PanelGraphe.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
        if ((Partie.scoreTotalJ1 > 400) || (Partie.scoreTotalJ2 > 400))
        {

            if (Partie.scoreTotalJ1 > Partie.scoreTotalJ2)
            {
                TextPointsMax.text = ((int)(Partie.scoreTotalJ1 * 1.5)).ToString();
                ptsMax = (int)(Partie.scoreTotalJ1 * 1.5);
            }
            else
            {
                TextPointsMax.text = ((int)(Partie.scoreTotalJ2 * 1.5)).ToString();
                ptsMax = (int)(Partie.scoreTotalJ2 * 1.5);
            }
        }
        else
        {
            ptsMax = 500;
            TextPointsMax.text = ptsMax.ToString();
        }

        if(Partie.scoreTotalJ1 != 0)
            TextPointsJ1.text = Partie.scoreTotalJ1.ToString();
        if(Partie.scoreTotalJ2 != 0)
            TextPointsJ2.text = Partie.scoreTotalJ2.ToString();

        double pixMax;
        pixMax = 1.0 / (float)ptsMax;
        endPosition1 = new Vector3(Point0.transform.position.x + Partie.scoreTotalJ1 * ((PointMax.transform.position.x - Point0.transform.position.x) * (float)pixMax), IndicateurJ1.transform.position.y, 0);
        endPosition2 = new Vector3(Point0.transform.position.x + Partie.scoreTotalJ2 * ((PointMax.transform.position.x - Point0.transform.position.x) * (float)pixMax), IndicateurJ2.transform.position.y, 0);
        endPosition3 = new Vector3(endPosition1.x, TextPointsJ1.transform.position.y, 0);
        endPosition4 = new Vector3(endPosition2.x, TextPointsJ2.transform.position.y, 0);
        iTween.MoveTo(IndicateurJ1.gameObject, endPosition1, 1f);
        iTween.MoveTo(IndicateurJ2.gameObject, endPosition2, 1f);
        iTween.MoveTo(TextPointsJ1.gameObject, endPosition3, 1f);
        iTween.MoveTo(TextPointsJ2.gameObject, endPosition4, 1f);
        

    }

    public void CompterPoints()
    {
        Points_par_combinaison = fonctions_combinaisons.nb_points(this);
        PointsParPlis[0] = Points_par_combinaison[(int)Combinaison.Lumiere];
        PointsParPlis[1] = Points_par_combinaison[(int)Combinaison.Ruban];
        PointsParPlis[2] = Points_par_combinaison[(int)Combinaison.Animal];
        PointsParPlis[3] = Points_par_combinaison[(int)Combinaison.Plante]; ;

        PointsParPlisAutres[0] = Points_par_combinaison[(int)Combinaison.Autre_sake_lune]; ;
        PointsParPlisAutres[1] = Points_par_combinaison[(int)Combinaison.Autre_sake_cerisier]; ;
        PointsParPlisAutres[2] = Points_par_combinaison[(int)Combinaison.Autre_inoshikacho]; ;
        PointsParPlisAutres[3] = Points_par_combinaison[(int)Combinaison.Ruban_poeme]; ;
        PointsParPlisAutres[4] = Points_par_combinaison[(int)Combinaison.Ruban_bleu]; ;

        this.nbrePoints = this.PointsParPlis[0] + this.PointsParPlis[1] + this.PointsParPlis[2] + this.PointsParPlis[3] + this.PointsParPlisAutres[0] + this.PointsParPlisAutres[1] + this.PointsParPlisAutres[2] + this.PointsParPlisAutres[3] + this.PointsParPlisAutres[4];
        //Debug.Log("Nbre Points: " + this.nbrePoints);
        //Debug.Log("Plante: " + this.PointsParPlis[3]);
        //Debug.Log("Animal: " + this.PointsParPlis[2]);
        //Debug.Log("Ruban: " + this.PointsParPlis[1]);
        //Debug.Log("Lumiere: " + this.PointsParPlis[0]);

    }

    private bool HaveCoupeSake()
    {
        foreach (Animal c in this.PlisAnimal)
        {
            if (c.NomAnimal == NomAnimal.Sake)
                return true;

        }
        return false;
    }


    public void AjouterPlis(Carte pli)
    {
        if (pli is Lumiere)
        {
            this.PlisLumiere.Add(pli);
        }

        else if (pli is Ruban)
        {
            this.PlisRuban.Add(pli);
        }

        else if (pli is Plante)
        {
            this.PlisPlante.Add(pli);
        }

        else if (pli is Animal)
        {
            this.PlisAnimal.Add(pli);
        }
    }


    public void ContinuerOuNon(int joueur, bool finManche)
    {
        descriptionText.text = "";
        decisionIA.text = "";
        pointsText.text = "";
        PanelContinuer.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        PanelPetitContinuer.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        Vector3 patate2 = new Vector3(1, 1, 1);
        iTween.ScaleTo(PanelPetitContinuer.gameObject, patate2, 1f);

        continuerText.text = "Continuer?";

        if (finManche == false)
        {
            PanelBoutonsContinuer.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            if ((joueur == 1) && (MenuManager.typeJeu == 1))
                pauseJeu = true;

            else if ((MenuManager.typeJeu == 2) && (joueur == 1))
            {
                continuerText.text = "";
                PanelBoutonsContinuer.GetComponent<RectTransform>().localScale = new Vector3(0, 0);
                decisionIA.text = "Le joueur a décidé de continuer.";
                continuerJ1 = IA.continuer();
            }

            else if (joueur == 2)
            {
                continuerText.text = "";
                PanelBoutonsContinuer.GetComponent<RectTransform>().localScale = new Vector3(0, 0);
                decisionIA.text = "Le joueur adverse a décidé de continuer.";
                continuerJ2 = IA.continuer();
            }



        }


        if (PointsParPlis[0] > 0)
        {
            if ((rainMan) && (PlisLumiere.Count == 4))
                descriptionText.text += "Lumieres (Rain Man) \n";

            else
                descriptionText.text += "Lumieres (" + this.PlisLumiere.Count + ") \n";

            pointsText.text += PointsParPlis[0].ToString() + "\n";
        }

        if (PointsParPlis[1] > 0)
        {
            descriptionText.text += "Rubans (" + this.PlisRuban.Count + ") \n";
            pointsText.text += PointsParPlis[1].ToString() + "\n";
        }

        if (PointsParPlis[2] > 0)
        {
            descriptionText.text += "Animaux (" + this.PlisAnimal.Count + ") \n";
            pointsText.text += PointsParPlis[2].ToString() + "\n";
        }

        if (PointsParPlis[3] > 0)
        {
            descriptionText.text += "Plantes (" + this.PlisPlante.Count + ") \n";
            pointsText.text += PointsParPlis[3].ToString() + "\n";
        }

        if (PointsParPlisAutres[0] > 0)
        {
            descriptionText.text += "Lune et Coupe de Saké\n";
            pointsText.text += PointsParPlisAutres[0].ToString() + "\n";
        }

        if (PointsParPlisAutres[1] > 0)
        {
            descriptionText.text += "Cerisier et Coupe de Saké \n";
            pointsText.text += PointsParPlisAutres[1].ToString() + "\n";
        }

        if (PointsParPlisAutres[2] > 0)
        {
            descriptionText.text += "Cerf, Sanglier et Papillon \n";
            pointsText.text += PointsParPlisAutres[2].ToString() + "\n";
        }

        if (PointsParPlisAutres[3] > 0)
        {
            descriptionText.text += "Trois rubans poèmes \n";
            pointsText.text += PointsParPlisAutres[3].ToString() + "\n";
        }

        if (PointsParPlisAutres[4] > 0)
        {
            descriptionText.text += "Trois rubans bleus \n";
            pointsText.text += PointsParPlisAutres[4].ToString() + "\n";
        }

        pointsTotalText.text = nbrePoints.ToString();

        if (finManche == false)
        {
            if (joueur == 2)
            {
                if (continuerJ2 == false)
                    StartCoroutine(Pause(10));

                else
                    StartCoroutine(Pause(5));

            }

            else if ((MenuManager.typeJeu == 2) && (joueur == 1))
            {
                if (continuerJ1 == false)
                    StartCoroutine(Pause(10));

                else
                    StartCoroutine(Pause(5));
            }
        }

    }



    public void FinManche(int scoreJ1, int scoreJ2, bool x2continuer, int joueur, bool matchNulZero, bool matchNulAutre, bool gagnerDebut4Mois, bool gagnerDebutPaires)
    {
        ContinuerOuNon(joueur, true);
        continuerText.text = "";
        PanelBoutonsContinuer.GetComponent<RectTransform>().localScale = new Vector3(0, 0);
        decisionIA.text = "";
        dealer = joueur;

        if (matchNulZero == true)
        {
            descriptionText.text += "Avantage du dealer\n";
            pointsText.text += "6 \n";
            this.nbrePoints = 6;

        }

        if(gagnerDebut4Mois == true)
        {
            descriptionText.text += "4 cartes du même mois dans la main de départ\n";
            pointsText.text += "6 \n";
            this.nbrePoints = 6;
        }

        if (gagnerDebutPaires == true)
        {
            descriptionText.text += "4 paires dans la main de départ\n";
            pointsText.text += "6 \n";
            this.nbrePoints = 6;
        }

        if (nbrePoints >= 7)
        {
            descriptionText.text += "Score supérieur à 7 \n";
            pointsText.text += "x2 \n";
            this.nbrePoints = this.nbrePoints * 2;

        }

        if (x2continuer == true)
        {
            descriptionText.text += "Koi-koi de l'adversaire \n";
            pointsText.text += "x2 \n";
            this.nbrePoints = this.nbrePoints * 2;

        }
        

        pointsTotalText.text = nbrePoints.ToString();


        if (joueur == 1)
        {
            this.scoreTotal = Partie.scoreTotalJ1 + this.nbrePoints;
            scoreJ1 = this.scoreTotal;
            scoreJ2 = Partie.scoreTotalJ2;
        }

        else
        {
            this.scoreTotal = Partie.scoreTotalJ2 + this.nbrePoints;
            scoreJ2 = this.scoreTotal;
            scoreJ1 = Partie.scoreTotalJ1;
        }


        decisionIA.text = "Score du joueur: " + scoreJ1 + "\n";
        decisionIA.text += "Score du joueur adverse: " + scoreJ2 + "\n";


        if ((Partie.numeroPartie == 12))
        {
			if (MenuManager.typeJeu == 1) {
				StartCoroutine (PanelFinJeu (scoreJ1, scoreJ2));
			} 


			else if (MenuManager.typeJeu == 2) {
				if(Partie.modeSoutenance == true)
					StartCoroutine(PauseFin(10, this.nbrePoints, dealer));

				else
					StartCoroutine(PanelFinJeu(scoreJ1, scoreJ2));
			}
           
        }

        else
            StartCoroutine(PauseFin(10, this.nbrePoints, dealer));
        

    }


    public void ReprendrePause()
    {
        if ((Partie.modeSoutenance == true) && (MenuManager.typeJeu == 2))
            Time.timeScale = Partie.speed;
        else
            Time.timeScale = 1f;
        pauseJeu2 = false;
        GameManager.escape = false;
    }

    public void NouvellePartie()
    {
        StartCoroutine(FermerPanelPause(true, false));
        
    }

    public void Regles()
    {
        TutoPause = true;
        StartCoroutine(Tuto.OpenTuto());
    }

    public void Quitter()
    {
        StartCoroutine(FermerPanelPause(false, true));
        
    }


   public  IEnumerator FermerPanelPause(bool newPartie, bool quitter)
    {
        Time.timeScale = 1f;
        TutoPause = false;
        if (pauseJeu2 == true)
        {
            Vector3 patate = new Vector3(0, 0, 0);
            iTween.ScaleTo(PetitPanelPause.gameObject, patate, 0.5f);
            yield return new WaitForSeconds(0.2f);
            Time.timeScale = 0f;
            PanelPause.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        }

        else if (pauseJeu3 == true)
        {
            Vector3 patate2 = new Vector3(0, 0, 0);
            iTween.ScaleTo(PanelFin.gameObject, patate2, 0.5f);
            yield return new WaitForSeconds(0.5f);
            PanelContinuer.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        }
        if (newPartie == true)
        {
            Partie.fadeOut = true;
            SceneManager.UnloadSceneAsync("Jeu");
            SceneManager.LoadScene("Menu");
        }
        else if(quitter == true)
            Application.Quit();
    }

    public void ContinuerOui()
    {
        pauseJeu = false;
        continuerJ1 = true;
        StartCoroutine(FermerPanel(1));

    }

    public void ContinuerNon()
    {
        pauseJeu = false;
        continuerJ1 = false;
        //StartCoroutine(FermerPanel(1));
    }

    public IEnumerator FermerPanel(float waitTime)
    {
        Vector3 patate2 = new Vector3(0, 0, 0);
        iTween.ScaleTo(PanelPetitContinuer.gameObject, patate2, 1f);
        yield return new WaitForSeconds(0.2f);
        PanelContinuer.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.2f);
        pauseJeuDebut = false;

    }


    public IEnumerator Pause(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        StartCoroutine(FermerPanel(1));
    }

    public IEnumerator PauseFin(float waitTime, int score, int dealer)
    {
        
        yield return new WaitForSeconds(waitTime);

        

        Partie.FinDePartie(score, dealer);

    }


    public IEnumerator PauseDebut()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(FermerPanel(1));
    }




    
}




