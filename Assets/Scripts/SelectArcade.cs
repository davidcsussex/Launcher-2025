using System.Diagnostics;
using Unity.Cinemachine;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;


public class SelectArcade : MonoBehaviour
{
    public GameObject launcher; // empty gameobject holding all launcher gameobjects
    public GameObject cinemachine;


    string root = "c:/launcher/builds/";

    string[] builds ={
        "0/fmp year 1.exe", 
        "1/temporal tower.exe",  
        "2/fmp project.exe", 
        "3/unit 8 fmp.exe", 
        "4/dirtywork_fmp.exe", 
        "5/unit8_fmp.exe", 
        "6/fmp.exe",
        "7/fmp - mr snatcher.exe", 
        "8/unit 8 fmp.exe", 
        "9/guardian of a false paradise.exe", 
        "10/stygian salvo.exe", 
        "11/unit 8 fmp project.exe", 
        "12/fmp year 1.exe", 
        "13/unit 8 game.exe", 
        "14/my fmp.exe", 
        "15/TTT (Tickets To Tomorrow).exe",
        "16/Monster Dungeon.exe", 
        "17/WIP(RIP).exe", 
        "18/FMP.exe", 
        "19/ClassProject.exe", 
        "20/the gates of peterborough.exe", 
        "21/Boom.exe", 
        "22/Unit 8 Assignment.exe", 
        "23/Colossal's Grove.exe", 
        "24/FMP.exe", 
        "25/Flimsy Kart (FMP).exe", //jayden




    };




    public GameObject[] cabinets;
    string[] cabinetNames = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29" };

    Transform[]targets;

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

        targets = new Transform[24];

        GameObject obj, lookObj;
        Transform pos;
        int targetCount = 0;
        //set up target points
        foreach (string cabinetName in cabinetNames )
        {
            obj = GameObject.Find( cabinetName );
            if (obj != null)
            {
                print("found cabinet " + cabinetName);


                //get child look point
                pos = obj.transform.GetChild(2);    // get lookpoint which is third child
                if( pos == null )
                {
                    print("not found");
                }
                else
                {

                    print("found target at ");
                    targets[targetCount] = pos;
                    targetCount++;
                }
            }

        }


        print("1 found " + targets.Length);
        print("2 found " + targetCount);
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
