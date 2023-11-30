using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Animator animatorController;

    void Update()
    {
        animatorController.SetFloat("X", playerController.input.x);
        animatorController.SetFloat("Y", playerController.input.y);
    }
}
