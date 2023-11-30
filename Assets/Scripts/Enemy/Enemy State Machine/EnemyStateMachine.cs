using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour, IStateMachine
{
    public EnemyAI enemyAI;
    EnemyBaseState currentState;
    public PatrolState patrolState = new PatrolState();
    public ChaseState chaseState = new ChaseState();
    public AttackState attackState = new AttackState();

    public void StartState()
    {
        currentState = patrolState;
        currentState.OnStartState(this);
    }

    public void RunState()
    {
        currentState.OnRunState(this);
    }

    public void SwitchState(EnemyBaseState baseState)
    {
        currentState = baseState;
        currentState.OnStartState(this);
    }
}
