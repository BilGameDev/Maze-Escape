using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : EnemyBaseState
{
    private int destPoint = 0; //Point of destination

    public override void OnStartState(EnemyStateMachine stateMachine)
    {
        enemyAI = stateMachine.enemyAI;

        enemyAI.agent.speed = enemyAI.enemyObject.defaultSpeed;
        enemyAI.animator.SetBool("Run", false);
        enemyAI.sphereCollider.radius = enemyAI.enemyObject.detectionRadius;
        GotoNextPoint();
    }

    public override void OnRunState(EnemyStateMachine stateMachine)
    {
        if (!enemyAI.agent.pathPending && enemyAI.agent.remainingDistance < 0.5f)
            GotoNextPoint();

        if (enemyAI.playerObject && CanSeePlayer(enemyAI.playerObject.transform)) stateMachine.SwitchState(stateMachine.chaseState);
    }

    void GotoNextPoint()
    {

        // Returns if no points have been set up
        if (enemyAI.patrolPoints.Length == 0)
            return;

        // Set the agent to go to the currently selected destination
        enemyAI.agent.destination = enemyAI.patrolPoints[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary
        destPoint = (destPoint + 1) % enemyAI.patrolPoints.Length;
    }

    bool CanSeePlayer(Transform playerTransform)
    {
        Vector3 directionToPlayer = playerTransform.position - enemyAI.transform.position;
        float angle = Vector3.Angle(directionToPlayer, enemyAI.transform.forward);

        if (angle <= enemyAI.enemyObject.angleDetection)
        {
            RaycastHit hit;
            if (Physics.Raycast(enemyAI.transform.transform.position, directionToPlayer.normalized, out hit, 1000))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true; // Player is in sight
                }
            }
        }

        return false; // Player is not in sight
    }
}
