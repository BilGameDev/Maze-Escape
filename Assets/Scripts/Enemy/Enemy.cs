using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy", order = 3)]
public class Enemy : ScriptableObject
{
    [Header("Enemy Settigs")]
    public float angleDetection; //The minimum angle for the detection to work
    public float detectionRadius; //The default trigger radius\
    public float chaseRadius; //The trigger radius once spotted
    public float defaultSpeed;
    public float runSpeed;

    public float damageDistance; //The distance at which the enemy can damage the player
    public float damageAmount = 10f; // Amount of damage
    public float damageInterval = 1f; // Damage interval
}
