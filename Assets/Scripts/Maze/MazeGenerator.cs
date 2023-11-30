using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

// This script is responsible for procedurally generating a Maze. 
// I use Depth-First Search to ensure the maze is always passable.
public class MazeGenerator : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] GameSettings gameSettings;

    [Header("Maze Settings")]
    [SerializeField] MazeSettings mazeSettings;


    private MazeCell[,] grid; //A 2D Array to keep grid details
    private Stack<MazeCell> cellStack = new Stack<MazeCell>(); //A stack to use for Depth-First Search, Allows us to Pop and Push to stack.
    private NavMeshSurface navMeshSurface;

    private enum Position //An enum to define the position of the door of a cell
    {
        Up,
        Right,
        Bottom,
        Left
    }

    void Start()
    {
        CreateGrid();
        CreateMaze();
    }

    void CreateGrid()
    {
        grid = new MazeCell[mazeSettings.gridWidth, mazeSettings.gridHeight];

        GameObject navMeshObject = Instantiate(mazeSettings.navMeshSurface);

        navMeshObject.transform.localScale = new Vector3(mazeSettings.gridWidth * mazeSettings.cellSpacing, 1, mazeSettings.gridWidth * mazeSettings.cellSpacing);
        navMeshObject.transform.localPosition = new Vector3(navMeshObject.transform.localScale.x / 2, -0.5f, navMeshObject.transform.localScale.z / 2);
        mazeSettings.floorMaterial.mainTextureScale = new Vector2(mazeSettings.gridWidth * mazeSettings.cellSpacing / 2,mazeSettings.gridWidth * mazeSettings.cellSpacing / 2);

        navMeshSurface = navMeshObject.GetComponent<NavMeshSurface>();


        // Uses the cell prefab to create a grid according to the defined dimentions
        for (int x = 0; x < mazeSettings.gridWidth; x++)
        {
            for (int y = 0; y < mazeSettings.gridHeight; y++)
            {
                Vector3 cellPosition = new Vector3(x * mazeSettings.cellSpacing, 0, y * mazeSettings.cellSpacing);
                GameObject cell = Instantiate(mazeSettings.cellPrefab, cellPosition, Quaternion.identity, transform);

                //Assign variable values for future use
                grid[x, y] = cell.GetComponent<MazeCell>();
                grid[x, y].x = x;
                grid[x, y].y = y;
            }
        }
    }

    void CreateMaze()
    {
        // Once the grid is generated, we can go through it using DFS.
        // We will start from the first cell, push it to the stack and mark it as visited
        // After that, we will randomly get a neighbour that have not been visited and remove walls in between.
        // If it leads to a visited path, we will backtrack.

        MazeCell currentCell = grid[0, 0];
        currentCell.visited = true;
        cellStack.Push(currentCell);

        while (cellStack.Count > 0)
        {
            currentCell = cellStack.Pop();
            List<MazeCell> unvisitedNeighbors = GetNeighbors(currentCell);

            if (unvisitedNeighbors.Count > 0)
            {
                cellStack.Push(currentCell);

                MazeCell chosenNeighbor = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
                RemoveWallsBetween(currentCell, chosenNeighbor);

                chosenNeighbor.visited = true;
                cellStack.Push(chosenNeighbor);
            }
        }

        //Set the last cell as the Win Zone
        grid[mazeSettings.gridHeight - 1, mazeSettings.gridWidth - 1].ActivateWinZone();

        //Once the maze is generated, we can bake the platform for enemies to walk on
        navMeshSurface.BuildNavMesh();

        //Then we can spawn the enemies
        SpawnEnemies();

        //And Instantiate the player
        Instantiate(gameSettings.playerPrefab, grid[0, 0].playerSpawn.position, Quaternion.identity);
    }

    void SpawnEnemies()
    {
        // We will spawn the enemies in random cells
        for (int i = 0; i < mazeSettings.numberOfEnemies; i++)
        {
            int randomX = Random.Range(0, mazeSettings.gridWidth);
            int randomY = Random.Range(0, mazeSettings.gridHeight);
            MazeCell randomCell = grid[randomX, randomY];

            // Make sure not to spawn an enemy on the starting cell
            while (randomCell == grid[0, 0])
            {
                randomX = Random.Range(0, mazeSettings.gridWidth);
                randomY = Random.Range(0, mazeSettings.gridHeight);
                randomCell = grid[randomX, randomY];
            }

            randomCell.enemyPrefab.GetComponent<EnemyAI>().patrolPoints = randomCell.patrolPoints;
            randomCell.enemyPrefab.SetActive(true);
        }
    }

    //This method helps us get unvisited neighbours
    List<MazeCell> GetNeighbors(MazeCell mazeCell)
    {
        List<MazeCell> unvisited = new List<MazeCell>();

        int x = mazeCell.x;
        int y = mazeCell.y;

        // Left
        if (y < mazeSettings.gridHeight - 1)
        {
            if (!grid[x, y + 1].visited) unvisited.Add(grid[x, y + 1]);
        }
        // Up
        if (x < mazeSettings.gridWidth - 1)
        {
            if (!grid[x + 1, y].visited) unvisited.Add(grid[x + 1, y]);
        }
        // Right
        if (y > 0)
        {
            if (!grid[x, y - 1].visited) unvisited.Add(grid[x, y - 1]);
        }
        // Bottom
        if (x > 0)
        {
            if (!grid[x - 1, y].visited) unvisited.Add(grid[x - 1, y]);
        }

        return unvisited;
    }

    //This method helps us get the position for a neighbour
    Position GetNeighborsPos(MazeCell mazeCell, MazeCell neighbourCell)
    {
        int x = mazeCell.x;
        int y = mazeCell.y;

        // Left
        if (y < mazeSettings.gridHeight - 1)
        {
            if (grid[x, y + 1] == neighbourCell)
            {
                return Position.Left;
            }
        }

        // Up
        if (x < mazeSettings.gridWidth - 1)
        {
            if (grid[x + 1, y] == neighbourCell)
            {
                return Position.Up;
            }
        }
        // Right
        if (y > 0)
        {
            if (grid[x, y - 1] == neighbourCell)
            {
                return Position.Right;
            }
        }
        // Bottom
        if (x > 0)
        {
            if (grid[x - 1, y] == neighbourCell)
            {
                return Position.Bottom;
            }
        }

        return Position.Up;
    }


    //We can remove the walls, or in our case, replace it with a door according to the information we get of its position
    void RemoveWallsBetween(MazeCell mazeCell, MazeCell chosenNeighbor)
    {

        switch (GetNeighborsPos(mazeCell, chosenNeighbor))
        {
            case Position.Up:
                mazeCell.ReplaceDoor(0);
                chosenNeighbor.ReplaceDoor(2);
                break;

            case Position.Right:
                mazeCell.ReplaceDoor(1);
                chosenNeighbor.ReplaceDoor(3);
                break;

            case Position.Bottom:
                mazeCell.ReplaceDoor(2);
                chosenNeighbor.ReplaceDoor(0);
                break;

            case Position.Left:
                mazeCell.ReplaceDoor(3);
                chosenNeighbor.ReplaceDoor(1);
                break;
        }
    }
}

