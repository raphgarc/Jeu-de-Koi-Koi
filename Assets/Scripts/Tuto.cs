using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuto : MonoBehaviour {

    private static GameObject PanelTutoGlobal;
    private static GameObject PanelTuto1;
    private static GameObject PanelTuto2;
    private static GameObject PanelTuto3;
    private static GameObject PanelTuto4;
    private static GameObject PanelTuto5;
    private static GameObject PanelTuto6;
    private static GameObject PanelTuto7;
    private static GameObject PanelTuto8;
    private static GameObject PanelTuto9;
    private static GameObject PanelTuto10;
    private static GameObject PanelTuto11;
    private static GameObject PanelTuto12;

    public static bool pauseTuto;
    private static int cptTuto;


    private void Start()
    {
        PanelTutoGlobal = GameObject.FindGameObjectWithTag("PanelTutoGlobal");
        PanelTutoGlobal.GetComponent<RectTransform>().localScale = new Vector3(0, 0);

        PanelTuto1 = GameObject.FindGameObjectWithTag("PanelTuto1");
        PanelTuto2 = GameObject.FindGameObjectWithTag("PanelTuto2");
        PanelTuto3 = GameObject.FindGameObjectWithTag("PanelTuto3");
        PanelTuto4 = GameObject.FindGameObjectWithTag("PanelTuto4");
        PanelTuto5 = GameObject.FindGameObjectWithTag("PanelTuto5");
        PanelTuto6 = GameObject.FindGameObjectWithTag("PanelTuto6");
        PanelTuto7 = GameObject.FindGameObjectWithTag("PanelTuto7");
        PanelTuto8 = GameObject.FindGameObjectWithTag("PanelTuto8");
        PanelTuto9 = GameObject.FindGameObjectWithTag("PanelTuto9");
        PanelTuto10 = GameObject.FindGameObjectWithTag("PanelTuto10");
        PanelTuto11 = GameObject.FindGameObjectWithTag("PanelTuto11");
        PanelTuto12 = GameObject.FindGameObjectWithTag("PanelTuto12");

        pauseTuto = false;
        cptTuto = 1;
    }



    public static IEnumerator OpenTuto()
    {
        Time.timeScale = 1f;
        Vector3 patate = new Vector3(1, 1, 1);
        iTween.ScaleTo(PanelTutoGlobal.gameObject, patate, 2f);
        yield return new WaitForSeconds(2f);
        if (Joueur.TutoPause)
            Time.timeScale = 0f;
        pauseTuto = false;
    }


    public void NextTuto()
    {

        Vector3 patate = new Vector3(0, 0, 0);

        switch (cptTuto)
        {
            case 1:
                if(Joueur.TutoPause)
                    PanelTuto1.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                else
                    iTween.ScaleTo(PanelTuto1.gameObject, patate, 1f);
                cptTuto++;
                break;

            case 2:

                if (Joueur.TutoPause)
                    PanelTuto2.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                else
                    iTween.ScaleTo(PanelTuto2.gameObject, patate, 1f);
                cptTuto++;
                break;

            case 3:
                if (Joueur.TutoPause)
                    PanelTuto3.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                else
                    iTween.ScaleTo(PanelTuto3.gameObject, patate, 1f);
                cptTuto++;
                break;

            case 4:
                if (Joueur.TutoPause)
                    PanelTuto4.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                else
                    iTween.ScaleTo(PanelTuto4.gameObject, patate, 1f);
                cptTuto++;
                break;

            case 5:
                if (Joueur.TutoPause)
                    PanelTuto5.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                else
                    iTween.ScaleTo(PanelTuto5.gameObject, patate, 1f);
                cptTuto++;
                break;

            case 6:
                if (Joueur.TutoPause)
                    PanelTuto6.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                else
                    iTween.ScaleTo(PanelTuto6.gameObject, patate, 1f);
                cptTuto++;
                break;

            case 7:
                if (Joueur.TutoPause)
                    PanelTuto7.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                else
                    iTween.ScaleTo(PanelTuto7.gameObject, patate, 1f);
                cptTuto++;
                break;

            case 8:
                if (Joueur.TutoPause)
                    PanelTuto8.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                else
                    iTween.ScaleTo(PanelTuto8.gameObject, patate, 1f);
                cptTuto++;
                break;

            case 9:
                if (Joueur.TutoPause)
                    PanelTuto9.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                else
                    iTween.ScaleTo(PanelTuto9.gameObject, patate, 1f);
                cptTuto++;
                break;

            case 10:
                if (Joueur.TutoPause)
                    PanelTuto10.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                else
                    iTween.ScaleTo(PanelTuto10.gameObject, patate, 1f);
                cptTuto++;
                break;

            case 11:
                if (Joueur.TutoPause)
                    PanelTuto11.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                else
                    iTween.ScaleTo(PanelTuto11.gameObject, patate, 1f);
                cptTuto++;
                break;

            case 12:
                if(Joueur.TutoPause)
                    PanelTutoGlobal.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                else
                    iTween.ScaleTo(PanelTutoGlobal.gameObject, patate, 0.5f);
                
                PanelTuto1.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                PanelTuto2.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                PanelTuto3.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                PanelTuto4.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                PanelTuto5.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                PanelTuto6.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                PanelTuto7.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                PanelTuto8.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                PanelTuto9.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                PanelTuto10.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                PanelTuto11.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                PanelTuto12.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                cptTuto = 1;
                break;
        }
    }

}
