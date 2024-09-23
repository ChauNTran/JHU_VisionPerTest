using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityServe;
using System.IO;
using System;

public class Variables : MonoBehaviour
{
    public struct OneDot
    {
        public int Cnt;
        public float X;
        public float Y;
        public int MaxN;                        
    }

    public struct OneE
    {
        public DateTime TmStp;
        public Vector3 Left;
        public Vector3 Right;
    }

    private static ServeLib SrLib = new ServeLib();
    public static bool dtcBool = false;
    public static string StartTimeAt;
    public static string StartDate;                 
    public static string EndTimeAt;
    public static bool IsStart;                    
    public static string BasicTestInfo;             
    public static float ImgLeft = 0.0f;
    public static string PickImgFileName;
    public static bool CheckIn = false;
    public static List<String> TestLoglist = new List<string>();            
    public static string LogNamePart;
    public static int TestrepeatNum;            
    //public static bool IsStartClick;                
    //public static bool IsEndClick;                  
    public static bool IsDataSave;
    public static String DataFileName;                  
    public static List<OneE> TestDataList = new List<OneE>();
    public static List<string> TestDataStringList = new List<string>();
    public static string TestiInfo;
    public static DateTime timeDuration;                 
    public static DateTime sT;                          
    public static DateTime eT;                          
    public static string PatientName = "";  
    public static string ExperimenterName = "";  
    public static int DotSize;                  
    public static int DotNum;
    public static int Radius;//10/10/23
    public static float MinDistValue = 0.5f;    
    public static bool HasValidNumberAndSize = false;    
    public static float DotsDist = 0f;
    public static int SelectMinDist = 0;                 
    public static List<string> RawRndDotList;
    public static string RndDotFName;
    public static List<OneDot> RndDotList = new List<OneDot>();
    public static bool PlayDotCount = false;
    public static bool PlayFinish = false;
    public static bool PlayDotStart = false;
    public static bool PlayDotStop = false;
    public static bool IsDotCount;
    public static bool IsCountStart;
    public static List<string> RndDotLog = new List<string>();         
    public static List<string> PreLoadDotFileList = new List<string>();
    public static List<OneDot> MinDist80 = new List<OneDot>();
    public static List<OneDot> MinDist60 = new List<OneDot>();
    public static List<OneDot> MinDist40 = new List<OneDot>();
    public static List<OneDot> MinDist20 = new List<OneDot>();
    public static List<string> MinDistFileList = new List<string>();
    public static bool IsOrbit;//10/10/23
    public static bool IsReading;
    public static bool IsReadingInstru;
    public static bool IsReadingStart;
    public static bool IsReadingEnd;
    public static bool IsReadingTracking;          
    public static int LineCnt;                     
    public static string lineNamePre;              
    public static bool helpSwitch;                 
    public static int MissWord;                    
    public static int WrongWord;                   
    public static int WhichBtnClick;               
    public static string readingMissInfo;          
    public static float CurrColorVal = 0f;
    public static int DefColor = 0;
    public static bool CntDownColor = true;
    public static bool IsCaliContrast;
    public static int ScreenShotCount = 0;
    public static int counterForTest = 1;
    public static bool HelpBtnTrueOrFalse = false;// new help button stuff
    public static List<Vector2> RndDotPos = new List<Vector2>();
    public static List<GameObject> Marks = new List<GameObject> {};
    public static bool YesSaveBtnClicked = false;// 10/3/23 new stuff
    public static bool NoSaveBtnClicked = false;// 10/3/23 new stuff
    public static string SetUsed;//10/17/23
    





    public static void SetupStartTime(string TestName)
    {
        if (TestName == "DotCountTest")
        {

            TestLoglist.Clear();
            BasicTestInfo = "PatientName," + PatientName + ",ExperimenterName," + ExperimenterName + "," + TestName + "," + "DotSize = " + DotSize.ToString() + "," + "Test Dot Number = " + DotNum.ToString();
            TestLoglist.Add(BasicTestInfo);
            
            TestDataList.Clear();
        }

        if (TestName == "Reading Test")
        {
            TestLoglist.Clear();
            BasicTestInfo = "PatientName," + PatientName + ",ExperimenterName," + ExperimenterName + ",Test," + TestName + ", Reading File," + DataFileName;
            TestLoglist.Add(BasicTestInfo);
        }
        //Orbit version added 10/10/23
        if (TestName == "Orbit Test")
        {
            TestLoglist.Clear();
            BasicTestInfo = "PatientName," + PatientName + ",ExperimenterName," + ExperimenterName + ",Test," + TestName + ", Radius," + Radius.ToString();
            TestLoglist.Add(BasicTestInfo);
        }

        StartTimeAt = DateTime.Now.ToString("HH:mm:ss.fff");
        StartDate = DateTime.Now.ToString("yyyy-MM-dd");

        IsStart = true;

        if (IsDotCount)
        {
            TestiInfo = "Dot Counting Test" + " , Start Test Date, " + StartDate + ",Start Time," + StartTimeAt;
            TestLoglist.Add(TestiInfo);
        }
        if(IsReading)
        {
            TestiInfo = "Reading Test , " + "Start Test Date," + StartDate + ", Start Time," + StartTimeAt;
            TestLoglist.Add(TestiInfo);
        }

        //Added a Orbit version 10/10/23
        if (IsOrbit)
        {
            TestiInfo = "Orbit Test , " + "Start Test Date," + StartDate + ", Start Time," + StartTimeAt;
            TestLoglist.Add(TestiInfo);
        }
    }
    
    public static void SetupEndTime()
    {
        EndTimeAt = DateTime.Now.ToString("HH:mm:ss.fff");
        IsStart = false;                        // stop
        if(Variables.IsDotCount)
            TestLoglist.Add("Dot Counting Test"+ " , End Test time , " + EndTimeAt + "\r\n");
        if(Variables.IsReading)
            TestLoglist.Add("Reading Test , End Test time , " + EndTimeAt + "\r\n");
        if (Variables.IsOrbit)
            TestLoglist.Add("Orbit Test , End Test time , " + EndTimeAt + "\r\n");

    }

    public static void BuildLogFileNamePart()
    {
        LogNamePart = SrLib.BuildLogNamePart(PatientName);
    }
    

    public void DownColorBtnClick()
    {
        Variables.CntDownColor = true;
    }

    public static void MinDistFileNameList()
    {
        MinDistFileList.Clear();
        MinDistFileList.Add(@"C:\RPBANDRAABLog\RndGridList\MinDistance_80_Unity.txt");
        MinDistFileList.Add(@"C:\RPBANDRAABLog\RndGridList\MinDistance_60_Unity.txt");
        MinDistFileList.Add(@"C:\RPBANDRAABLog\RndGridList\MinDistance_40_Unity.txt");
        MinDistFileList.Add(@"C:\RPBANDRAABLog\RndGridList\MinDistance_20_Unity.txt");
    }
    

    public void UpColorBtnClick()
    {
        CntDownColor = false;
        CaliContrastAllowed();
    }

    public static bool ChkIn()
    {
        ServeLib Slib = new ServeLib();
        CheckIn = Slib.PassChk;
        return CheckIn;
    }

    public static void DotsDistance()
    {
        DotsDist = SrLib.GetDotDistance(0.4f, 1.6f, 2.6f, 5.1f);
    }

    public static void GetRndDotList()
    {
        RawRndDotList = new List<string>();
        RawRndDotList = SrLib.ReadFileFromFile(@"C:\VFTLog\RndDot.csv");
    }
    
    public static void CheckStringList(List<string> InList)
    {
        int i = 0;
        foreach(string str in InList)
        {
            i++;
        }
    }
    
    public static void CaliContrastAllowed()
    {
        IsCaliContrast = SrLib.CheckUnityPerMission(@"C:\RPBANDRAABLog\CaliContrast.csv");

        string fNm = @"C:\RPBANDRAABLog\CaliContrast.csv";
        StreamReader sr = new StreamReader(fNm);
        if (File.Exists(fNm))
            IsCaliContrast = true;
        else
            IsCaliContrast = false;
    }
    

    public static List<OneDot> RndDotDataList(List<string> RawData)
    {
        List<OneDot> TmpLst = new List<OneDot>();
        if(RawData.Count > 0)
        {
            for(int n = 2; n < RawData.Count; n ++)
            {
                string str = RawData[n];
                OneDot tmpD = new OneDot();
                string[] Oned = str.Split(';');
                tmpD.Cnt = Convert.ToInt32( Oned[0]);
                tmpD.X = Convert.ToSingle(Oned[1]);
                tmpD.Y = Convert.ToSingle(Oned[2]);
                TmpLst.Add(tmpD);
            }
        }
        return TmpLst;
    }
    

    public static void CheckRndDotList(List<OneDot> InList)
    {
        foreach(OneDot dt in InList)
        {
            string str = "Cnt = " + dt.Cnt.ToString() + " ; " + dt.X.ToString() + " ; " + dt.Y.ToString();
        }
    }
    
    public static void WriteLogData(string fName, string test)
    {
        if(test == "Reading")
        {
            TestLoglist.Add(readingMissInfo);
        }
    }
    
    public static void WriteTrackLogData(string fName, string test)
    {
        if (test == "Reading")
        {
            TestDataStringList.Add(readingMissInfo);
        }

        string str = SrLib.WriteToLogFile(fName, TestDataStringList);
    }
    
    public static void CollectEyeInfo()
    {
        double xl = 0, yl = 0, zl = 0, xr = 0, yr = 0, zr = 0;
        //Dll_Eye.GetXY(ref xl, ref yl, ref zl, ref xr, ref yr, ref zr);
        OneE CurrD = new OneE();
        CurrD.Left.x = Convert.ToSingle(xl);
        CurrD.Left.y = Convert.ToSingle(yl);
        CurrD.Left.z = Convert.ToSingle(zl);
        CurrD.Right.x = Convert.ToSingle(xr);
        CurrD.Right.y = Convert.ToSingle(yr);
        CurrD.Right.z = Convert.ToSingle(zr);
        CurrD.TmStp = DateTime.Now;
        Variables.TestDataList.Add(CurrD);
    }

    public static void FeedOneData(string StrN)
    {
        DataToString(TestDataList);

        if (StrN == "Reading_Eye")
        {
            //this for loop should check the RPBANDRAABLog for files that match the description. Once it finds one that doesn't exist it should stop. And assign the nonexisting number as the new counter so this next file will be created.
            for (int i = 1; ; i++)
            {
                if (!File.Exists(@"C:\RPBANDRAABLog\" + StrN + "_" + i + Variables.LogNamePart + ".csv"))
                {
                    Variables.counterForTest = i;
                    break;
                }
            }
            WriteTrackLogData(@"C:\RPBANDRAABLog\" + StrN + "_" + Variables.counterForTest + Variables.LogNamePart + ".csv", "Reading");
        }
        if (StrN == "DotCount_Eye") {

            for (int i = 1; ; i++)
            {
                if (!File.Exists(@"C:\RPBANDRAABLog\" + StrN + "_" + i + Variables.LogNamePart + ".csv"))
                {
                    Variables.counterForTest = i;
                    break;
                }
            }
            WriteTrackLogData(@"C:\RPBANDRAABLog\" + StrN + "_" + Variables.counterForTest + Variables.LogNamePart + ".csv", "wDotCount_Eye");
        }
        //Added this a Dot Orbit version so it will get its own excel files seperate from the others. 
        if (StrN == "DotOrbit_Eye")
        {
            for (int i = 1; ; i++)
            {
                if (!File.Exists(@"C:\RPBANDRAABLog\" + StrN + "_" + i + Variables.LogNamePart + ".csv"))
                {
                    Variables.counterForTest = i;
                    break;
                }
            }
            WriteTrackLogData(@"C:\RPBANDRAABLog\" + StrN + "_" + Variables.counterForTest + Variables.LogNamePart + ".csv", "DotOrbit_Eye");
        }
    }
    
    

    private static void DataToString(List<OneE> Dlist)
    {
        TestDataStringList.Clear();

        TestDataStringList.Add(BasicTestInfo);
        TestDataStringList.Add(TestiInfo);

        if (IsDotCount)
        {
            Variables.TestDataStringList.Add("Cnt,Random Dot X, Y, Z,Coord Set Used," +SetUsed);//Changed

            foreach (string dotL in RndDotLog)
            {
                Variables.TestDataStringList.Add(dotL);
            }
        }
        //Code needed to get the eye tracking data at row 25 for every test
        string empty = "";
        string TrackText = "Left Eye X,Y,Z,Right Eye X,Y,Z,Time in Millisecond";
        TestDataStringList.Add(TrackText);
        int index = TestDataStringList.IndexOf(TrackText);
        for (int j = 0; TestDataStringList.IndexOf(TrackText) != 22; j++) {
            TestDataStringList.Insert(index, empty);
            
        }
        
            
        //TestDataStringList.Add(index); //To get eye tracking data in row 25 need TrackText in index 22.
            
        int i, totalNum;
        totalNum = Dlist.Count;
        //Eye tracking data gets added here
        string str = (-Dlist[0].Left.x).ToString("N3") + "," +
            Dlist[0].Left.y.ToString("N3") + "," +
            Dlist[0].Left.z.ToString("N3") + "," +
            (-Dlist[0].Right.x).ToString("N3") + "," +
            Dlist[0].Right.y.ToString("N3") + "," +
            Dlist[0].Right.z.ToString("N3") + "," +
            Dlist[0].TmStp.ToString("HH:mm:ss.fff");

        for (i = 1; i < totalNum; i ++)
        {
            str = (-Dlist[i].Left.x).ToString("N3") + "," +
            Dlist[i].Left.y.ToString("N3") + "," +
            Dlist[i].Left.z.ToString("N3") + "," +
            (-Dlist[i].Right.x).ToString("N3") + "," +
            Dlist[i].Right.y.ToString("N3") + "," +
            Dlist[i].Right.z.ToString("N3") + "," +
            (Dlist[i].TmStp.Subtract(Dlist[0].TmStp).TotalMilliseconds).ToString("F0");           
            TestDataStringList.Add(str);
        }

        TestDataStringList.Add(TestLoglist[TestLoglist.Count - 1]);                 
    }
    

  
    public static string CalculateTestTimeDuration()
    {
       TimeSpan durationT = eT - sT;
        return (durationT.TotalSeconds).ToString("0000.000");
    }
    

    public static List<string> InputMinDistList(string fName)
    {
        List<string> TmpList = new List<string>();
        PreLoadDotFileList.Clear();
        TmpList = SrLib.ReadFileFromFile(fName);
        return TmpList;
    }
    
    public static List<OneDot> ProcessStringToOneDotList(List<string> StrList)
    {
        List<OneDot> tmpList = new List<OneDot>();

        if (StrList.Count > 0)
        {
            for (int n = 3; n < StrList.Count; n++)
            {
                string str = StrList[n];
                OneDot tmpD = new OneDot();
                string[] Oned = str.Split(';');
                tmpD.Cnt = Convert.ToInt32(Oned[0]);
                tmpD.X = Convert.ToSingle(Oned[1]);
                tmpD.Y = Convert.ToSingle(Oned[2]);
                tmpList.Add(tmpD);
            }
        }

        return tmpList;
    }
    

    public static void LoadPresetRndListTolist()
    {
        MinDistFileNameList();

        PreLoadDotFileList.Clear();
        MinDist80.Clear();
        PreLoadDotFileList = InputMinDistList(MinDistFileList[0]);
        MinDist80 = ProcessStringToOneDotList(PreLoadDotFileList);
        PreLoadDotFileList.Clear();
        MinDist60.Clear();
        PreLoadDotFileList = InputMinDistList(MinDistFileList[1]);
        MinDist60 = ProcessStringToOneDotList(PreLoadDotFileList);
        PreLoadDotFileList.Clear();
        MinDist40.Clear();
        PreLoadDotFileList = InputMinDistList(MinDistFileList[2]);
        MinDist40 = ProcessStringToOneDotList(PreLoadDotFileList);
        PreLoadDotFileList.Clear();
        MinDist20.Clear();
        PreLoadDotFileList = InputMinDistList(MinDistFileList[3]);
        MinDist20 = ProcessStringToOneDotList(PreLoadDotFileList);
        

    }
    

}

