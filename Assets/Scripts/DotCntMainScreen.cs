using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityServe;

public class DotCntMainScreen : MonoBehaviour
{
    [SerializeField] private EyesTracking eyesTracking;
    private AudioSource PlySound;
    private static List<GameObject> NewDots;

    void Start()
    {
        Variables.IsCountStart = false;
        Variables.MinDistFileNameList();
    }

    public void Update()
    {

        if(Variables.PlayDotCount)
        {
            PlySound = GetComponent<AudioSource>();
            PlySound.clip = Resources.Load<AudioClip>("CountDots");
            PlySound.Play();
            Variables.PlayDotCount = false;
        }

        if(Variables.PlayDotStart)
        {
            PlySound = GetComponent<AudioSource>();
            PlySound.clip = Resources.Load<AudioClip>("Start2");
            PlySound.Play();
            Variables.PlayDotStart = false;
        }
        
        if (Variables.PlayDotStop)
        {
            PlySound = GetComponent<AudioSource>();
            PlySound.clip = Resources.Load<AudioClip>("Stop1");
            PlySound.Play();
            Variables.PlayDotStop = false;
        }
        

        if (Variables.PlayFinish)
        {
            PlySound = GetComponent<AudioSource>();
            PlySound.clip = Resources.Load<AudioClip>("Finish");
            PlySound.Play();
            Variables.PlayFinish = false;
        }

        if (Variables.IsCountStart && eyesTracking.isTracking)
            Variables.CollectEyeInfo(
                eyesTracking.gazeData.left.forward.x,
                eyesTracking.gazeData.left.forward.y,
                eyesTracking.gazeData.left.forward.z,
                eyesTracking.gazeData.right.forward.x,
                eyesTracking.gazeData.right.forward.y,
                eyesTracking.gazeData.right.forward.z
           );
    }


    public static void GenerateNewDots()
    {
        NewDots = new List<GameObject>();

        if (Variables.DotNum > 0)
        {
            NewDots.Clear();
        }
    }

    public static void CleanScrn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameObject MnBdImg;
        MnBdImg = GameObject.Find("MainBoard");
        MnBdImg.transform.position = new Vector3(MnBdImg.transform.position.x, MnBdImg.transform.position.y, -100f);

    }
}

