using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using UnityDataPro;
using SFB;

public class LoadImgControl : MonoBehaviour
{
    public Camera headsetCamera;
    public RawImage LdImg;
    public Text ReadingInfoBoard;
    public GameObject ShowImg;
    public Transform CenterDot;//New Idea                     
    private Vector3 LdImgInitPos = new Vector3();
    public Text helpTxt;
    public GameObject SavePrompt;

    UnityDataHandle UDH = new UnityDataHandle();
    private string imageDirectory;

    private void Start()
    {
        imageDirectory = @"C:\RPBANDRAABLog";
        headsetCamera.cullingMask = 0 << 5;
    }
    public void InitInfoBoard()
    {
        ReadingInfoBoard.GetComponent<Text>().text = "Reading test image file: \r\n" + Variables.PickImgFileName;
    }
    

   public void InitHelpTextContent()
    {
        Variables.HelpBtnTrueOrFalse = !Variables.HelpBtnTrueOrFalse;// Added this boolean and made it invert whenever you click the button, so we get text and erase text

        if (Variables.HelpBtnTrueOrFalse == true)
        {
            helpTxt.text = "Operation instruction:" + "\r\n" +

                                                "1. Press the LOAD IMAGE button, navigate to ReadingImg and select one of the Reading# png's." + "\r\n" + "\r\n" +
                                                "2. Pressing the START button will spawn the text, a invisible timer will then begin." + "\r\n" + "\r\n" +
                                                "3. If the image spawns at a poor size, the patient can ask the operator to click the SMALL or LARGE buttons to resize it" + "\r\n" + "\r\n" +
                                                "4. If the image spawns off to the side, the patient can ask the operator to click the LEFT, RIGHT, UP, or DOWN buttons to move the text" + "\r\n" + "\r\n" +
                                                "5. The patient will need to read through the paragraph out loud, the operator will make marks on words the patient misspeaks or skips." + "\r\n" + "\r\n" +
                                                "6. Missing word: put mouse cursor on the word, click mouse left button, it will draw red line on the word." + "\r\n" + "\r\n" +
                                                "7. Reading error: place mouse cursor on the word, right click mouse button, it will draw a blue line on the word" + "\r\n" + "\r\n" +
                                                "8. When the patient is finished reading, click the STOP TEST button, A pop up asking the operator if they would like to save will appear. " + "\r\n" + "\r\n" +
                                                "9. If YES then a screenshot of the paragraph with its marks will be saved under the patient's inputted name, and eye tracking for this test run will be saved in an Excel file." + "\r\n" + "\r\n" +
                                                "10. If NO then NO screenshot of the paragraph with its marks will be saved under the patient's inputted name, and eye tracking for this test run will be NOT saved in an Excel file." + "\r\n" + "\r\n" +
                                                "11. The program will automatically count the number of missing words and reading errors made, and display it above the LOAD IMAGE button." + "\r\n" + "\r\n" +
                                                "12. After this you may repeat steps 1-11 as many times as you have unique paragraphs to read." + "\r\n" 
                                                ;//Added additional instructions and attactched this function to the help button being pressed.
            //helpTxt.transform.position = new Vector3(helpTxt.transform.position.x, helpTxt.transform.position.y, -100);
            //disabling the code above allows me to move the helpTxt to other positions before making my builds and the help text will STAY there.
        }
        if (Variables.HelpBtnTrueOrFalse == false)
        {
            helpTxt.text = "";//Added this if statement to go along with my new boolean.
        }

    }

    

    public void InitShowImg()
    {
        MoveShowImg("Start");// NEW STUFF JOEY
        //ShowImg.GetComponent<RawImage>().rectTransform.sizeDelta = new Vector2(800, 640);
        //RectTransform rt = ShowImg.GetComponent<RawImage>().rectTransform;
    }

    public void StartShowImgPos()
    {
        //ShowImg.GetComponent<RawImage>().rectTransform.position = new Vector3(0f, 0f, -100f);
    }
    
    public void OpenFileFolder()
    {   
        Variables.IsReading = true; 
        Variables.IsReadingInstru = true;
        Variables.helpSwitch = false;
        //Variables.PickImgFileName = UDH.OpenFolderGetFile();
        ExtensionFilter png = new ExtensionFilter("png", "png");
        ExtensionFilter jpg = new ExtensionFilter("jpg", "jpg");
        ExtensionFilter jpeg = new ExtensionFilter("jpeg", "jpeg");
        ExtensionFilter[] extensions = new ExtensionFilter[] { png, jpg, jpeg };
        string[] imagePaths = StandaloneFileBrowser.OpenFilePanel("Open Image File", imageDirectory, extensions, false);

        if(imagePaths.Length == 0)
        {
            System.Windows.Forms.MessageBox.Show("not able to open folder!");
            return;
        }

        List<string> splits = imagePaths[0].Split('\\').ToList();
        splits.RemoveAt(splits.Count-1);
        imageDirectory = string.Join('\\', splits);

        Variables.PickImgFileName = imagePaths[0];
        Variables.DataFileName = Variables.PickImgFileName;
        Variables.WhichBtnClick = 0;
        Variables.IsReadingInstru = true;
        Variables.LineCnt = 0;
        Variables.lineNamePre = "line";
        Variables.IsReadingTracking = true;
        Variables.SetupStartTime("Reading Test");

        InitInfoBoard();
        ShiftLoadImgBoard("Load");
        LoadImg();


    }

    private void ShiftLoadImgBoard(string str)
    {
        if (str == "Load")
        {
            LdImgInitPos = LdImg.rectTransform.position;
            LdImg.rectTransform.position = new Vector3(1000f, LdImg.rectTransform.position.y / 2, 35);
        }

        if (str == "MoveIn")
        {
            LdImg.rectTransform.position = LdImgInitPos;
        }
    }

    private void LoadImg()
    {
        if (Variables.PickImgFileName != null)
            UpdateImage();
    }

    private void UpdateImage()
    {
#pragma warning disable CS0618 // Type or member is obsolete
          UnityEngine.WWW www = new UnityEngine.WWW("file:///" + Variables.PickImgFileName);
       
#pragma warning restore CS0618 // Type or member is obsolete
          LdImg.texture = www.texture;
            //ShowImg.GetComponent<RawImage>().texture = www.texture;     // get the image 

        SetImagePositionInit();

        headsetCamera.cullingMask = 0 << 5;
    }

    private void SetImagePositionInit()
    {
        //Changed this from this: LdImg.rectTransform.position = new Vector3(LdImg.rectTransform.position.x/2, LdImg.rectTransform.position.y/2, 35);
        LdImg.rectTransform.position = new Vector3(CenterDot.position.x, CenterDot.position.y, CenterDot.position.z - 1);//New Idea
#pragma warning disable CS0618 // Type or member is obsolete
        LdImg.rectTransform.sizeDelta = new Vector2(280, 200);//Originaly was 140,100, change to 
        Variables.ImgLeft = LdImg.rectTransform.position.x / 2f;
#pragma warning restore CS0618 // Type or member is obsolete
        Variables.TestDataList.Clear();
    }

    private void ShowImageWhenStart()
    {
        LdImg.rectTransform.position = new Vector3(LdImg.rectTransform.position.x / 2, LdImg.rectTransform.position.y / 2, 35);
        #pragma warning disable CS0618 // Type or member is obsolete
    }

    public void LargeBtnClick()
    {
        LdImg.rectTransform.position = new Vector3(LdImg.rectTransform.position.x, LdImg.rectTransform.position.y, LdImg.rectTransform.position.z - 1);
    }

    public void SmallBtnClick()
    {
        LdImg.rectTransform.position = new Vector3(LdImg.rectTransform.position.x, LdImg.rectTransform.position.y, LdImg.rectTransform.position.z + 1);
    }

    public void UpBtnClick()
    {
        LdImg.rectTransform.position = new Vector3(LdImg.rectTransform.position.x, LdImg.rectTransform.position.y + 1, LdImg.rectTransform.position.z);
    }

    public void DownBtnClick()
    {
        LdImg.rectTransform.position = new Vector3(LdImg.rectTransform.position.x, LdImg.rectTransform.position.y - 1, LdImg.rectTransform.position.z);
    }

    public void LeftBtnClick()
    {
        LdImg.rectTransform.position = new Vector3(LdImg.rectTransform.position.x - 1, LdImg.rectTransform.position.y, LdImg.rectTransform.position.z);
    }

    public void RightBtnClick()
    {
        LdImg.rectTransform.position = new Vector3(LdImg.rectTransform.position.x + 1, LdImg.rectTransform.position.y, LdImg.rectTransform.position.z);
    }
    

    public void QuitBtnClick()
    {
        UnityEngine.Application.Quit();
    }

    public void StartBtnClick()
    {
        for (int i = 0; i < Variables.Marks.Count; i++)
        {
            Destroy(Variables.Marks[i]);

        }//NEW STUFF JOEY
        Variables.Marks.Clear();
        Variables.StartTimeAt = DateTime.Now.ToString();
        Variables.IsStart = true;
        Variables.IsReadingStart = true;
        Variables.IsReadingTracking = true;
        Variables.MissWord = 0;
        Variables.WrongWord = 0;
        Variables.sT = DateTime.Now;
        headsetCamera.cullingMask = 1 << 5;
        InitShowImg();
        //ShiftLoadImgBoard("MoveIn");
        //LoadImg();
    }




    public void StopBtnClick()
    {
        
        SavePrompt.SetActive(true);//10/2/23
        Variables.eT = DateTime.Now;
        Variables.EndTimeAt = DateTime.Now.ToString();
        Variables.IsStart = false;
        Variables.SetupEndTime();//Gets called and records the test end time
        Variables.IsReadingEnd = true;
        Variables.IsReadingTracking = false;
        headsetCamera.cullingMask = 0 << 5;

        Variables.readingMissInfo =  "Test duration = " + Variables.CalculateTestTimeDuration() + " seconds\r\n" +
                                                    "Missing word;" + Variables.MissWord.ToString() + "\r\n" +
                                                    "Wrong word;" + Variables.WrongWord.ToString();
        ReadingInfoBoard.GetComponent<Text>().text = Variables.readingMissInfo;

        
        
        
    }

    public void YesSaveBtn() {
        SavePrompt.SetActive(false);//10/2/23
        ScreenShotSave();//saves screenshot of the mark data
        Variables.FeedOneData("Reading_Eye");//Records the data for this test
        //Variables.counterForTest++;// NEW STUFF JOEY
        for (int i = 0; i < Variables.Marks.Count; i++)
        {
            Destroy(Variables.Marks[i]);

        }   
        Variables.Marks.Clear();
        MoveShowImg("Finish");//NEW STUFF JOEY
        ShiftLoadImgBoard("Load");
    }
    public void NoSaveBtn()
    {
        SavePrompt.SetActive(false);//10/2/23
        for (int i = 0; i < Variables.Marks.Count; i++)
        {
            Destroy(Variables.Marks[i]);

        }
        Variables.Marks.Clear();
        MoveShowImg("Finish");//NEW STUFF JOEY
        ShiftLoadImgBoard("Load");
    }
    private void MoveShowImg(string str)
    {
        //if (str == "Finish")
        //    ShowImg.GetComponent<RawImage>().rectTransform.position = new Vector3(4000f, 0f, -100f); //changed from 2000 to 4000

        //if (str == "Start")
        //    ShowImg.transform.position = new Vector3(1000f, 500f, -100f);// changed from 0 and 0 to 1000, 500

    }
  
    public void SaveBtnClick()
    {
        Variables.IsDataSave = true;
    }
    
 
    public static void ScreenShotSave()
    {
        Variables.ScreenShotCount++;
        //This is a new variable I added so that screenshotsave does not overwrite itself.
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot.Apply();

        byte[] byteArr = screenShot.EncodeToJPG();
        System.IO.File.WriteAllBytes(@"C:\RPBANDRAABLog\Read_" + Variables.LogNamePart + Variables.ScreenShotCount + "Shot.jpg", byteArr);
        //Variables.ScreenShotSave to the above code's name.

        /**

        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot.Apply();

        byte[] byteArr = screenShot.EncodeToJPG();
        System.IO.File.WriteAllBytes(@"C:\RPBANDRAAPLog\Read_" + Variables.LogNamePart + "Shot.jpg",byteArr);
        
        **/
    }
}
