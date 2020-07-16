using UnityEditor;
using UnityEngine;
using System;
using System.IO;

public class ScreenshotTaker
{
    //[MenuItem("Tools/Take Screenshot")]
    public static void TakeScreenshot()
    {
        if (!Directory.Exists("Screenshots"))
            Directory.CreateDirectory("Screenshots");

        ScreenCapture.CaptureScreenshot(string.Format("Screenshots/{0}.png", DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss")));
    }
}

