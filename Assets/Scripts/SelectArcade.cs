using System.Diagnostics;
using System;
using Unity.Cinemachine;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;


public class SelectArcade : MonoBehaviour
{
    public GameObject launcher; // empty gameobject holding all launcher gameobjects
    public GameObject cinemachine;


    string[] builds ={
        "c:/launcher/builds/0-aidan/FMP.exe",




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

    float textFlashTimer;
    int index = 0;
    bool focus;
    bool gameIsLoading;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = 0;
        loading = false;
        textFlashTimer = 0;
        index = 0;
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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadGame();
            gameIsLoading = true;
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
        filename = builds[index];
        print("loading + " + filename);
        process = Process.Start(filename);

        Invoke("UnfreezeLauncher", 10); // wait 10 seconds after loaded game is started before enabling launcher

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
            GUI.Label(new Rect(100, 100, 250, 250), "Focus= " + focus + "\ngame is loading=" + gameIsLoading);
    }
}
