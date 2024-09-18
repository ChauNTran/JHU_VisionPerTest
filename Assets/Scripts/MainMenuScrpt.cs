using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScrpt : MonoBehaviour
{
    public void GotoTestScene()
    {
        GotoTestScene(this.gameObject.name);
    }

    public void GotoTestScene(string sceneName)
    {
        switch (sceneName)
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
        SceneManager.LoadScene("MainMenuScene");
        Variables.IsDotCount = false;
        Variables.IsReading = false;
        Variables.counterForTest = 1;//New Stuff
    }

}
