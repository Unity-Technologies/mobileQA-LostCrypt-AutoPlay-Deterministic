using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PauseAtFrame : MonoBehaviour
{
    public int frameToPauseAt;

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount == frameToPauseAt)
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPaused = true;
            #endif
        }
    }
}
