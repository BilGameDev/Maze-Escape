using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] PlayerInput playerInput;

    void Update()
    {
        if (playerInput.input.magnitude > 0)
            audioSource.enabled = true;
        else
            audioSource.enabled = false;
    }
}
