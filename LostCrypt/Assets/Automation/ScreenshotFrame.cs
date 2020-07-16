using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotFrame : MonoBehaviour
{

    public bool continuousScreenshots = false;
    public int screenshotFrameInterval = 1;
    public int frameToScreenshot = 10;
    public string screenshotFolderName = "Screenshots";

    DateTime startTime;
    
    void Start()
    {
        startTime = DateTime.Now;

        if (!System.IO.Directory.Exists(screenshotFolderName))
        {
            System.IO.Directory.CreateDirectory(screenshotFolderName);
        }
        
        System.IO.Directory.CreateDirectory($"{screenshotFolderName}/{startTime.ToString("yyyy-MM-dd_hh-mm-ss")}");
    }

    // Update is called once per frame
    void Update()
    {
        if (continuousScreenshots)
        {
            if (Time.frameCount % screenshotFrameInterval == 0)
            {
                ScreenCapture.CaptureScreenshot(
                    $"{screenshotFolderName}/{startTime.ToString("yyyy-MM-dd_hh-mm-ss")}/frame-{Time.frameCount}.png");
            }
        }
        else
        {
            if (Time.frameCount == frameToScreenshot)
            {
                ScreenCapture.CaptureScreenshot(
                    $"{screenshotFolderName}/{startTime.ToString("yyyy-MM-dd_hh-mm-ss")}/frame-{Time.frameCount}.png");
            }
        }
    }
}
