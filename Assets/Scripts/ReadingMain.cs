using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityServe;

public class ReadingMain : MonoBehaviour
{
    [SerializeField] private AudioClip ReadingInstructionAudio;
    [SerializeField] private AudioClip StartSound;
    [SerializeField] private AudioClip FinishSound;
    [SerializeField] private AudioClip DataSavedSound;
    [SerializeField] private GameObject ShowImg;
    [SerializeField] private EyesTracking eyesTracking;

    private AudioSource PlySound;

    void Start()
    {
        PlySound = GetComponent<AudioSource>();

        Variables.IsReadingInstru = false;
        Variables.IsReadingStart = false;
        Variables.IsReadingEnd = false;
        Variables.IsDataSave = false;
        Variables.IsReadingTracking = false;
        //Dll_Eye.InitViveSR();

        // Look for eyestracking object

    }

    void Update()
    {
        if (Variables.IsReadingInstru)
        {
            PlySound.clip = ReadingInstructionAudio;
            PlySound.Play();
            Variables.IsReadingInstru = false;
        }
        

        if (Variables.IsReadingStart && eyesTracking.isTracking)
        {
            PlySound.clip = StartSound;
            PlySound.Play();
            Variables.IsReadingStart = false;
            Variables.CollectEyeInfo(
                eyesTracking.gazeData.left.forward.x,
                eyesTracking.gazeData.left.forward.y,
                eyesTracking.gazeData.left.forward.z,
                eyesTracking.gazeData.right.forward.x,
                eyesTracking.gazeData.right.forward.y,
                eyesTracking.gazeData.right.forward.z
            );
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && Variables.IsReadingTracking)
        {
            DestroyLine(Variables.LineCnt);
            if (Variables.WhichBtnClick == 1)
            {
                Variables.MissWord--;
                Variables.WhichBtnClick = 0;
            }
            if (Variables.WhichBtnClick == -1)
            {
                Variables.WrongWord--;
                Variables.WhichBtnClick = 0;
            }
        }
        

        if (Variables.IsReadingTracking && eyesTracking.isTracking)
        {
            Variables.CollectEyeInfo(
                eyesTracking.gazeData.left.forward.x,
                eyesTracking.gazeData.left.forward.y,
                eyesTracking.gazeData.left.forward.z,
                eyesTracking.gazeData.right.forward.x,
                eyesTracking.gazeData.right.forward.y,
                eyesTracking.gazeData.right.forward.z
           );
        }


        if (Input.GetMouseButtonDown(0) && Variables.IsReadingTracking)
        {
            Variables.WhichBtnClick = 1;
            Vector3 mousePos = Input.mousePosition;
            if (mousePos.x < 2000)
            {
                Vector3 currPos = mousePos - transform.position;
                AddOneLine(currPos, "Left");
                Variables.LineCnt++;
                Variables.MissWord++;
            }
        }
        

        if (Input.GetMouseButtonDown(1) && Variables.IsReadingTracking)
        {
            Variables.WhichBtnClick = -1;
            Vector3 mPo = Input.mousePosition;
            if (mPo.x < 2000)
            {
                Vector3 currPos = mPo - transform.position;
                AddOneLine(currPos, "Right");
                Variables.LineCnt++;
                Variables.WrongWord++;
            }
        }
        

        if (Input.GetKeyDown(KeyCode.S))
        {
            LoadImgControl.ScreenShotSave();
        }
        

        if (Variables.IsReadingEnd)
        {
            PlySound.clip = FinishSound;
            PlySound.Play();
            Variables.IsReadingEnd = false;
        }

        if (Variables.IsDataSave)
        {
            PlySound.clip = DataSavedSound;
            PlySound.Play();
            Variables.IsDataSave = false;
        }
        
    }


    public void TestRecording()
    {
        AudioClip reco;
        AudioSource aud;
        aud = GetComponent<AudioSource>();
        foreach(string device in Microphone.devices)
        {
        }
        reco = Microphone.Start("Built-in Microphone", true, 30, 44100);
        aud.clip = reco;
        
    }
    

    private void AddOneLine(Vector3 Pos, string lineN)
    {
        GameObject nLine = new GameObject(Variables.lineNamePre + Variables.LineCnt);
        Variables.Marks.Add(nLine);
        nLine.transform.parent = ShowImg.transform;
        nLine.AddComponent<Image>();
        if (lineN == "Left")
            nLine.GetComponent<Image>().color = Color.red;
        if (lineN == "Right")
            nLine.GetComponent<Image>().color = Color.blue;

        nLine.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 3);
        nLine.transform.Rotate(0f, 0f, 30f);
        nLine.transform.position = Pos;
    }
    


    private void DestroyLine(int i)
    {
        string str = Variables.lineNamePre + (i - 1);
        GameObject obj = GameObject.Find(str);
        Destroy(obj);
    }
    

}
