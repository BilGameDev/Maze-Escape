using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyBaseState
{
    Player player;

    public override void OnRunState(EnemyStateMachine stateMachine)
    {

    }

    public override void OnStartState(EnemyStateMachine stateMachine)
    {
        enemyAI = stateMachine.enemyAI;
        player = enemyAI.playerObject.transform.root.GetComponent<Player>();

        enemyAI.animator.Play("Attack");
        player.health -= enemyAI.enemyObject.damageAmount;
        enemyAI.lastDamageTime = Time.time;

        if (player.health < 0.1f)
        {
            player.GameOver();
            stateMachine.SwitchState(stateMachine.patrolState);
        }

        stateMachine.SwitchState(stateMachine.chaseState);
    }
}
