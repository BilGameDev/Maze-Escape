using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] GameObject[] walls = new GameObject[4]; // 0: Top, 1: Right, 2: Bottom, 3: Left
    [SerializeField] GameObject[] doors = new GameObject[4]; // 0: Top, 1: Right, 2: Bottom, 3: Left
    [SerializeField] GameObject winZoneObject; //Just some objects to make it easy to otice the windZone
    public Transform[] patrolPoints; // Patrol points
    public GameObject enemyPrefab;
    public Transform playerSpawn;
    [HideInInspector] public int x ,y; //Coordiates in the grid
    [HideInInspector] public bool visited = false;
    [HideInInspector] public bool winZone = false;


    public void ReplaceDoor(int index)
    {
        walls[index].SetActive(false);
        doors[index].SetActive(true);
    }

    public void ActivateWinZone()
    {
        winZoneObject.SetActive(true);
        winZone = true;
    }


    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && winZone)
        {
            collider.transform.root.GetComponent<Player>().Win();
        }
    }

}
