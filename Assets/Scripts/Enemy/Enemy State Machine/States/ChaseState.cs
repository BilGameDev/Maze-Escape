

using UnityEngine;

public class ChaseState : EnemyBaseState
{
    public override void OnStartState(EnemyStateMachine stateMachine)
    {
        enemyAI = stateMachine.enemyAI;

        enemyAI.sphereCollider.radius = enemyAI.enemyObject.chaseRadius;
        enemyAI.agent.speed = enemyAI.enemyObject.runSpeed;
        enemyAI.animator.SetBool("Run", true);
        enemyAI.PlayerDetected();
    }


    public override void OnRunState(EnemyStateMachine stateMachine)
    {

        if (enemyAI.playerObject == null)
            stateMachine.SwitchState(stateMachine.patrolState);
        else
        {
            enemyAI.agent.destination = enemyAI.playerObject.transform.position;
            if (InAttackRange() && CanAttackPlayer()) stateMachine.SwitchState(stateMachine.attackState);
        }

    }

    bool InAttackRange()
    {
        return Vector3.Distance(enemyAI.transform.position, enemyAI.playerObject.transform.position) < enemyAI.enemyObject.damageDistance;
    }

    bool CanAttackPlayer()
    {
        return Time.time > enemyAI.lastDamageTime + enemyAI.enemyObject.damageInterval;
    }

}
