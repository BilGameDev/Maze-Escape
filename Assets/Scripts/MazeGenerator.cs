using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

// This script is responsible for procedurally generating a Maze. 
// I use Depth-First Search to ensure the maze is always passable.
public class MazeGenerator : MonoBehaviour
{
    [Header("Maze Settings")]
    [SerializeField] int gridWidth = 10;
    [SerializeField] int gridHeight = 10;
    [SerializeField] float cellSpacing = 10f;
    [SerializeField] int numberOfEnemies; //Amount of enemies to spawn in the maze

    [Header("References")]
    [SerializeField] GameObject cellPrefab; // Prefab for the cell object
    [SerializeField] GameObject playerPrefab;
    [SerializeField] NavMeshSurface navMeshSurface;

    private MazeCell[,] grid; //A 2D Array to keep grid details
    private Stack<MazeCell> cellStack = new Stack<MazeCell>(); //A stack to use for Depth-First Search, Allows us to Pop and Push to stack.

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
        grid = new MazeCell[gridWidth, gridHeight];

        // Uses the cell prefab to create a grid according to the defined dimentions
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 cellPosition = new Vector3(x * cellSpacing, 0, y * cellSpacing);
                GameObject cell = Instantiate(cellPrefab, cellPosition, Quaternion.identity, transform);

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
        grid[gridHeight - 1, gridWidth - 1].ActivateWinZone();

        //Once the maze is generated, we can bake the platform for enemies to walk on
        navMeshSurface.BuildNavMesh();

        //Then we can spawn the enemies
        SpawnEnemies();

        //And Instantiate the player
        Instantiate(playerPrefab, grid[0, 0].transform.position, Quaternion.identity);
    }

    void SpawnEnemies()
    {
        // We will spawn the enemies in random cells
        for (int i = 0; i < numberOfEnemies; i++)
        {
            int randomX = Random.Range(0, gridWidth);
            int randomY = Random.Range(0, gridHeight);
            MazeCell randomCell = grid[randomX, randomY];

            // Make sure not to spawn an enemy on the starting cell
            while (randomCell == grid[0, 0])
            {
                randomX = Random.Range(0, gridWidth);
                randomY = Random.Range(0, gridHeight);
                randomCell = grid[randomX, randomY];
            }

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
        if (y < gridHeight - 1)
        {
            if (!grid[x, y + 1].visited) unvisited.Add(grid[x, y + 1]);
        }
        // Up
        if (x < gridWidth - 1)
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
        if (y < gridHeight - 1)
        {
            if (grid[x, y + 1] == neighbourCell)
            {
                return Position.Left;
            }
        }

        // Up
        if (x < gridWidth - 1)
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

