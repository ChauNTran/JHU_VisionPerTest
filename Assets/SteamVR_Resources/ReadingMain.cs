﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityServe;

public class ReadingMain : MonoBehaviour
{
    private AudioSource PlySound;
    private GameObject RImage;
    ServeLib sLib = new ServeLib();

    void Start()
    {

        Variables.IsReadingInstru = false;
        Variables.IsReadingStart = false;
        Variables.IsReadingEnd = false;
        Variables.IsDataSave = false;
        Variables.IsReadingTracking = false;

        GameObject ReadingInfoBoard;
        ReadingInfoBoard = GameObject.Find("InfoBoard");
        ReadingInfoBoard.GetComponent<Text>().text = "Reading Test";
        Dll_Eye.InitViveSR();
        RImage = GameObject.Find("ShowImg");
        GameObject rawImg = GameObject.Find("RawImage");
        float x, y;
        x = rawImg.GetComponent<RawImage>().rectTransform.rect.width;
        y = rawImg.GetComponent<RawImage>().rectTransform.rect.height;

        
        GameObject conTBd = GameObject.Find("ControlBoard");
        conTBd.GetComponent<RawImage>().rectTransform.position = new Vector2( x - 180, 400);
       
    }

    void Update()
    {
        if (Variables.IsReadingInstru)
        {
            PlySound = GetComponent<AudioSource>();
            PlySound.clip = Resources.Load<AudioClip>("ReadingInstruction");
            PlySound.Play();
            Variables.IsReadingInstru = false;
        }
        

        if (Variables.IsReadingStart)
        {
            PlySound = GetComponent<AudioSource>();
            PlySound.clip = Resources.Load<AudioClip>("Start2");
            PlySound.Play();
            Variables.IsReadingStart = false;
            Variables.CollectEyeInfo();             
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
        

        if (Variables.IsReadingTracking)
            Variables.CollectEyeInfo();             
        

        if (Input.GetMouseButtonDown(0) && Variables.IsReadingTracking)
        {
            Variables.WhichBtnClick = 1;
            Vector3 mousePos = Input.mousePosition;
            if (mousePos.x < 1500)
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
            if (mPo.x < 1500)
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
            PlySound = GetComponent<AudioSource>();
            PlySound.clip = Resources.Load<AudioClip>("Finish");
            PlySound.Play();
            Variables.IsReadingEnd = false;
        }

        if (Variables.IsDataSave)
        {
            PlySound = GetComponent<AudioSource>();
            PlySound.clip = Resources.Load<AudioClip>("DataSaved");
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
        nLine.transform.parent = RImage.transform;
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
