using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;



public class Task_SliderTarget : MonoBehaviour
{
    public string savePath;
    public string saveFileName;
    bool headerWritten = false;
    public float TimeEstimate = 0f;
    bool TimeRecording = false;
    bool guideIsMoving = false;
    Vector3 startPosition;
    Vector3 targetPosition;
    bool NewTestGo = false;
    bool AllTestDone = false;
    int testNumber = 0;
    // Vector3 moveDistance;
    public GameObject objectToMove;
    public GameObject moveTarget;
    public GameObject targetMarker;
    public GameObject StartMarker;
    int markPos;
    int startPos;
    int resLvl;
    int testRunNumber = 0;
    public float timeToTarget = 3f;
    
    //int testState = 0;
    public List<int> TestCaseParameters = new List<int>();
    public List<List<int>> TestCaseList = new List<List<int>>();
    public List<int> UsedCases = new List<int>();
    public enum testStates {readyForNew, testIsMoving, waitForTimeClick}
    public testStates testState = testStates.readyForNew;

    public List<Vector3> SaveSliderCoords = new List<Vector3>();
    public List<Vector3> SaveGuideCoords = new List<Vector3>();


    /* to define phases of the test. 
     * 0 - nothing is happening
     * 
     * 1 - intialize new test 
     *  generate all valid test cases.
     *  get markPosition (1,2,3,4,5) from test case list
     *  move mark to markPosition
     *  get sliderPosition (1,2,3,4,5) from test case list
     *  move slider to start position
     *  move sliderTarget to start position    
     *      
     *  
     *  
     *  get timeToTarget (1500, 2500, 3500) from test case list
     *  get resistance level (1,2,3) from test case list
     *  wait for hand to enter Slider & trigger pressed
     *  
     *  write to disk:
     *  start position, target position
     *  movement distance
     *  movement direction
     *  is movement away from body or towards?
     *  TimeToTarget
     *  resistance level
     *  
     *  
     *  slider light turns on when hand is covering it
     *  slider light turns off when hand is taken off
     * 
     * 
     *   
     *   
     
    // 2 - new sequence starts      
    //     slider light is on.
    //     sliderTarget starts to move towards mark (MoveOverSeconds)
    //     slider moves by user hand location (button held down)
    //       if hand is removed from the slider, light turns off and movement stops
    //       user goal: move the sliderTarget with your hand at the same phase as the slider moves
    //     end movement when controller button is released
    //     start counting time from that point 
    //       user goal: wait as long as the movement took, then press button
    //     when button pressed, record passed time between end of the press and button click
    //     end -> intialize new scene.*/



    // OUT OF DATE. Check where to do this actually. Good points though, this is exactly what needs to be done if I remember right.

    //here be buffering of all relevant data to arrays
    /*
     tallenna realtime-tracking data, sekä guiden etta handcontrollerin koordinaatit joka 1/100s.
         pistä arrayhin ja sit lopuksi kirjoita ne tiedostoon.

     tallenna käyttäjän tekemän siirron pituus.  (Alku ja loppupiste seuraamisen aikana)
     tallenna käyttäjän tekemän siirron kesto. (start time, end time, erotus)
     tallenna käyttäjän liikeen pituus ajanlaskun aikana (käyttääkö liikettä apuna ajan mittaukseen)
     tallenna kuinka hyvin käyttäjä seuraa ohjainpalikkaa 
         prosenttiosuus kun guidelinet kohtaavat?
     tallenna onko liike pitkä vai lyhyt
     tallenna onko liike poispäin vai keho kohti.
     Eli luenta siitä et joka 1/100 sekunti kirjoitetaan joku ylös. 
     */





    Vector3 markPosition;


    // Start is called before the first frame update
    void Start()
    {
        GenerateTestCases();
        TestInitialize();
        //  targetPosition = moveTarget.transform.position;
        //moveDistance = new Vector3((this.transform.position.x - moveTarget.transform.position.x), 0f, 0f);
        //StartCoroutine(MoveOverSeconds(objectToMove, targetPosition, timeToTarget));


    }

    // Update is called once per frame
    void Update()
    {
       
        /*  // Manual controls to restart tests. Buggy now that there is tracker motion data saving etc. states that get messed up if you use these.
            //  

        if (Input.GetKeyDown(KeyCode.T))
        {
           // GameObject.Find("Task_SliderFollow").GetComponent<TaskController_Slider>().Movable = true;
            //(GameObject.Find("Task_SliderFollow").GetComponent<TaskController_Slider>().Movable == true ){ 
            StartCoroutine(MoveOverSeconds(objectToMove, targetPosition, timeToTarget));
            //Debug.Log("Away!");
        }

       if (Input.GetKeyDown(KeyCode.R)) // tähän se chekki et otetaan vain yksi keypress eikä montaa.
        {
           // StartCoroutine(CheckPerSeconds());
           // Debug.Log("go!");
           TestInitialize();
     
        }
        */

       

       
       if ((OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) || (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) || (Input.GetKeyDown(KeyCode.Space)))
            {
            if ((GameObject.Find("Task_SliderFollow").GetComponent<TaskController_Slider>().Movable == true) && testState == testStates.readyForNew)
                { // STARTS A NEW TEST 
                        StartCoroutine(MoveOverSeconds(objectToMove, targetPosition, timeToTarget));
                        testState = testStates.testIsMoving;
               

                StartCoroutine(CheckPerSeconds());
                TimeEstimate = 0f;
                SaveGuideCoords.Clear();
                SaveSliderCoords.Clear();

            }

            if (testState == testStates.waitForTimeClick)
                { // ENDS the test
                GameObject.Find("Task_SliderFollow").GetComponent<TaskController_Slider>().CountingTime = false;
                TimeRecording = false;
                    SaveFileData();
             
                    TimeEstimate = 0f;
                    TestInitialize();
                    testState = testStates.readyForNew;
                
                }

            if (testState == testStates.testIsMoving){

            }


            }



        if ((OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger)) || (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger)))
        {
            if ((testState == testStates.testIsMoving) && (!guideIsMoving))
            {



                TimeRecording = true;
               
            }


            if ((!guideIsMoving) && (testState != testStates.readyForNew))

            {
                testState = testStates.waitForTimeClick;
                GameObject.Find("Task_SliderFollow").GetComponent<TaskController_Slider>().CountingTime = true;
            }
        }


        if (TimeRecording)
        {
            TimeEstimate += Time.deltaTime;
         


          

        

        }
    }


    





    private void SaveFileData()
    {

        string path1 = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/VRTIMES_Data";
        string path2 = path1 + "/" + saveFileName;


        if (!headerWritten)
        {
            DateTime saveTimeNow = System.DateTime.Now;
            saveTimeNow.ToString("yyyyMMddHHmmss");
            var tmpTestCasesCount = TestCaseList.Count.ToString();
            string headerToWrite = "Starting new test session with " + tmpTestCasesCount + " cases. " +  saveTimeNow + Environment.NewLine +
                                   "Format: case number, starting position, target position,  movement resistance, movement speed, user time estimate" + Environment.NewLine;
            headerWritten = true;
            System.IO.File.AppendAllText(path2, headerToWrite);
        }

        if (!AllTestDone)
        {

            // SAVE INFORMATION ABOUT THIS CASE, PARAMETERS AND SUCH
            var tmpTimeEstimate = TimeEstimate.ToString();
            var tmpMarkPos = markPos.ToString();
            var tmpTargetPos = startPos.ToString();
            var tmpTimeToTarget = timeToTarget.ToString();
            var tmpResLvl = resLvl.ToString();
            var tmpTestRunNumber = testRunNumber.ToString();

            string stateToWrite = Environment.NewLine + tmpTestRunNumber + "," + tmpMarkPos + "," + tmpTargetPos + "," + tmpResLvl + "," + tmpTimeToTarget + "," + tmpTimeEstimate + Environment.NewLine;
            System.IO.File.AppendAllText(path2, stateToWrite);
            //Debug.Log("saved current test session parameters");


            // REAL TIME VALUES GATHERED ARE SAVED HERE.
            // 1. STARTING WITH SLIDER MOVEMENT

            string tmpSaveInfo = "Slider movement coordinates:";
            var tmpSaveThis = tmpSaveInfo + Environment.NewLine;
            System.IO.File.AppendAllText(path2, tmpSaveThis);

            for (int i = 0; i < SaveSliderCoords.Count; i++)
            {
                Vector3 saveThis = SaveSliderCoords[i];    
                tmpSaveThis = saveThis.ToString("F6");
                //Debug.Log("write array down" + tmpSaveThis);
                stateToWrite = tmpSaveThis + Environment.NewLine;
                System.IO.File.AppendAllText(path2, stateToWrite);
            }   //Debug.Log("Saved slider movement coordinates");

            // 2. STARTING WITH HAND CONTROLLER COORDS



        }


        /*
            1: testcasen parametrit

        // */
    }






    public IEnumerator BlinkLight(float time)
        // if I remember right there were issues with this.

    {

        GameObject.Find("Task_SliderFollow").GetComponent<TaskController_Slider>().CountingTime = true;
      
      yield return new WaitForSeconds(time/1000f);
        GameObject.Find("Task_SliderFollow").GetComponent<TaskController_Slider>().CountingTime = false;
       // Debug.Log("slider blinked");

    }



 public IEnumerator CheckPerSeconds()
    //used to record test session data to lists before saving it.
    //done every 0.1seconds.
    //Starts itself again if the test is moving. 
    //Can be only used to record movement during the motion phase "the test is moving"


    {
        Vector3 SliderCoords = GameObject.Find("Slider").transform.position;
        SaveSliderCoords.Insert(0, SliderCoords);
        Vector3 GuideCoords = GameObject.Find("SliderTarget").transform.position;
        SaveGuideCoords.Insert(0, GuideCoords);
       // Debug.Log("UserSlider:"+ SliderCoords.ToString("F4") + "Target: " + GuideCoords.ToString("F4"));
            yield return new WaitForSeconds(0.1f);

        if (testState == testStates.testIsMoving) { StartCoroutine(CheckPerSeconds()); }  
        
    }





    public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        guideIsMoving = true;
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
        guideIsMoving = false;

    }



 

    void RollNewTestNumber()
        // this is used to choose a random test from the list of all possible tests.
        // every case will be used only once.

    {

        bool NewTestNumberFailed = false;

        testNumber = UnityEngine.Random.Range(0, TestCaseList.Count);
        for (int i = 0; i < UsedCases.Count; i++)
            {
                if (testNumber == UsedCases[i]) {
                    NewTestNumberFailed = true;
                }
            }

        if (!NewTestNumberFailed) {
            UsedCases.Insert(0, testNumber);
            NewTestGo = true;
        }

        if (UsedCases.Count == TestCaseList.Count) // kaikki tapaukset on käyty läpi.
        { NewTestGo = true;
            AllTestDone = true;
            Debug.Log("All tests done! (" + TestCaseList.Count.ToString() + ")");
        }
    }


    


    void TestInitialize()
    {
        // moves every element to new position for a the new test.
        // run before each test.

        NewTestGo = false;  
        while (!NewTestGo) { 
            RollNewTestNumber();
        }
        testRunNumber++;

        List<int> CaseRunParameters = new List<int>(TestCaseList[testNumber]);    
     
        resLvl = CaseRunParameters[1];
        if (resLvl == 0)
        {GameObject.Find("SystemManager").GetComponent<GameManager>().controllerGain = 0.45f;}
        if (resLvl == 1)
        { GameObject.Find("SystemManager").GetComponent<GameManager>().controllerGain = 1.0f; }
        if (resLvl == 2)
        { GameObject.Find("SystemManager").GetComponent<GameManager>().controllerGain = 1.25f; }



        markPos = CaseRunParameters[3];
        if (markPos == 0)
        {
            targetPosition = GameObject.Find("L_close").transform.position;
        }
        if (markPos == 1)
        {
            targetPosition = GameObject.Find("L_far").transform.position;
        }
        if (markPos == 2)
        {
            targetPosition = GameObject.Find("Middle").transform.position;
        }
        if (markPos == 3)
        {
            targetPosition = GameObject.Find("R_close").transform.position;
        }
        if (markPos == 4)
        {
            targetPosition = GameObject.Find("R_far").transform.position;
        }
        moveTarget.transform.position = targetPosition;
        targetMarker.transform.position = new Vector3(moveTarget.transform.position.x, 0.85f, moveTarget.transform.position.z);

        startPos = CaseRunParameters[2];
        if (startPos == 0)
        {
            StartMarker.transform.position = GameObject.Find("L_close").transform.position;
        }
        if (startPos == 1)
        {
            StartMarker.transform.position = GameObject.Find("L_far").transform.position;
        }
        if (startPos == 2)
        {
            StartMarker.transform.position = GameObject.Find("Middle").transform.position;
        }
        if (startPos == 3)
        {
            StartMarker.transform.position = GameObject.Find("R_close").transform.position;
        }
        if (startPos == 4)
        {
            StartMarker.transform.position = GameObject.Find("R_far").transform.position;
        }
    
        StartMarker.transform.position = new Vector3(StartMarker.transform.position.x, 0.85f, StartMarker.transform.position.z);

        GameObject.Find("SliderTarget").transform.position = new Vector3(
            StartMarker.transform.position.x, 
            (GameObject.Find("SliderTarget").transform.position.y), 
            StartMarker.transform.position.z);
            // bugaa, välillä ei siirrä oikeaan kohtaan. Huomisen figuroitavaa. 
            // kun on liikkunut, arvotaan uudeet speksit, aloituspiste siirtyy mut tää ei siihen minne pitäisi (why?)


        GameObject.Find("Slider").transform.position = new Vector3(
            StartMarker.transform.position.x,
            (GameObject.Find("Slider").transform.position.y),
            StartMarker.transform.position.z);
        




        int moveSpeed = CaseRunParameters[0];
        if (moveSpeed == 0)
        {
            timeToTarget = 2f;
        }
        if (moveSpeed == 1)
        {
            timeToTarget = 3f;
        }
        if (moveSpeed == 2)
        {
            timeToTarget = 4f;
        }


        GameObject.Find("Task_SliderFollow").GetComponent<TaskController_Slider>().ResetTest = true;
        StartCoroutine(BlinkLight(200f));
    }






    void GenerateTestCases()
        // this is done in the beginning to generate all possible test cases. All good and checked.

    {
        TestCaseParameters.Clear();
        for (int m = 0; m < 5; m++)  // mark
        {           
            for (int s = 0; s < 5; s++) //start
            {            
                for (int r = 0; r < 3; r++) //resistance
                {                 
                    for (int t = 0; t < 3; t++) // time
                    {
                        // checks that the distance between start and mark is <2 and never in the same position
                        // only then record the entry as acceptable test cse:
                        if ((s - m < 3) && (s - m > -3) && (s - m != 0))
                            {
                            
                            //Debug.Log("mark: " + m + " start:" + s + " resistance: " + r + " Time: " + t);
                            TestCaseParameters.Insert(0, m); //mark position
                            TestCaseParameters.Insert(0, s); //start position
                            TestCaseParameters.Insert(0, r); // resistance / hand motion speed
                            TestCaseParameters.Insert(0, t); //time to target                        
                            TestCaseList.Insert(0, new List<int>(TestCaseParameters));
                            TestCaseParameters.Clear();

                            
                            /* //testing the values are right in the TestCaseList entries.
                            List<int> testList = new List<int>(TestCaseList[0]);
                            for (int i = 0; i < testList.Count; i++)
                            {
                              Debug.Log(testList[i]); 
                            }
                            */
    }
                        else { TestCaseParameters.Clear(); }
                    }
                }
            }
        }
        Debug.Log("Test cases generated:" + TestCaseList.Count);
        AllTestDone = false;
    }









    private void FixedUpdate()
    {


    }
}