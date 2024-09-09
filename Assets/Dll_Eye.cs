using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

//public static class Dll_Eye : MonoBehaviour
public static class Dll_Eye
{

    [DllImport("SR_EyeTrk-Dll")]
    public static extern bool ReleaseEngine();

    [DllImport("SR_EyeTrk-Dll")]
    public static extern bool InitViveSR();

    [DllImport("SR_EyeTrk-Dll")]
    // get eye data
    public static extern void GetXY(ref double LX, ref double LY, ref double LZ, ref double RX, ref double RY, ref double RZ);

    // Start is called before the first frame update


    // call SRanipal eye calibration exe
    [DllImport("SRanipal")]
    public static extern int LaunchEyeCalibration(IntPtr callback);

    public static void testLog()
    {
        Debug.Log("Press button");
    }
}
