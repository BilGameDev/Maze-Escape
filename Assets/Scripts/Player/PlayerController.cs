using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform playerBody;
    [SerializeField] CharacterController characterController;
    [SerializeField] CameraController cameraController;
    
    [HideInInspector] public Vector2 input;

    private Vector3 movement;


    public void Move()
    {
        // Get the forward and right vectors of the camera without the vertical component
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        // Zero out the y-component to ensure horizontal movement only
        forward.y = 0;
        right.y = 0;

        // Normalize these vectors as they may not be unit length anymore after altering the y-component
        forward.Normalize();
        right.Normalize();

        // Calculate the movement vector
        movement = input.x * right + input.y * forward;
        movement *= moveSpeed;

        // Move the character
        characterController.Move(movement * Time.deltaTime);
    }

    public void RotatePlayer()
    {
        if (input.magnitude > 0)
            playerBody.rotation = Quaternion.Lerp(playerBody.rotation, Quaternion.Euler(0, cameraController.yRotation, 0), rotationSpeed * Time.deltaTime);

    }

    void FixedUpdate()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {

        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        RotatePlayer();
    }
}
