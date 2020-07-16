using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordInputs : MonoBehaviour
{
    public InputCollection inputCollection;
    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            inputCollection.Add(InputType.LeftDown, Time.frameCount);
        }
        else if(Input.GetKeyUp(KeyCode.LeftArrow))
        {
            inputCollection.Add(InputType.LeftUp, Time.frameCount);
        }
        
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            inputCollection.Add(InputType.RightDown, Time.frameCount);
        }
        else if(Input.GetKeyUp(KeyCode.RightArrow))
        {
            inputCollection.Add(InputType.RightUp, Time.frameCount);
        }
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            inputCollection.Add(InputType.JumpDown, Time.frameCount);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            inputCollection.Add(InputType.JumpUp, Time.frameCount);
        }
    }
}
