using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settigs")]
    [SerializeField] float angleDetection; //The minimum angle for the detection to work
    [SerializeField] float detectionRadius; //The default trigger radius\
    [SerializeField] float chaseRadius; //The trigger radius once spotted
    [SerializeField] float defaultSpeed;
    [SerializeField] float runSpeed;
    
    [SerializeField] float damageDistance; //The distance at which the enemy can damage the player
    [SerializeField] float damageAmount = 10f; // Amount of damage
    [SerializeField] float damageInterval = 1f; // Damage interval
    [SerializeField] Transform[] patrolPoints; //Points the enemy will cycle between


    private int destPoint = 0; //Point of destination
    private SphereCollider sphereCollider;
    private Animator animator;
    private NavMeshAgent agent; //Enemy uses Unity's NavMesh for pathfinding
    private float lastDamageTime;
    private GameObject playerObject = null;

    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        sphereCollider.radius = detectionRadius;
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

    void Update()
    {
        // Choose the next destination when the agent gets
        // close to the current one
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (CanSeePlayer(collider.transform))
                playerObject = collider.gameObject; //If spotted make sure the player object is set

            if (playerObject)
            {
                //Once spotted, enemy goes into chase mode
                sphereCollider.radius = chaseRadius;
                agent.speed = runSpeed;
                animator.SetBool("Run", true);
                agent.destination = playerObject.transform.position;


                //If the distance critera meets, damage the player
                if (Vector3.Distance(transform.position, playerObject.transform.position) < damageDistance && Time.time > lastDamageTime + damageInterval)
                {
                    playerObject.transform.root.GetComponent<Player>().health -= damageAmount;
                    lastDamageTime = Time.time;
                    if (playerObject.transform.root.GetComponent<Player>().health < 0.1f)
                    {
                        playerObject.transform.root.GetComponent<Player>().GameOver();
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        //If the player leaves the chase area, forget player and pathfind back to patrol points
        if (collider.CompareTag("Player"))
        {
            sphereCollider.radius = detectionRadius;
            agent.speed = defaultSpeed;
            animator.SetBool("Run", false);
            playerObject = null;
            GotoNextPoint();
        }
    }

    bool CanSeePlayer(Transform playerTransform)
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        float angle = Vector3.Angle(directionToPlayer, transform.forward);

        if (angle <= angleDetection / 2)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, 100))
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
