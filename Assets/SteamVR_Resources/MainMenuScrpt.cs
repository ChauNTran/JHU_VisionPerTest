﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScrpt : MonoBehaviour
{
    public void GotoTestScene()
    {
        switch(this.gameObject.name)
        {
            case "ReadParagBtn":
                SceneManager.LoadScene("ReadingTestScene");
                Variables.HelpBtnTrueOrFalse = false;//This was newly added to make sure the help button always spawns stuff on a click and never erases stuff on the first click
                break;
            case "DotCountBtn":
                //SceneManager.LoadScene("DotCountScene");
                SceneManager.LoadScene("DotCountSceneNoInput");
                Variables.HelpBtnTrueOrFalse = false;//This was newly added to make sure the help button always spawns stuff on a click and never erases stuff on the first click
                break;
            case "DotCountNormalBtn":
                SceneManager.LoadScene("DotCountScene");
                //SceneManager.LoadScene("DotCountSceneNoInput");
                Variables.HelpBtnTrueOrFalse = false;//This was newly added to make sure the help button always spawns stuff on a click and never erases stuff on the first click
                break;
            case "FindObjBtn":
                SceneManager.LoadScene("ObjectFindScene");
                break;
            case "PatientInfoBtn":
                SceneManager.LoadScene("InfoScene");
                break;
            case "CaliContrastBtn":
                SceneManager.LoadScene("CaliContrast");
                break;      
            case "QuitBtn":
                Application.Quit();
                break;
            case "OrbitBtn":
                SceneManager.LoadScene("OrbitScene");
                break;
        }   
    }


    public void BackToMainMenu()
    {
        Variables.IsDotCount = false;
        Variables.IsReading = false;
        SceneManager.LoadScene("MainMenuScene");
        Variables.counterForTest = 1;//New Stuff
        DotControlBoard.RoundCount = 0;//New Stuff 10_17_23
    }

}
