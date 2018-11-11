using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    private static GameObject PanelMenu;
    private static GameObject PanelJouer;
    private static GameObject PanelNiveau;
    private static Text textNiveau;
    public static int typeJeu;
    public static int niveauIAnormal = 0;
    public static int niveauIA1 = 0;
    public static int niveauIA2 = 0;

    private static int passage = 0;



    private void Start()
    {
        passage = 0;
        Time.timeScale = 1f;
        Screen.SetResolution(1190, 700, false, 60);
        PanelMenu = GameObject.FindGameObjectWithTag("PanelMenu");
        PanelJouer = GameObject.FindGameObjectWithTag("PanelJouer");
        PanelNiveau = GameObject.FindGameObjectWithTag("PanelNiveau");
        textNiveau = GameObject.Find("TextNiveau").GetComponent<Text>();
        PanelJouer.GetComponent<RectTransform>().localScale = new Vector3(0, 0);
        PanelNiveau.GetComponent<RectTransform>().localScale = new Vector3(0, 0);
        if (Partie.fadeOut)
            StartCoroutine(Partie.FadeOut(Partie.musique, 2f));
    }

    public void LancerRegles()
    {
        StartCoroutine(Tuto.OpenTuto());
    }

    public void Quitter()
    {
        Application.Quit();
    }

    public void ChoisirTypeJeu()
    {
        PanelMenu.GetComponent<RectTransform>().localScale = new Vector3(0, 0);
        Vector3 patate = new Vector3(1, 1, 1);
        iTween.ScaleTo(PanelJouer.gameObject, patate, 2f);
        //PanelJouer.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    public void LancerJvsIA()
    {
        Vector3 patate = new Vector3(0, 0, 0);
        iTween.ScaleTo(PanelJouer.gameObject, patate, 1f);
        typeJeu = 1;
        AfficherPanelNiveau();
    }

    public void LancerIAvsIA()
    {
        Vector3 patate = new Vector3(0, 0, 0);
        iTween.ScaleTo(PanelJouer.gameObject, patate, 1f);
        typeJeu = 2;
        AfficherPanelNiveau();
    }

    private void AfficherPanelNiveau()
    {
        if (passage == 0)
            passage = 1;

        textNiveau.text = "";

        if (typeJeu == 1)
            textNiveau.text = "Veuillez choisir le niveau de l'IA:";
        else
        {
            if (passage == 1)
                textNiveau.text = "Veuillez choisir le niveau de la première IA:";
            else
                textNiveau.text = "Veuillez choisir le niveau de la deuxième IA:";
        }
        Vector3 patate2 = new Vector3(1, 1, 1);
        iTween.ScaleTo(PanelNiveau.gameObject, patate2, 1f);
        
    }


    public void ChoixNiveau(int choix)
    {
        switch(choix)
        {
            case 1:
                if (typeJeu == 1)
                    niveauIAnormal = 1;
                else
                {
                    if (passage == 1)
                        niveauIA1 = 1;
                    else
                        niveauIA2 = 1;
                }
                break;

            case 2:
                if (typeJeu == 1)
                    niveauIAnormal = 2;
                else
                {
                    if (passage == 1)
                        niveauIA1 = 2;
                    else
                        niveauIA2 = 2;
                }
                break;

            case 3:
                if (typeJeu == 1)
                    niveauIAnormal = 3;
                else
                {
                    if (passage == 1)
                        niveauIA1 = 3;
                    else
                        niveauIA2 = 3;
                }
                break;

            case 4:
                if (typeJeu == 1)
                    niveauIAnormal = 4;
                else
                {
                    if (passage == 1)
                        niveauIA1 = 4;
                    else
                        niveauIA2 = 4;
                }
                break;
        }


        if ((typeJeu == 1) || (passage == 2))
        {
            Vector3 patate2 = new Vector3(0, 0, 0);
            iTween.ScaleTo(PanelNiveau.gameObject, patate2, 0.5f);
            Debug.Log(niveauIAnormal);
            Debug.Log(niveauIA1);
            Debug.Log(niveauIA2);
            StartCoroutine(LancerJeu());
        }

        else if (passage == 1)
        {
            passage = 2;
            AfficherPanelNiveau();
        }


        
            
    }



    IEnumerator LancerJeu()
    {
        yield return new WaitForSeconds(0.3f);
        

        if (SceneManager.GetSceneByName("Jeu").IsValid())
        {
            Debug.Log("GAME UNLOADED");
            SceneManager.UnloadSceneAsync("Jeu");
        }

        Partie.ReloadPartie();
        Resources.UnloadUnusedAssets();
        Scene tmp = SceneManager.GetSceneByName("Jeu");
        SceneManager.LoadSceneAsync("Jeu");
    }
}
