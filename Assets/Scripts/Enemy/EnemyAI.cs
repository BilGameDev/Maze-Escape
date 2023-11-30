using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;

[RequireComponent(typeof(EnemyStateMachine))]
public class EnemyAI : MonoBehaviour
{
    public Enemy enemyObject;
    public SphereCollider sphereCollider;
    public Animator animator;
    public NavMeshAgent agent; //Enemy uses Unity's NavMesh for pathfinding
    [HideInInspector] public Transform[] patrolPoints; //Points the enemy will cycle between
    [HideInInspector] public GameObject playerObject = null;
    [HideInInspector] public float lastDamageTime;


    public event Action OnPlayerDetected;

    private EnemyStateMachine enemyStateMachine;

    void Start()
    {
        enemyStateMachine = GetComponent<EnemyStateMachine>();
        enemyStateMachine.StartState();
    }

    void Update()
    {
        enemyStateMachine.RunState();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerObject = collider.gameObject;
        }
    }


    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerObject = null;
        }
    }

    public void PlayerDetected()
    {
        OnPlayerDetected?.Invoke();
    }
}
