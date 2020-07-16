using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScreenshotFrame))]
[CanEditMultipleObjects]
public class ScreenshotFrameEditor : Editor
{
    SerializedProperty continuousScreenshots;
    SerializedProperty screenshotFrameInterval;
    SerializedProperty frameToScreenshot;
    SerializedProperty screenshotFolderName;
    
    void OnEnable()
    {
        continuousScreenshots = serializedObject.FindProperty("continuousScreenshots");
        screenshotFrameInterval = serializedObject.FindProperty("screenshotFrameInterval");
        frameToScreenshot = serializedObject.FindProperty("frameToScreenshot");
        screenshotFolderName = serializedObject.FindProperty("screenshotFolderName");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(continuousScreenshots);
        
        EditorGUI.BeginDisabledGroup(!continuousScreenshots.boolValue);
        EditorGUILayout.PropertyField(screenshotFrameInterval);
        EditorGUI.EndDisabledGroup();
        
        EditorGUI.BeginDisabledGroup(continuousScreenshots.boolValue);
        EditorGUILayout.PropertyField(frameToScreenshot);
        EditorGUI.EndDisabledGroup();
        
        EditorGUILayout.PropertyField(screenshotFolderName);
        serializedObject.ApplyModifiedProperties();
    }
}
