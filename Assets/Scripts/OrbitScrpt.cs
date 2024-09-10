using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityDataPro;
using System.Runtime.InteropServices;
using UnityServe;
using UnityEngine.UI;
using System;
using System.IO;




public class OrbitalScrpt : MonoBehaviour
{

    //mainboard
    GameObject MnBdImg;

    

    //change the text 
    GameObject MsgTxt;
    


    public InputField _input;



    //public static List<GameObject> RndDots = new List<GameObject>();



    //only used once. not sure what is this for, comment out for now
    ServeLib Slb = new ServeLib();

    //only used once. Seems important in the main control boards
    public static bool ShowDot1 = false;


    //intialize useful bool for control flow
    public static bool IsStartClick = false;
    public static bool IsRepeatClick = false;
    public static bool IsEndClick = false;


    //defind the cavas size 
    public static int minX = -40;
    public static int maxY = 40;


    //read orbit radius
    public void ReadOrbitRadius()
    {
        if (_input.text != "")
        {
            Variables.Radius = Convert.ToInt32(_input.text);

        }
    }

    
    public GameObject helpTxt;
    public GameObject CenterDot;

    public GameObject SavePrompt;//10/2/23

    public void InitHelpTextContent()
    {
        Variables.HelpBtnTrueOrFalse = !Variables.HelpBtnTrueOrFalse;// Added this boolean and made it invert whenever you click the button, so we get text and erase text

        if (Variables.HelpBtnTrueOrFalse == true)
        {
            helpTxt = GameObject.Find("HelpText");
            helpTxt.GetComponent<Text>().text = "Operation instruction:" + "\r\n" +

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
            helpTxt = GameObject.Find("HelpText");
            helpTxt.GetComponent<Text>().text = "";//Added this if statement to go along with my new boolean.
        }

    }

    //on click is called, the random dots are placed on screen
    public void StartBtnClick()
    {   
        Variables.IsDotCount = false;

        //Sets the number of Dots to be 1 since we will only see one at a time in this test 10/10/23
        Variables.DotNum = 13;

        //Sets the Dot Size to a defualt value of 3 10/10/23
        Variables.DotSize = 3;

        //not sure what is this for
        Variables.IsOrbit = true;

        //start data collecting
        Variables.IsCountStart = true;


        CenterDot.SetActive(false);


        //dot number limit to non-zero and smaller than 80
        //dot size must be non-zero
        if (Variables.DotNum < 80 && Variables.DotNum > 0 && Variables.DotSize > 0)
        {
            Variables.HasValidNumberAndSize = true;

            //clear the previous saved dots list so you will get unique dots when you call generate dots and not the same as last time.
            Variables.RndDotPos.Clear();

            //generate a new list of random dots
            Variables.RndDotPos = RndDotPosGeneration(Variables.DotNum);


            //for load
            IsStartClick = true;
            Variables.IsStart = true;


            //not sure what is this for
            Variables.PlayDotCount = true;

            DateTime dt = DateTime.Now;

            MsgTxt = GameObject.Find("MsgText");
            MsgTxt.GetComponent<Text>().text = "Find the green dot at center";

            GenerateDots();


        }
        else
        {
            //Maybe we can do the checking at the input page later? 
            Variables.HasValidNumberAndSize = false;
            MsgTxt = GameObject.Find("MsgText");
            MsgTxt.GetComponent<Text>().text = "Check Dot size and number selected";
        }


    }

    //the function calls to generate random dot positions
    public void GenerateDots()
    {
        //find the camera
        GameObject cam;
        cam = GameObject.Find("Main Camera");



        Variables.PlayDotStart = true;

        Variables.RndDotLog.Clear();

        //flow conditions
        if ((Variables.DotNum > 0 && Variables.DotSize > 0 && IsStartClick))
        {
            //Store Starting Time
            Variables.SetupStartTime("Orbit Test");

            //Changes title to the look at the white dot.
            MsgTxt = GameObject.Find("MsgText");
            MsgTxt.GetComponent<Text>().text = "Look at the white dot";



            for (int i = 0; i < Variables.DotNum; i++)
            {
                //create indivisual game object sphere
                GameObject tmpDot = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                //assign the sphere to a parent, camera
                tmpDot.transform.parent = cam.transform;
                

                //assign position
                tmpDot.transform.localPosition = new Vector3(Variables.RndDotPos[i].x, Variables.RndDotPos[i].y, 100);// IS THIS SUPOSED TO BE 100 or 10???
                //This data needs to be changed, we need to generate x and y coordinates that with this function SQRT(x^2 + y^2 ) = radius value inputed 10/10/23

                //assign size
                tmpDot.transform.localScale = new Vector3(0.2f * Variables.DotSize, 0.2f * Variables.DotSize, 0.1f);

                //assign color
                tmpDot.GetComponent<Renderer>().material.color = Color.white;

                //assign name
                tmpDot.name = "Dot" + i.ToString();

                double xx = Variables.RndDotPos[i].x / Mathf.Sqrt(Mathf.Pow(Variables.RndDotPos[i].x, 2) + Mathf.Pow(Variables.RndDotPos[i].y, 2) + Mathf.Pow(100, 2));//NewStuff

                double yy = Variables.RndDotPos[i].y / Mathf.Sqrt(Mathf.Pow(Variables.RndDotPos[i].x, 2) + Mathf.Pow(Variables.RndDotPos[i].y, 2) + Mathf.Pow(100, 2));//New Stuff

                double zz = 100 / Mathf.Sqrt(Mathf.Pow(Variables.RndDotPos[i].x, 2) + Mathf.Pow(Variables.RndDotPos[i].y, 2) + Mathf.Pow(100, 2));//New Stuff

                Variables.RndDotLog.Add(i.ToString() + "," + xx.ToString("N3") + "," + yy.ToString("N3") + "," + zz.ToString("N3"));//New Stuff

                //Variables.RndDotLog.Add(i.ToString() + ";" + Variables.RndDotPos[i].x.ToString() + ";" +  Variables.RndDotPos[i].y.ToString()); //OLD STUFF
            }



        }
        else
        {
            MsgTxt = GameObject.Find("MsgText");
            MsgTxt.GetComponent<Text>().text = "Must end before restart";
        }


    }

    //on click is called, the last set of dots will be loaded 
    public void RepeatBtnClick()
    {


        if (IsStartClick)
        {

            //not sure what is this for
            Variables.IsOrbit = true;

            //start data collecting
            Variables.IsCountStart = true;

            GenerateDots();
        }
        else
        {
            MsgTxt = GameObject.Find("MsgText");
            MsgTxt.GetComponent<Text>().text = "Must end before repeat";
        }


    }

    //on click is called, the dots will be removed 
    public void EndTestBtn()
    {   
        //activates the saveprompt gameobject so you can choose if you would like to save or not
        SavePrompt.SetActive(true);//10/2/23
        
        //Gets called and records the end time
        Variables.SetupEndTime();
        // looks to see if IsStartClick = true, then deletes the onscreen dots.
        if (IsStartClick)
        {

            for (int i = 0; i < Variables.DotNum; i++)
            {
                string objN = "Dot" + i.ToString();
                GameObject tmpDot = GameObject.Find(objN);
                Destroy(tmpDot);
            }

            Variables.IsStart = false;
            Variables.IsCountStart = false;
            Variables.PlayDotStop = true;





            MsgTxt = GameObject.Find("MsgText");
            MsgTxt.GetComponent<Text>().text = "Finished with " + Variables.DotNum + " dots";
            CenterDot.SetActive(true);
        }
        else
        {
            MsgTxt = GameObject.Find("MsgText");
            MsgTxt.GetComponent<Text>().text = "Must start before end";
        }




    }

    //SavePrompt activated buttons that are attatched to buttons, clicking each button will activate each situation and give different saves.
    public void YesSaveBtn()
    {
        SavePrompt.SetActive(false);//10/2/23
        Variables.FeedOneData("DotOrbit_Eye");      
        

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


        for (int i = 0; i < numberOfDots; i++)
        {
            int x = x_coor[i];
            int y = y_coor[i];
            Vector2 element = DotPositions[x, y];
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
        for (int row = 0; row < numberOfDots; row++)
        {

            //the first position is half increment
            if (row == 0)
            {
                x = x + HalfIncre;
            }
            else
            {
                x = x + FullIncre;
            }

            //renew y coordinate in first for loop
            int y = 40;

            for (int col = 0; col < numberOfDots; col++)
            {

                //the first position is half increment
                if (col == 0)
                {
                    y = y - HalfIncre;
                }
                else
                {
                    y = y - FullIncre;
                }

                positions[row, col] = new Vector2(x, y);

                //Debug.Log("Position:" + positions[row, col]);
            }

        }

        return positions;
    }




    //generate CSV with dots positions to Alex's desktop
    public void writeCSV()
    {

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

