using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public static class Partie {


    public static System.Random rng;
    public static int scoreTotalJ1;
    public static int scoreTotalJ2;
    public static int numeroPartie;
    public static int dealer;
    // Use this for initialization
    public static bool modeSoutenance = true;
    private static int[][] tabScores;
    public static AudioSource musique;

    public static bool fadeOut = false;

    public static float speed;

    public static void LancementJeu()
    {

        rng = new System.Random();
        musique = GameObject.Find("Musique").GetComponent<AudioSource>();
        
        musique.Play();
        
    }

    public static void ReloadPartie()
    {
        scoreTotalJ1 = 0;
        scoreTotalJ2 = 0;
        numeroPartie = 1;
        dealer = 1;
        speed = 100f;
    }


    public static void NouvellePartie()
    {
        //SceneManager.UnloadSceneAsync("Jeu");
        //SceneManager.GetSceneByName("Jeu").
        Resources.UnloadUnusedAssets();
        SceneManager.LoadSceneAsync("Jeu", LoadSceneMode.Single);
    }

    public static void FinDePartie(int score, int dealer2)
    {
        if(dealer2 == 1)
            scoreTotalJ1 += score;
        else
            scoreTotalJ2 += score;


        //dealer = dealer2;
        if(((modeSoutenance == true)) && (MenuManager.typeJeu == 2)){
            if (dealer == 1)
                dealer = 2;
            else
                dealer = 1;
        }

        else
            dealer = dealer2;
        Debug.Log(scoreTotalJ1);
        Debug.Log(scoreTotalJ2);
        Debug.Log(dealer);
        Debug.Log(numeroPartie);
        Debug.Log(SystemInfo.processorCount);
        
        
        //WriteCsv();
        numeroPartie++;
        NouvellePartie();
    }

    private static void WriteCsv()
    {

        string first = scoreTotalJ1.ToString();
        string second = scoreTotalJ2.ToString();
        var newLine = string.Format("{0},{1}", first, second, Environment.NewLine);

        using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\Public\TestFolder\WriteLines2.csv", true))
        {
            file.Write(first + ",");
            file.Write(second + ",");
            //file.WriteLine();
            //file.Write(",");
            //file.Write(second);
        }


        /*
        var csv = new StringBuilder();
        string filePath = @"C:\test.csv";

        string first = scoreTotalJ1.ToString();
        string second = scoreTotalJ2.ToString();
        var newLine = string.Format("{0},{1}", first, second, Environment.NewLine);
        csv.Append(newLine);


        File.WriteAllText(filePath, csv.ToString());
        */

    }


    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        fadeOut = false;
        Time.timeScale = 1f;
        Debug.Log("patate");
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            Debug.Log(audioSource.volume);
            yield return null;
        }

        Debug.Log("patate3");
        audioSource.Stop();
        audioSource.volume = startVolume;
        

        
    }

}
