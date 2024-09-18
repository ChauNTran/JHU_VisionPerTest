using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityDataPro;
using System.Runtime.InteropServices;
using UnityServe;
using UnityEngine.UI;
using System;
using System.IO;

/**
README

Problem:
    1. Have not test for generating number of dots larger than 80 (larger than the range unit -40 ~ 40).         -> solved
    2. dots number cannot be 0 or negative. Uesr must put in a valid number at info scene before start the test. -> solved
    3. The headset recalibriate itself everytime start, just fix it instead   -> solved
    4. move update dot size and number inside dot test scene -> solved
    5. add the experientor name to the main manual  -> solved


Alex's update 7/9/23
    1. New codes added in LoadRndDotBtnClick, GenerateDots. 
    2. New functions (RndDotPosGeneration, Shuffle, CreateArray, ConstDotPositions, writeCSV) added at the end of this file. 

Alex's update 7/10/23
    1. successfully ran on the VR at the lab

Alex's update 7/11/23
    1. limited the dots number to be < 80
    2. limited the dots number to be > 0 

Alex's update 7/12/23
    1. fix flow problems
    2. add experimentor name option
    3. change input size and number in dot count scene

Alex's update 7/17/23
    1. fixed the VR ha

Alex's update 7/23/23
    1. attemped fixing saving data issue 

Alex's update 8/30/23
    1. fix cannot open unity hub with M2, attempted fixing that
    2. fix saving dot coordinate


**/


public class DotControlBoard : MonoBehaviour
{

    //mainboard
    public GameObject MnBdImg;

    //change the text 
    public Text MsgTxt;


    public InputField _input;
    public InputField dotSizeInput;
    public InputField dotNumberInput;

    public Text helpTxt;
    public GameObject CenterDot;
    public GameObject SavePrompt;//10/2/23
    public GameObject mainCamera;

    private List<GameObject> RndDots = new List<GameObject>();



    //only used once. not sure what is this for, comment out for now
    //ServeLib Slb = new ServeLib();

    //only used once. Seems important in the main control boards
    public static bool ShowDot1 = false;


    //intialize useful bool for control flow
    public static bool IsStartClick = false;
    public static bool IsRepeatClick = false;
    public static bool IsEndClick = false;


    //defind the cavas size 
    public static int minX = -40;
    public static int maxY = 40;
    public static int RoundCount = 0;

    private void Start()
    {
        RoundCount = 0;
    }

    //read dot size

    public void ReadDotSize()
    {
        if(dotSizeInput.text != "")
        {
            Variables.DotSize = Convert.ToInt32(dotSizeInput.text);
            
        }
    }

    //read dot number
    public void ReadDotNumber()
    {
        if(dotNumberInput.text != "")
        {
            Variables.DotNum = Convert.ToInt32(dotNumberInput.text);
        }
    }


    public void InitHelpTextContent()
    {
        Variables.HelpBtnTrueOrFalse = !Variables.HelpBtnTrueOrFalse;// Added this boolean and made it invert whenever you click the button, so we get text and erase text

        if (Variables.HelpBtnTrueOrFalse == true)
        {
            helpTxt.text = "Operation instruction:" + "\r\n" +

                                                "1. Enter a dot number and dot size before begginning the dot scene." + "\r\n" + "\r\n" +
                                                "2. Upon clicking the Start Test button a timer will start, the patient must count the dots on screen as fast as they can." + "\r\n" + "\r\n" +
                                                "3. Clicking the End Test button will open a popup to choose whether you would like to save." + "\r\n" + "\r\n" +
                                                "4. Selecting YES will save an excel sheet with eye tracking data under the patient's input name." + "\r\n" + "\r\n" +
                                                "5. Selecting NO will not save an excel sheet." + "\r\n" + "\r\n" +
                                                "6. Pressing the START TEST button will spawn a random pattern of dots and begin the test." + "\r\n" + "\r\n" +
                                                "7. Hitting END TEST will stop the timer and erase the pattern." + "\r\n" + "\r\n" +
                                                "8. If you would like to see the same pattern once again, hit REPEAT TEST button only after you have hit the END button once." + "\r\n" + "\r\n" +
                                                "9. The button press order is REPEAT TEST -> END TEST -> REPEAT TEST -> END TEST .... " + "\r\n" + "\r\n" +
                                                "10. If you do not want to reuse the pattern but spawn a brand new one for testing hit START TEST button after the END TEST button." + "\r\n" + "\r\n" +
                                                "11. The button press order for this is START TEST -> END TEST-> START TEST -> END TEST....." + "\r\n" + "\r\n"
                                                ;//Added additional instructions and attactched this function to the help button being pressed.
            //helpTxt.transform.position = new Vector3(-520, -340, -10);//What if I get rid of this thing.
        }
        if (Variables.HelpBtnTrueOrFalse == false)
        {
            helpTxt.text = "";//Added this if statement to go along with my new boolean.
        }

    }

    //on click is called, the random dots are placed on screen
    // unused
    public void StartBtnClick()
    {
        
        //not sure what is this for
        Variables.IsDotCount = true;

        //start data collecting
        Variables.IsCountStart = true;

        //find the mainboard and set its position to a predetermined point
        MnBdImg.transform.position = new Vector3(MnBdImg.transform.position.x, MnBdImg.transform.position.y, -100f);

        
        CenterDot.SetActive(false);


        

        //dot number limit to non-zero and smaller than 80
        //dot size must be non-zero
        if ( Variables.DotNum < 80 && Variables.DotNum > 0 && Variables.DotSize > 0)
        {
            Variables.HasValidNumberAndSize = true;

            //clear the previous saved dots list 
            Variables.RndDotPos.Clear();

            //generate a new list of random dots
            Variables.RndDotPos = RndDotPosGeneration(Variables.DotNum);


            //for load
            IsStartClick = true;
            Variables.IsStart = true;


            //not sure what is this for
            Variables.PlayDotCount = true;

            DateTime dt = DateTime.Now;

            MsgTxt.text = "Find the green dot at center";

            GenerateDots();


        }
        else
        {
            //Maybe we can do the checking at the input page later? 
            Variables.HasValidNumberAndSize = false;
            MsgTxt.text = "Check Dot size and number selected";
        }


    }


    public void StartNoInput()
    {   
        //Added this to keep the dotsize and dotnum stagnant for only the noinput scene 10_17_23
        Variables.DotSize = 3;
        ReadDotNumber();
        Debug.Log("DotNum read: "+Variables.DotNum);
        //not sure what is this for
        Variables.IsDotCount = true;

        //start data collecting
        Variables.IsCountStart = true;

        //find the mainboard and set its position to a predetermined point, get that mainboard out of the way
        MnBdImg.transform.position = new Vector3(MnBdImg.transform.position.x, MnBdImg.transform.position.y, -100f);

        //Gets the center dot off the screen so we don't see it during the test
        CenterDot.SetActive(false);

        //dot number limit to non-zero and smaller than 80
        //dot size must be non-zero
        if (Variables.DotNum < 80 && Variables.DotNum > 0 && Variables.DotSize > 0)
        {
            Variables.HasValidNumberAndSize = true;

            //clear the previous saved dots list 
            Variables.RndDotPos.Clear();
            //Increase roundcount for each test
            RoundCount++;

            //Dot coordinate data from 1Devin excel sheet (10 dots)
            List<Vector2> Devin1 = new List<Vector2>();
            Devin1.Add(new Vector2(-36, -12));
            Devin1.Add(new Vector2(-28, -36));
            Devin1.Add(new Vector2(-20, -28));
            Devin1.Add(new Vector2(-12, 28));
            Devin1.Add(new Vector2(-4, -4));
            Devin1.Add(new Vector2(4, 12));
            Devin1.Add(new Vector2(12, 20));
            Devin1.Add(new Vector2(20, 4));
            Devin1.Add(new Vector2(28, -20));
            Devin1.Add(new Vector2(36, 36));

            //Dot coordinate data from 2Devin excel sheet (10 dots)
            List<Vector2> Devin2 = new List<Vector2>();
            Devin2.Add(new Vector2(-36, -28));
            Devin2.Add(new Vector2(-28, -36));
            Devin2.Add(new Vector2(-20, -4));
            Devin2.Add(new Vector2(-12, 12));
            Devin2.Add(new Vector2(-4, 28));
            Devin2.Add(new Vector2(4, -12));
            Devin2.Add(new Vector2(12, 20));
            Devin2.Add(new Vector2(20, -20));
            Devin2.Add(new Vector2(28, 36));
            Devin2.Add(new Vector2(36, 4));

            //Dot coordinate data from 3Devin excel sheet (10 dots)
            List<Vector2> Devin3 = new List<Vector2>();
            Devin3.Add(new Vector2(-36, 28));
            Devin3.Add(new Vector2(-28, -36));
            Devin3.Add(new Vector2(-20, 12));
            Devin3.Add(new Vector2(-12, -4));
            Devin3.Add(new Vector2(-4, 4));
            Devin3.Add(new Vector2(4, 20));
            Devin3.Add(new Vector2(12, -20));
            Devin3.Add(new Vector2(20, 36));
            Devin3.Add(new Vector2(28, -28));
            Devin3.Add(new Vector2(36, -12));

            //Dot coordinate data from 4Devin excel sheet (11 dots)
            List<Vector2> Devin4 = new List<Vector2>();
            Devin4.Add(new Vector2(-37, -12));
            Devin4.Add(new Vector2(-30, 2));
            Devin4.Add(new Vector2(-23, 30));
            Devin4.Add(new Vector2(-16, 23));
            Devin4.Add(new Vector2(-9, 16));
            Devin4.Add(new Vector2(-2, 9));
            Devin4.Add(new Vector2(5, -26));
            Devin4.Add(new Vector2(12, -33));
            Devin4.Add(new Vector2(19, 37));
            Devin4.Add(new Vector2(26, -19));
            Devin4.Add(new Vector2(33, -5));

            //Dot coordinate data from 5Devin excel sheet (11 dots)
            List<Vector2> Devin5 = new List<Vector2>();
            Devin5.Add(new Vector2(-37, 2));
            Devin5.Add(new Vector2(-30, -12));
            Devin5.Add(new Vector2(-23, 23));
            Devin5.Add(new Vector2(-16, -19));
            Devin5.Add(new Vector2(-9, 37));
            Devin5.Add(new Vector2(-2, -26));
            Devin5.Add(new Vector2(5, 16));
            Devin5.Add(new Vector2(12, 30));
            Devin5.Add(new Vector2(19, -5));
            Devin5.Add(new Vector2(26, -33));
            Devin5.Add(new Vector2(33, 9));

            //Dot coordinate data from 6Devin excel sheet (11 dots)
            List<Vector2> Devin6 = new List<Vector2>();
            Devin6.Add(new Vector2(-37, -26));
            Devin6.Add(new Vector2(-30, 16));
            Devin6.Add(new Vector2(-23, 2));
            Devin6.Add(new Vector2(-16, -33));
            Devin6.Add(new Vector2(-9, 37));
            Devin6.Add(new Vector2(-2, -5));
            Devin6.Add(new Vector2(5, -12));
            Devin6.Add(new Vector2(12, 23));
            Devin6.Add(new Vector2(19, 30));
            Devin6.Add(new Vector2(26, 9));
            Devin6.Add(new Vector2(33, -19));


            //Dot coordinate data from 7Devin excel sheet (12 dots)
            List<Vector2> Devin7 = new List<Vector2>();
            Devin7.Add(new Vector2(-37, 31));
            Devin7.Add(new Vector2(-31, 1));
            Devin7.Add(new Vector2(-25, 25));
            Devin7.Add(new Vector2(-19, -11));
            Devin7.Add(new Vector2(-13, 13));
            Devin7.Add(new Vector2(-7, -17));
            Devin7.Add(new Vector2(-1, 37));
            Devin7.Add(new Vector2(5, 7));
            Devin7.Add(new Vector2(11, -5));
            Devin7.Add(new Vector2(17, -29));
            Devin7.Add(new Vector2(23, -23));
            Devin7.Add(new Vector2(29, 19));

            //Dot coordinate data from 8Devin excel sheet (12 dots)
            List<Vector2> Devin8 = new List<Vector2>();
            Devin8.Add(new Vector2(-37, -5));
            Devin8.Add(new Vector2(-31, -23));
            Devin8.Add(new Vector2(-25, 19));
            Devin8.Add(new Vector2(-19, -29));
            Devin8.Add(new Vector2(-13, 37));
            Devin8.Add(new Vector2(-7, -17));
            Devin8.Add(new Vector2(-1, 1));
            Devin8.Add(new Vector2(5, 7));
            Devin8.Add(new Vector2(11, 13));
            Devin8.Add(new Vector2(17, -11));
            Devin8.Add(new Vector2(23, 31));
            Devin8.Add(new Vector2(29, 25));



            //Dot coordinate data from 9Devin excel sheet (12 dots)
            List<Vector2> Devin9 = new List<Vector2>();
            Devin9.Add(new Vector2(-37, 13));
            Devin9.Add(new Vector2(-31, -23));
            Devin9.Add(new Vector2(-25, -11));
            Devin9.Add(new Vector2(-19, 7));
            Devin9.Add(new Vector2(-13, 31));
            Devin9.Add(new Vector2(-7, -5));
            Devin9.Add(new Vector2(-1, 1));
            Devin9.Add(new Vector2(5, 25));
            Devin9.Add(new Vector2(11, 37));
            Devin9.Add(new Vector2(17, -29));
            Devin9.Add(new Vector2(23, 19));
            Devin9.Add(new Vector2(29, -17));


            //Dot coordinate data from 10Devin excel sheet (13 dots)
            List<Vector2> Devin10 = new List<Vector2>();
            Devin10.Add(new Vector2(-37, 19));
            Devin10.Add(new Vector2(-31, 13));
            Devin10.Add(new Vector2(-25, -35));
            Devin10.Add(new Vector2(-19, -23));
            Devin10.Add(new Vector2(-13, -5));
            Devin10.Add(new Vector2(-7, -17));
            Devin10.Add(new Vector2(-1, -11));
            Devin10.Add(new Vector2(5, 37));
            Devin10.Add(new Vector2(11, 7));
            Devin10.Add(new Vector2(17, 31));
            Devin10.Add(new Vector2(23, 25));
            Devin10.Add(new Vector2(29, 1));
            Devin10.Add(new Vector2(35, -29));

            //Dot coordinate data from 11Devin excel sheet (13 dots)
            List<Vector2> Devin11 = new List<Vector2>();
            Devin11.Add(new Vector2(-37, 25));
            Devin11.Add(new Vector2(-31, -35));
            Devin11.Add(new Vector2(-25, -23));
            Devin11.Add(new Vector2(-19, 13));
            Devin11.Add(new Vector2(-13, -11));
            Devin11.Add(new Vector2(-7, 31));
            Devin11.Add(new Vector2(-1, 37));
            Devin11.Add(new Vector2(5, 1));
            Devin11.Add(new Vector2(11, 19));
            Devin11.Add(new Vector2(17, 7));
            Devin11.Add(new Vector2(23, -29));
            Devin11.Add(new Vector2(29, -5));
            Devin11.Add(new Vector2(35, -17));

            //Dot coordinate data from 12Devin excel sheet (13 dots)
            List<Vector2> Devin12 = new List<Vector2>();
            Devin12.Add(new Vector2(-37, -23));
            Devin12.Add(new Vector2(-31, 13));
            Devin12.Add(new Vector2(-25, 37));
            Devin12.Add(new Vector2(-19, -11));
            Devin12.Add(new Vector2(-13, -17));
            Devin12.Add(new Vector2(-7, -35));
            Devin12.Add(new Vector2(-1, 25));
            Devin12.Add(new Vector2(5, 1));
            Devin12.Add(new Vector2(11, -5));
            Devin12.Add(new Vector2(17, 31));
            Devin12.Add(new Vector2(23, -29));
            Devin12.Add(new Vector2(29, 7));
            Devin12.Add(new Vector2(35, 19));

            //Dot coordinate data from 13Devin excel sheet (14 dots)
            List<Vector2> Devin13 = new List<Vector2>();
            Devin13.Add(new Vector2(-38, 8));
            Devin13.Add(new Vector2(-33, 23));
            Devin13.Add(new Vector2(-28, 18));
            Devin13.Add(new Vector2(-23, -12));
            Devin13.Add(new Vector2(-18, 33));
            Devin13.Add(new Vector2(-13, 28));
            Devin13.Add(new Vector2(-8, 38));
            Devin13.Add(new Vector2(-3, -22));
            Devin13.Add(new Vector2(2, 13));
            Devin13.Add(new Vector2(7, -17));
            Devin13.Add(new Vector2(12, -2));
            Devin13.Add(new Vector2(17, -27));
            Devin13.Add(new Vector2(22, -7));
            Devin13.Add(new Vector2(27, 3));

            //Dot coordinate data from 14Devin excel sheet (14 dots)
            List<Vector2> Devin14 = new List<Vector2>();
            Devin14.Add(new Vector2(-38, 28));
            Devin14.Add(new Vector2(-33, -2));
            Devin14.Add(new Vector2(-28, -7));
            Devin14.Add(new Vector2(-23, 8));
            Devin14.Add(new Vector2(-18, 18));
            Devin14.Add(new Vector2(-13, 38));
            Devin14.Add(new Vector2(-8, -17));
            Devin14.Add(new Vector2(-3, -22));
            Devin14.Add(new Vector2(2, 3));
            Devin14.Add(new Vector2(7, 33));
            Devin14.Add(new Vector2(12, -27));
            Devin14.Add(new Vector2(17, -12));
            Devin14.Add(new Vector2(22, 13));
            Devin14.Add(new Vector2(27, 23));

            //Dot coordinate data from 15Devin excel sheet (14 dots)
            List<Vector2> Devin15 = new List<Vector2>();
            Devin15.Add(new Vector2(-38, 28));
            Devin15.Add(new Vector2(-33, 13));
            Devin15.Add(new Vector2(-28, 3));
            Devin15.Add(new Vector2(-23, -12));
            Devin15.Add(new Vector2(-18, -2));
            Devin15.Add(new Vector2(-13, -27));
            Devin15.Add(new Vector2(-8, 8));
            Devin15.Add(new Vector2(-3, 18));
            Devin15.Add(new Vector2(2, 38));
            Devin15.Add(new Vector2(7, 33));
            Devin15.Add(new Vector2(12, -22));
            Devin15.Add(new Vector2(17, -7));
            Devin15.Add(new Vector2(22, 23));
            Devin15.Add(new Vector2(27, -17));

            List<Vector2>[] matrix = {Devin1, Devin2, Devin3, Devin4, Devin5, Devin6, Devin7, Devin8, Devin9, Devin10, Devin11, Devin12, Devin13, Devin14, Devin15};
            //Assign Devin7 or 8 or 9 coordinates depending on test # so they will be used in dot generation
            if (RoundCount == 1)
            {   //ReadDotNumber();
                int index = Variables.DotNum -1;
                Variables.RndDotPos = matrix[index];
                //Debug.Log("List chosen: "+Variables.RndDotPos);
                Variables.SetUsed = "You used set Devin"+(Variables.DotNum);
                Debug.Log("List chosen: "+Variables.SetUsed);
                Variables.DotNum = (matrix[index]).Count;
                //Debug.Log("list length: "+Variables.DotNum);
            }
            if (RoundCount == 2)
            {   //ReadDotNumber();
                int index = Variables.DotNum -1;
                Variables.RndDotPos = matrix[index];
                //Debug.Log("List chosen: "+Variables.RndDotPos);
                Variables.SetUsed = "You used set Devin"+(Variables.DotNum);
                Debug.Log("List chosen: "+Variables.SetUsed);
                Variables.DotNum = (matrix[index]).Count;
                //Debug.Log("list length: "+Variables.DotNum);
            
            }
            if (RoundCount == 3)
            {   //ReadDotNumber();
                int index = Variables.DotNum -1;
                Variables.RndDotPos = matrix[index];
                //Debug.Log("List chosen: "+Variables.RndDotPos);
                Variables.SetUsed = "You used set Devin"+(Variables.DotNum);
                Debug.Log("List chosen: "+Variables.SetUsed);
                Variables.DotNum = (matrix[index]).Count;
                //Debug.Log("list length: "+Variables.DotNum);
            
            }

            if (RoundCount >3) {
                //generate a new list of random positioned dots based on the dotnum and puts it in a variable.list of dots only after you have conducted 3 rounds
                //Randomly generate dots with in a range of given input
                int N = UnityEngine.Random.Range(12-2,12+3);
                Variables.DotNum = N;
                Variables.RndDotPos = RndDotPosGeneration(Variables.DotNum);
            }


            //for load
            IsStartClick = true;
            Variables.IsStart = true;


            //not sure what is this for
            Variables.PlayDotCount = true;

            DateTime dt = DateTime.Now;

            MsgTxt.text = "Find the green dot at center";
            
            //Calls generatedots now that we have a list of random dot coordinates that can be publicly accessed.
            GenerateDots();


        }
        else
        {
            //Maybe we can do the checking at the input page later? 
            Variables.HasValidNumberAndSize = false;
            MsgTxt.text = "Check Dot size and number selected";
        }


    }


    //the function calls to create dots based off of list of random dot coordinates
    public void GenerateDots()
    {        


        Variables.PlayDotStart = true;

        Variables.RndDotLog.Clear();

        //flow conditions
        if ((Variables.DotNum > 0 && Variables.DotSize > 0 && IsStartClick ))
        {

            Variables.SetupStartTime("DotCountTest");

            //store starting time
            MsgTxt.text = "Start Counting";
            
            

            for (int i = 0; i < Variables.DotNum; i++)
            {
                //create individual game object sphere
                GameObject tmpDot = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                //assign the sphere to a parent, camera
                tmpDot.transform.parent = mainCamera.transform;

                //assign position
                tmpDot.transform.localPosition = new Vector3(Variables.RndDotPos[i].x  , Variables.RndDotPos[i].y  , 100);// IS THIS SUPOSED TO BE 100 or 10???

                //assign size
                tmpDot.transform.localScale = new Vector3(0.2f * Variables.DotSize, 0.2f * Variables.DotSize, 0.1f);

                //assign color
                tmpDot.GetComponent<Renderer>().material.color = Color.white;

                //assign name
                tmpDot.name = "Dot" + i.ToString();

                RndDots.Add(tmpDot);

                double xx = Variables.RndDotPos[i].x / Mathf.Sqrt(Mathf.Pow(Variables.RndDotPos[i].x, 2) + Mathf.Pow(Variables.RndDotPos[i].y, 2) + Mathf.Pow(100, 2));//NewStuff

                double yy = Variables.RndDotPos[i].y / Mathf.Sqrt(Mathf.Pow(Variables.RndDotPos[i].x, 2) + Mathf.Pow(Variables.RndDotPos[i].y, 2) + Mathf.Pow(100, 2));//New Stuff

                double zz = 100 / Mathf.Sqrt(Mathf.Pow(Variables.RndDotPos[i].x, 2) + Mathf.Pow(Variables.RndDotPos[i].y, 2) + Mathf.Pow(100, 2));//New Stuff

                Variables.RndDotLog.Add(i.ToString() + "," + xx.ToString("N3") + "," + yy.ToString("N3") + "," + zz.ToString("N3"));//New Stuff

                //Variables.RndDotLog.Add(i.ToString() + "," + Variables.RndDotPos[i].x.ToString() + "," +  Variables.RndDotPos[i].y.ToString() + ",100" + "," + RoundCount); //OLD STUFF
            }

            
            
        }
        else
        {
            MsgTxt.text = "Must end before restart";
        }
        

    }

    //on click is called, the last set of dots will be loaded 
    public void RepeatBtnClick(){


        if (IsStartClick){

            //not sure what is this for
            Variables.IsDotCount = true;

            //start data collecting
            Variables.IsCountStart = true;

            GenerateDots();
        }else{
            MsgTxt.text = "Must end before repeat";
        }

        
    }

    //on click is called, the dots will be removed 
    public void EndTestBtn()
    {
        
        SavePrompt.SetActive(true);//10/2/23

        //Gets called and records the end time
        Variables.SetupEndTime();

        if (IsStartClick){

            foreach (GameObject tmpDot in RndDots)
            {
                Destroy(tmpDot);
            }

            RndDots.Clear();

            Variables.IsStart = false;
            Variables.IsCountStart = false;
            Variables.PlayDotStop = true;
            
            
            

            
            MsgTxt.text = "Finished with " + Variables.DotNum + " dots";
            CenterDot.SetActive(true);

        }
        else
        {
            MsgTxt.text = "Must start before end";
        }



        ReadDotNumber();
    }
    public void EndNoInput()
    {

        SavePrompt.SetActive(true);//10/2/23

        //Gets called and records the end time
        Variables.SetupEndTime();

        if (IsStartClick)
        {

            foreach (GameObject tmpDot in RndDots)
            {
                Destroy(tmpDot);
            }

            RndDots.Clear();

            Variables.IsStart = false;
            Variables.IsCountStart = false;
            Variables.PlayDotStop = true;





            MsgTxt.text = "Finished with " + Variables.DotNum + " dots";
            CenterDot.SetActive(true);

        }
        else
        {
            MsgTxt.text = "Must start before end";
        }



        
    }

    public void YesSaveBtn()
    {
        SavePrompt.SetActive(false);//10/2/23
        Variables.FeedOneData("DotCount_Eye");
        

    }
    public void NoSaveBtn()
    {
        SavePrompt.SetActive(false);//10/2/23
    }





    //Given the number of dots, this function returns a list of x,y coordinate pairs
    //there should be no two x or y coordinates are the same     
    public List<Vector2> RndDotPosGeneration(int numberOfDots)
    {
       Vector2[,] DotPositions = ConstDotPositions(numberOfDots);

       int[] x_coor = CreateArray(numberOfDots);
       int[] y_coor = Shuffle(x_coor);


        List<Vector2> FinalDotPositions = new List<Vector2>();


        for (int i = 0; i < numberOfDots; i++){
            int x = x_coor[i];
            int y = y_coor[i];
            Vector2 element = DotPositions[x,y];
            FinalDotPositions.Add(element);
            //Debug.Log("final position:" + element);
        } 
       return FinalDotPositions;

    }


    //helper method 1
    //take a 1D array with (x,y) components, return a shuffled version of the array 
    public int[] Shuffle(int[] array)
    {
        int[] shuffledArray = new int[array.Length];
        array.CopyTo(shuffledArray, 0);

        int n = array.Length;
        for (int i = 0; i < n - 1; i++)
        {
            //randomly swich positions of i to an elements at the array in each run 
            int j = UnityEngine.Random.Range(i, n);
            int temp = shuffledArray[i];
            shuffledArray[i] = shuffledArray[j];
            shuffledArray[j] = temp;

            
        }


        foreach (int element in shuffledArray)
        {
            //Debug.Log("y_coor: " + element);
        }

        return shuffledArray;
    }

    //helper method 2
    //creating array from 0 to N
    public int[] CreateArray(int N)
    {
        int[] array = new int[N];
        for (int i = 0; i < N; i++)
        {
            array[i] = i;
            
       //Debug.Log("x_coor: " + array[i]);
       
        }

        
        return array;
    }

    
    //helper method 3
    //takes interger N  and return a 2D vector of x,y paires in intergers 
    //Assume boundings are x: -40 ~ 40; y: 40 ~ -40
    public Vector2[,] ConstDotPositions(int numberOfDots)
    {
        Vector2[,] positions = new Vector2[numberOfDots, numberOfDots];


        //HalfIncre: half increment/size of a square 
        //FullIncre: size of a square
        int HalfIncre = 80 / (numberOfDots * 2);
        int FullIncre = 80 / numberOfDots; 

        //Debug.Log("HalfIncre:" + HalfIncre);
        //Debug.Log("FullIncre:" + FullIncre);

        //set x coordinate out of for loop
        int x = -40; 
        

        //iterate N*N times to generate N*N dots, more demo see demo.pdf 
        for (int row = 0; row < numberOfDots; row++ ){

            //the first position is half increment
            if( row == 0){
                x = x + HalfIncre;
            }else{
                x = x + FullIncre;
            }

            //renew y coordinate in first for loop
            int y = 40; 

            for (int col = 0; col < numberOfDots; col++){

                //the first position is half increment
                if( col == 0){
                    y = y - HalfIncre;
                }else{
                   y = y - FullIncre;
                }

                positions[row, col] = new Vector2(x, y);

                //Debug.Log("Position:" + positions[row, col]);
            }

        }

         return positions;
    }




    //generate CSV with dots positions to Alex's desktop
    public void writeCSV(){

        string filename = "/Users/alexandra/Desktop" + Variables.PatientName + ".txt";

        //false: the test file will be over-written everytime called this function
        //true:  the file will not be over-written
        TextWriter tw = new StreamWriter(filename, true);



        //for getting dot's position
        /**
        for (int i = 0; i <Variables.RndDotPos.Count; i++ ){
            tw.WriteLine(i.ToString() + "," + Variables.RndDotPos[i].x.ToString() + "," + Variables.RndDotPos[i].y.ToString());
        }
        **/


        tw.Close();
    }
 
 
}
