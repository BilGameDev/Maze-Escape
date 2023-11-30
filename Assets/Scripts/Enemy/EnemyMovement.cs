using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed { get; set; }
    public float runSpeed { get; set; }
    [SerializeField] public Transform[] patrolPoints; //Points the enemy will cycle between

    private int destPoint = 0; //Point of destination
    [HideInInspector] public NavMeshAgent agent; //Enemy uses Unity's NavMesh for pathfinding

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Move()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (patrolPoints.Length == 0)
            return;

        // Set the agent to go to the currently selected destination
        agent.destination = patrolPoints[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary
        destPoint = (destPoint + 1) % patrolPoints.Length;
    }
}
