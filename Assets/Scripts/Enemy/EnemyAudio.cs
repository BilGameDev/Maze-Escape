using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip detectedClip;
    [SerializeField] EnemyAI enemyAI;

    void Start()
    {
        enemyAI.OnPlayerDetected += PlayerDetectedSound;
    }

    void PlayerDetectedSound()
    {
        audioSource.PlayOneShot(detectedClip);
    }
}
