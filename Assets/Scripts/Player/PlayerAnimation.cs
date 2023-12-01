using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Animator animatorController;

    void Update()
    {
        animatorController.SetFloat("X", playerInput.input.x);
        animatorController.SetFloat("Y", playerInput.input.y);
    }
}
