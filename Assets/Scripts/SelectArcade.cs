using System.Diagnostics;
using System;
using Unity.Cinemachine;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UI;


public class SelectArcade : MonoBehaviour
{
    public GameObject launcher; // empty gameobject holding all launcher gameobjects
    public GameObject cinemachine;


    string root = "c:/launcher/builds/";

    string[] builds ={
        "0/FMP.exe", //aidan
        "1/NetcodeForGameobjects-using-relays.exe",  //alfie
        "2/Gelbound.exe", //cameron
        "3/Y2FMPDemo.exe", //cody
        "4/GravityShifter.exe", //jayden
        "5/BloodMoon.exe", //logan
        "6/Assassins Order project.exe",
        "7/Red Revolution.exe", //matthew
        "8/Download.exe", //oakleigh
        "9/FMPyear2.exe", //sam
        "10/Final FMP.exe", //tobi
        "11/FMP year2.exe", //tyler carrick
        "12/Sonic Blue Rush.exe", //charlie h
        "13/UK Firefighting Sim FMP.exe", //charlie w
        "14/Cold Storage.exe", //connor
        "15/TTT (Tickets To Tomorrow).exe", //finley
        "16/Monster Dungeon.exe", //harvey
        "17/WIP(RIP).exe", //jayden
        "18/FMP.exe", //leah
        "19/ClassProject.exe", //luca
        "20/the gates of peterborough.exe", //nate
        "21/Boom.exe", //taylor
        "22/Unit 8 Assignment.exe", //tommy
        "23/Colossal's Grove.exe", //tyler
        "24/FMP.exe", //will
        "25/Flimsy Kart (FMP).exe", //william




    };




    public Transform[] targets;
    public GameObject[] cabinets;
    

    int[] enabled0 = { 0, 1, 2 };
    int[] enabled1 = { 1, 2, 3 };

    int target = 0;
    int lastTarget;
    int nextTarget;

    string filename;
    bool loading;
    bool enableLoadingText;
    private Process process;
    public TextMeshProUGUI loadingText;

    bool focus;
    bool gameIsLoading;

    public TMP_Text selectText;
    public GameObject leftArrow, rightArrow;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = 0;
        loading = false;
        enableLoadingText = false;
        gameIsLoading = false;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayLoading();

        SelectGame();


        cinemachine.GetComponent<CinemachineCamera>().Follow = targets[target];


        //Optimise(); //only enable cabinets that are visible
        DisplayLoading();





    }


    private void SelectGame()
    {
        if( gameIsLoading==true)
        {
            return;
        }

        FadeSelectText();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            gameIsLoading = true;

            LoadGame();
            return;

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            target--;
            if (target < 0)
            {
                target = targets.Length - 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            target++;
            if (target >= targets.Length)
            {
                target = 0;
            }
        }

        nextTarget = target + 1;
        lastTarget = target - 1;

        if (nextTarget >= targets.Length)
        {
            nextTarget = 0;
        }
        if (lastTarget < 0)
        {
            lastTarget = targets.Length - 1;
        }



    }


    void Optimise()
    {
        foreach( GameObject t in cabinets)
        {
            t.gameObject.SetActive(false);
        }


        cabinets[target].gameObject.SetActive(true);
        cabinets[nextTarget].gameObject.SetActive(true);
        cabinets[lastTarget].gameObject.SetActive(true);
        

    }

    void LoadGame()
    {
        StartCoroutine("DoLoad");   //start flashing load text in background
        enableLoadingText = true;

        Application.runInBackground = true;
        filename = root + builds[target];
        print("loading " + filename);
        process = Process.Start(filename);

        Invoke("UnFreezeLauncher", 30); // wait a few seconds after loaded game is started before enabling the launcher again

    }

    void UnFreezeLauncher()
    {
        loading = false;
        gameIsLoading = false;
        enableLoadingText = false;
    }


    void OnApplicationFocus(bool hasFocus)
    {
        print("focus=" + hasFocus);
        focus = hasFocus;
        launcher.SetActive(hasFocus);   //
        gameIsLoading = false;
    }


    IEnumerator DoLoad()
    {
        enableLoadingText = true;

        for (int i = 0; i < 9; i++)
        {
            loading = loading ? false : true;
            yield return new WaitForSeconds(0.4f);
        }
        enableLoadingText = false;


        yield return null;

    }

    void DisplayLoading()
    {
        if (loading==true && enableLoadingText==true)
        {
            loadingText.text = "Loading...";
        }
        else
        {
            loadingText.text = " ";
        }
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(100, 100, 250, 250), "Focus= " + focus + "\ngame is loading=" + gameIsLoading);
    }


    float fade;
    int fadeDir = 1;
    void FadeSelectText()
    {
        Color c = new Color( 255, fade, 0 , 1);
        selectText.color = c;

        fade += Time.deltaTime*fadeDir*2;
        if(  fade > 1f && fadeDir == 1 )
        {
            fadeDir = -1;
        }
        if (fade < 0.4f && fadeDir == -1)
        {
            fadeDir = 1;
        }
        //apply this fade to the arrows
        leftArrow.GetComponent<RawImage>().color = new Color(0.2f, 1, 0, fade);
        rightArrow.GetComponent<RawImage>().color = new Color(0.2f, 1, 0, fade);

    }
}
