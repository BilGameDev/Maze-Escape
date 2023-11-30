using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState
{

    protected EnemyAI enemyAI;
    public abstract void OnStartState(EnemyStateMachine stateMachine);
    public abstract void OnRunState(EnemyStateMachine stateMachine);
}