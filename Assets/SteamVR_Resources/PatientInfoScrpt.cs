using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/**
README

Alex's update 7/12/23
    1. enable adding experimenter's name

**/

public class PatientInfoScrpt : MonoBehaviour
{

    public InputField _input;
    GameObject entTxt;

    public void UpdateInputInfo()
    {
        entTxt = GameObject.Find("enteredTxt");

        entTxt.GetComponent<Text>().text = "Patient Name " + Variables.PatientName.ToString() +
                                            "\r\nExperimenter Name " + Variables.ExperimenterName.ToString();//Added ToString stuff to the Patient Name



        Variables.BuildLogFileNamePart();

        Variables.IsDotCount = false;
        Variables.IsReading = false;
        Variables.LoadPresetRndListTolist();
    }

    public void ReadInputPName()
    {
        if (_input.text != "")
        {
            Variables.PatientName = _input.text;
        }
    }

    public void ReadInputEName()
    {
        if (_input.text != "")
        {
            Variables.ExperimenterName = _input.text;
        }
    } 



    public void ReadDotSize()
    {
        if (_input.text != "")
        {
            Variables.DotSize = Convert.ToInt32(_input.text);
        }
    }

    public void ReadDotNumber()
    {
        if (_input.text != "")
        {
            Variables.DotNum = Convert.ToInt32(_input.text);
            if (Variables.DotNum <= 9)
                Variables.MinDistValue = 80;
            else if (9 < Variables.DotNum && Variables.DotNum <= 12)
                Variables.MinDistValue = 60;
            else if (12 < Variables.DotNum && Variables.DotNum <= 18)
                Variables.MinDistValue = 40;
            else if (18 < Variables.DotNum && Variables.DotNum <= 35)
                Variables.MinDistValue = 20;
            else if (Variables.DotNum > 35)
            {
                Variables.MinDistValue = 20;
                Variables.DotNum = 35;              // force to max dot number and min dot distance. can not exceed this number.
            }
        }
    }

    public void ReadMiniDistanceValue()
    {
        if (_input.text != "")
            Variables.MinDistValue = Convert.ToSingle(_input.text);
    }
}
