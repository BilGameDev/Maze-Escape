using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Maze Settings", order = 2)]
public class MazeSettings : ScriptableObject
{
    [Header("Settings")]
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float cellSpacing = 10f;
    public int numberOfEnemies; //Amount of enemies to spawn in the maze

    [Header("References")]
    public GameObject cellPrefab; // Prefab for the cell object
    public GameObject navMeshSurface; // Prefab for the navMeshSurface
    public Material floorMaterial;
}
