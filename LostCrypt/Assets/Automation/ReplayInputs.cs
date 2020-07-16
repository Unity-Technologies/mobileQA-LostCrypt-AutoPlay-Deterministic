using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public struct InputState
{
    public bool Left;
    public bool Right;
    public bool JumpDown;
    public bool JumpUp;
}

public class ReplayInputs : MonoBehaviour
{
    public InputCollection inputCollection;
    public InputState inputState;

    Queue<RecordedInput> inputQueue;

    void Awake()
    {
        inputQueue = new Queue<RecordedInput>();
        foreach (var recordedInput in inputCollection.recordedInputs)
        {
            inputQueue.Enqueue(recordedInput);
        }
    }

    void Update()
    {
        inputState.JumpDown = inputState.JumpUp = false;

        while (inputQueue.Count >= 1 && inputQueue.Peek().frame == Time.frameCount)
        {
            var recordedInput = inputQueue.Dequeue();
            switch (recordedInput.type)
            {
                case InputType.LeftDown:
                    inputState.Left = true;
                    break;
                case InputType.LeftUp:
                    inputState.Left = false;
                    break;
                case InputType.RightDown:
                    inputState.Right = true;
                    break;
                case InputType.RightUp:
                    inputState.Right = false;
                    break;
                case InputType.JumpDown:
                    inputState.JumpDown = true;
                    break;
                case InputType.JumpUp:
                    inputState.JumpUp = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
#if UNITY_EDITOR
        if (inputQueue.Count <= 0)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif
    }
}
