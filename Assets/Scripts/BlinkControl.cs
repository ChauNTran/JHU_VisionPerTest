using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkControl : MonoBehaviour
{
    public Color startColor = Color.red;
    public Color endColor = Color.white;
    public GameObject Cb;

    [Range(0, 10)]
    public float speed = 1;

    Renderer ren;

    public void Awake()
    {
        ren = GetComponent<Renderer>();
        Variables.IsStart = false;
        Cb = GameObject.Find("Cube");
    }

    private void Update()
    {
        if (Variables.IsReadingStart)
        {
            ren.material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 2));
        }
    }

}
