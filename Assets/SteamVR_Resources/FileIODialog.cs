using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class FileIODialog : MonoBehaviour
{

    [StructLayout(LayoutKind.Sequential,CharSet= CharSet.Auto)]
    public class FileIODlg : OpenFileDlg
    {
    }
    

    public class LoadFileDialog
    {
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetOpenFileName([In, Out] FileIODlg openFilePath);
    }


    public class SaveFileDlg : OpenFileDlg
    {
    }

    public class SaveFileDialog
    {
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetSaveFileName([In, Out] SaveFileDlg saveFilePath);
    }

    [StructLayout(LayoutKind.Sequential,CharSet=CharSet.Auto)]
    public class OpenFileDlg
    {
        public int structSize = 0;
    }
}
