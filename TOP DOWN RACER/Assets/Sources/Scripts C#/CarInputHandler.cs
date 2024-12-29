using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    TopDownCarController topDownCarController;

    private void Awake()
    {
        topDownCarController = GetComponent<TopDownCarController>();
    }

    private void Update()
    {
        Vector2 inputVector = Vector2.zero;

        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");

        topDownCarController.SetInputVector(inputVector);

        if (Input.GetButtonDown("Jump"))
            topDownCarController.Jump(1.0f, 0.0f);
    }
}
