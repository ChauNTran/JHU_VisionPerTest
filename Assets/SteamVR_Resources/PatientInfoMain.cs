using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatientInfoMain : MonoBehaviour
{
    public InputField MinDist;
    void Start()
    {
        MinDist.text = Variables.MinDistValue.ToString();
    }

    public void LoadDefaultValue()
    {

    }
}
