using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityServe;
using UnityEngine.SceneManagement;

public class MasterScrpt : MonoBehaviour
{
    public static MasterScrpt Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        //StartCoroutine(LoadEyeTracking());
    }

    IEnumerator LoadEyeTracking()
    {
        //ServeLib sLib = new ServeLib();
        //Variables.dtcBool = sLib.CKSRanipalVer();

        //if (Variables.dtcBool)
        //{
        //    Dll_Eye.InitViveSR();
        //}
        yield return new WaitForEndOfFrame();
        //if (!sLib.ChaWenJian())
        //Application.Quit();
    }
}
