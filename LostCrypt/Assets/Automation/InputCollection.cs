using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType
{
    LeftDown,
    LeftUp,
    RightDown,
    RightUp,
    JumpDown,
    JumpUp
}

[Serializable]
public struct RecordedInput
{
    public InputType type;
    public int frame;
}

[CreateAssetMenu(menuName = "Automation/Input Collection")]
public class InputCollection : ScriptableObject
{
    [SerializeField]
    public List<RecordedInput> recordedInputs;

    public void Add(InputType inputType, int frame)
    {
        RecordedInput input;
        input.type = inputType;
        input.frame = frame;
        recordedInputs.Add(input);
    }
}
