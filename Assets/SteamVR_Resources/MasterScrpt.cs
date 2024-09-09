using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityServe;

public class MasterScrpt : MonoBehaviour
{
    void Start()
    {
        ServeLib sLib = new ServeLib();
        Variables.dtcBool = sLib.CKSRanipalVer();

        if (Variables.dtcBool)
        {
            Dll_Eye.InitViveSR();
        }

        //if (!sLib.ChaWenJian())
            //Application.Quit();
    }

    void Update()
    {
        
    }
}
