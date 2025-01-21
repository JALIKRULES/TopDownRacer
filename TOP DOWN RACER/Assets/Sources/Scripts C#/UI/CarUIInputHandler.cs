using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarUIInputHandler : MonoBehaviour
{
    CarInputHandler playerCarInputHandler;

    Vector2 inputVector = Vector2.zero;

    private void Awake()
    {
        CarInputHandler[] carInputHandlers = FindObjectsOfType<CarInputHandler>();

        foreach (CarInputHandler carInputHandler in carInputHandlers)
        {
            if (carInputHandler.isUIInput)
            {
                playerCarInputHandler = carInputHandler;
                break;
            }
        }



    }

    private void Start()
    {
        
    }

    public void OnAcceleratePress()
    {
        inputVector.y = 1.0f;
        playerCarInputHandler.SetInput(inputVector);
    }

    public void OnAccelerateBrakeRelease()
    {
        inputVector.y = 0.0f;
        playerCarInputHandler.SetInput(inputVector);
    }

    public void OnBrakePress()
    {
        inputVector.y = -1.0f;
        playerCarInputHandler.SetInput(inputVector);
    }

    public void OnLeftSteerPress()
    {
        inputVector.x = -1.0f;
        playerCarInputHandler.SetInput(inputVector);
    }
    public void OnRightSteerPress()
    {
        inputVector.x = 1.0f;
        playerCarInputHandler.SetInput(inputVector);
    }
    public void OnSteerRelease()
    {
        inputVector.x = 0.0f;
        playerCarInputHandler.SetInput(inputVector);
    }
}
