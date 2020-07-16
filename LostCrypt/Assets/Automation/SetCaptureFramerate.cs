using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCaptureFramerate : MonoBehaviour
{
    [Tooltip("Frame rate used for simulation, should be customized per project. (maps to Time.captureFramerate)")]
    public int simulationFrameRate = 10;
    [Tooltip("The actual frame rate to render, should be as high as possible to cap test device limits. (maps to Application.targetFrameRate)")]
    public int targetFrameRate = 120;
    [Tooltip("Screen width")]
    public int screenWidth = 1280;
    [Tooltip("Screen height")]
    public int screenHeight = 720;

    private void Awake()
    {
        Time.captureFramerate = simulationFrameRate;
        Application.targetFrameRate = targetFrameRate;
        Screen.SetResolution(screenWidth, screenHeight, true);
    }
}
