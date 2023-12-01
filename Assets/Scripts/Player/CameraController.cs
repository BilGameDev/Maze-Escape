using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] Transform playerCameraParent;
    [SerializeField] PlayerInput playerInput;

    [HideInInspector] public float xRotation = 0f;
    [HideInInspector] public float yRotation = 0f;

    private Vector2 mouseInput;

    void Update()
    {
        mouseInput.x = playerInput.lookInput.x * mouseSensitivity;
        mouseInput.y = playerInput.lookInput.y * mouseSensitivity;

        xRotation -= mouseInput.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limits the up/down camera rotation

        yRotation += mouseInput.x;
        playerCameraParent.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}