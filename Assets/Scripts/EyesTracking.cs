using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.Assertions;
using Varjo.XR;
using RenderHeads.Media.AVProMovieCapture;
public enum GazeDataSource
{
    InputSubsystem,
    GazeAPI
}
[Serializable]
public struct EyesData
{
    public EyesData(Vector3 GazeOriginCombinedLocal, Vector3 GazeDirectionCombinedLocal)
    {
        this.GazeOriginCombinedLocal = GazeOriginCombinedLocal;
        this.GazeDirectionCombinedLocal = GazeDirectionCombinedLocal;
    }
    public Vector3 GazeOriginCombinedLocal;
    public Vector3 GazeDirectionCombinedLocal;
}
public class EyesTracking : MonoBehaviour
{
    //[Header("Top Down View")]
    //[SerializeField] private RenderTexture HeadsetTexture;
    //[SerializeField] private RenderTexture TopdownTexture;
    //[SerializeField] private Camera monitorCamera;
    //[SerializeField] private Camera retinaCamera;

    [Header("Capture")]
    [SerializeField] private CaptureFromScreen capture;

    //[Header("Gaze ray radius")]
    //public float gazeRadius = 0.01f;
    //[Header("Gaze target offset towards viewer")]
    //public float targetOffset = 0.2f;
    [Header("Gaze point distance if not hit anything")]
    public float floatingGazeTargetDistance = 5f;
    public GameObject gazeTarget;

    [Header("Visualization Transforms")]
    public Transform fixationPointTransform;
    public Transform leftEyeTransform;
    public Transform rightEyeTransform;

    [Header("XR camera")]
    public Camera xrCamera;

    [Header("Gaze data")]
    public GazeDataSource gazeDataSource = GazeDataSource.InputSubsystem;

    public bool isTracking
    {
        get
        {
            return VarjoEyeTracking.IsGazeAllowed() && VarjoEyeTracking.IsGazeCalibrated();
        }
    }

    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;
    private Eyes eyes; // unity's
    public VarjoEyeTracking.GazeData gazeData { get; private set; }

    private List<VarjoEyeTracking.GazeData> dataSinceLastUpdate;
    private List<VarjoEyeTracking.EyeMeasurements> eyeMeasurementsSinceLastUpdate;
    private StreamWriter writer = null;

    private Vector3 leftEyePosition;
    private Vector3 rightEyePosition;
    private Quaternion leftEyeRotation;
    private Quaternion rightEyeRotation;
    private Vector3 fixationPoint;
    private Vector3 direction;
    private Vector3 rayOrigin;
    private bool logging = false;
    private const string ValidString = "VALID";
    private const string InvalidString = "INVALID";
    private static readonly string[] ColumnNames = { "Frame", "CaptureTime", "LogTime", "HMDPosition",
        "HMDRotation", "GazeStatus", "CombinedGazeForward", "CombinedGazePosition", "InterPupillaryDistanceInMM",
        "LeftEyeStatus", "LeftEyeForward", "LeftEyePosition", "LeftPupilIrisDiameterRatio", "LeftPupilDiameterInMM",
        "LeftIrisDiameterInMM", "RightEyeStatus", "RightEyeForward", "RightEyePosition", "RightPupilIrisDiameterRatio",
        "RightPupilDiameterInMM", "RightIrisDiameterInMM", "FocusDistance", "FocusStability" };

    private string root;
    private string dataPath;
    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(XRNode.CenterEye, devices);
        device = devices.FirstOrDefault();
    }



    private void Start()
    {
        root = Directory.GetCurrentDirectory();
        if (!device.isValid)
        {
            GetDevice();
        }
        dataPath = root + "\\EyesTrackingLogs\\";
        if (!Directory.Exists(dataPath))
            Directory.CreateDirectory(dataPath);
        XRSettings.gameViewRenderMode = GameViewRenderMode.None;
    }
    public void StartLogging()
    {
        if (logging)
        {
            Debug.LogWarning("Logging was on when StartLogging was called. No new log was started.");
            return;
        }

        logging = true;

        DateTime now = DateTime.Now;
        string fileName = string.Format("{0}-{1:00}-{2:00}-{3:00}-{4:00}", now.Year, now.Month, now.Day, now.Hour, now.Minute);

        string path = dataPath + fileName + ".csv";
        writer = new StreamWriter(path);

        Log(ColumnNames);
        Debug.Log("Log file started at: " + path);
    }
    public void StopLogging()
    {
        if (!logging)
            return;

        if (writer != null)
        {
            writer.Flush();
            writer.Close();
            writer = null;
        }
        logging = false;
        Debug.Log("Logging ended");

    }

    public void StartCapture()
    {
        capture.StartCapture();
    }
    public void StopCapture()
    {
        capture.StopCapture();
    }

    private void Update()
    {
        if (isTracking)
        {
            //Get device if not valid
            if (!device.isValid)
            {
                GetDevice();
            }

            // Show gaze target
            gazeTarget.SetActive(true);

            if (gazeDataSource == GazeDataSource.InputSubsystem)
            {
                // Get data for eye positions, rotations and the fixation point
                if (device.TryGetFeatureValue(CommonUsages.eyesData, out eyes))
                {
                    if (eyes.TryGetLeftEyePosition(out leftEyePosition))
                    {
                        leftEyeTransform.localPosition = leftEyePosition;
                    }

                    if (eyes.TryGetLeftEyeRotation(out leftEyeRotation))
                    {
                        leftEyeTransform.localRotation = leftEyeRotation;
                    }

                    if (eyes.TryGetRightEyePosition(out rightEyePosition))
                    {
                        rightEyeTransform.localPosition = rightEyePosition;
                    }

                    if (eyes.TryGetRightEyeRotation(out rightEyeRotation))
                    {
                        rightEyeTransform.localRotation = rightEyeRotation;
                    }

                    if (eyes.TryGetFixationPoint(out fixationPoint))
                    {
                        fixationPointTransform.localPosition = fixationPoint;
                    }
                }

                // Set raycast origin point to VR camera position
                rayOrigin = xrCamera.transform.position;

                // Direction from VR camera towards fixation point
                direction = (fixationPointTransform.position - xrCamera.transform.position).normalized;

            }
            else
            {
                gazeData = VarjoEyeTracking.GetGaze();

                if (gazeData.status != VarjoEyeTracking.GazeStatus.Invalid)
                {
                    // GazeRay vectors are relative to the HMD pose so they need to be transformed to world space
                    if (gazeData.leftStatus != VarjoEyeTracking.GazeEyeStatus.Invalid)
                    {
                        leftEyeTransform.position = xrCamera.transform.TransformPoint(gazeData.left.origin);
                        leftEyeTransform.rotation = Quaternion.LookRotation(xrCamera.transform.TransformDirection(gazeData.left.forward));
                    }

                    if (gazeData.rightStatus != VarjoEyeTracking.GazeEyeStatus.Invalid)
                    {
                        rightEyeTransform.position = xrCamera.transform.TransformPoint(gazeData.right.origin);
                        rightEyeTransform.rotation = Quaternion.LookRotation(xrCamera.transform.TransformDirection(gazeData.right.forward));
                    }

                    // Set gaze origin as raycast origin
                    rayOrigin = xrCamera.transform.TransformPoint(gazeData.gaze.origin);

                    // Set gaze direction as raycast direction
                    direction = xrCamera.transform.TransformDirection(gazeData.gaze.forward);

                    // Fixation point can be calculated using ray origin, direction and focus distance
                    fixationPointTransform.position = rayOrigin + direction * gazeData.focusDistance;

                }
            }
        }

        // If gaze ray didn't hit anything, the gaze target is shown at fixed distance
        gazeTarget.transform.position = rayOrigin + direction * floatingGazeTargetDistance;
        gazeTarget.transform.LookAt(rayOrigin, Vector3.up);
        gazeTarget.transform.localScale = Vector3.one * floatingGazeTargetDistance;

        if (logging)
        {
            int dataCount = VarjoEyeTracking.GetGazeList(out dataSinceLastUpdate, out eyeMeasurementsSinceLastUpdate);

            for (int i = 0; i < dataCount; i++)
            {
                LogGazeData(dataSinceLastUpdate[i], eyeMeasurementsSinceLastUpdate[i]);
            }
        }
    }

    void LogGazeData(VarjoEyeTracking.GazeData data, VarjoEyeTracking.EyeMeasurements eyeMeasurements)
    {
        string[] logData = new string[23];

        // Gaze data frame number
        logData[0] = data.frameNumber.ToString();

        // Gaze data capture time (nanoseconds)
        logData[1] = data.captureTime.ToString();

        // Log time (milliseconds)
        logData[2] = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString();

        // HMD
        logData[3] = xrCamera.transform.localPosition.ToString("F3");
        logData[4] = xrCamera.transform.localRotation.ToString("F3");

        // Combined gaze
        bool invalid = data.status == VarjoEyeTracking.GazeStatus.Invalid;
        logData[5] = invalid ? InvalidString : ValidString;
        logData[6] = invalid ? "" : data.gaze.forward.ToString("F3");
        logData[7] = invalid ? "" : data.gaze.origin.ToString("F3");

        // IPD
        logData[8] = invalid ? "" : eyeMeasurements.interPupillaryDistanceInMM.ToString("F3");

        // Left eye
        bool leftInvalid = data.leftStatus == VarjoEyeTracking.GazeEyeStatus.Invalid;
        logData[9] = leftInvalid ? InvalidString : ValidString;
        logData[10] = leftInvalid ? "" : data.left.forward.ToString("F3");
        logData[11] = leftInvalid ? "" : data.left.origin.ToString("F3");
        logData[12] = leftInvalid ? "" : eyeMeasurements.leftPupilIrisDiameterRatio.ToString("F3");
        logData[13] = leftInvalid ? "" : eyeMeasurements.leftPupilDiameterInMM.ToString("F3");
        logData[14] = leftInvalid ? "" : eyeMeasurements.leftIrisDiameterInMM.ToString("F3");

        // Right eye
        bool rightInvalid = data.rightStatus == VarjoEyeTracking.GazeEyeStatus.Invalid;
        logData[15] = rightInvalid ? InvalidString : ValidString;
        logData[16] = rightInvalid ? "" : data.right.forward.ToString("F3");
        logData[17] = rightInvalid ? "" : data.right.origin.ToString("F3");
        logData[18] = rightInvalid ? "" : eyeMeasurements.rightPupilIrisDiameterRatio.ToString("F3");
        logData[19] = rightInvalid ? "" : eyeMeasurements.rightPupilDiameterInMM.ToString("F3");
        logData[20] = rightInvalid ? "" : eyeMeasurements.rightIrisDiameterInMM.ToString("F3");

        // Focus
        logData[21] = invalid ? "" : data.focusDistance.ToString();
        logData[22] = invalid ? "" : data.focusStability.ToString();

        Log(logData);
    }
    void Log(string[] values)
    {
        if (!logging || writer == null)
            return;

        string line = "";
        for (int i = 0; i < values.Length; ++i)
        {
            values[i] = values[i].Replace("\r", "").Replace("\n", ""); // Remove new lines so they don't break csv
            line += values[i] + (i == (values.Length - 1) ? "" : ";"); // Do not add semicolon to last data string
        }
        writer.WriteLine(line);
    }
}
