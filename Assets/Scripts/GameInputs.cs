using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameInputs : MonoBehaviour
{
    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = new Vector2(0, 0);

        // Player Movement Controlls
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y += -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x += -1;
        }
        
        return inputVector;
    }
    
    public bool PauseState()
    {
        // Enum
        if (Input.GetKeyDown(KeyCode.P))
        {
            return true;
        }

        return false;
    }
}
