using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] PlayerController playerController;

    void Update()
    {
        if(playerController.input.magnitude > 0)
            audioSource.enabled = true;
        else
            audioSource.enabled = false;
    }
}
