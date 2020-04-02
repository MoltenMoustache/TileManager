using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    #region Singleton
    public static TileManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion


    GameObject[,] tiles;
    Djikstras.Node[,] nodes;
    [SerializeField] IntVector2 gridDimensions;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] Transform tileParent;
    [SerializeField] float spawnOffset;


    GameObject[] TwoDimensionToOneDimensionArray(GameObject[,] a_2dArray)
    {
        int xLength = a_2dArray.GetLength(0);
        int yLength = a_2dArray.GetLength(1);
        GameObject[] oneDimensional = new GameObject[xLength * yLength];

        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                oneDimensional[x * xLength + y] = a_2dArray[x, y];
            }
        }

        return oneDimensional;
    }

    [ContextMenu("Spawn Grid")]
    public void SpawnGrid()
    {
        Vector3 spawnPosition = new Vector3(0, 0, 0);
        tiles = new GameObject[gridDimensions.x, gridDimensions.y];
        nodes = new Djikstras.Node[gridDimensions.x, gridDimensions.y];

        for (int x = 0; x < gridDimensions.x; x++)
        {
            spawnPosition.x += spawnOffset;
            spawnPosition.z = 0;
            for (int y = 0; y < gridDimensions.y; y++)
            {
                spawnPosition.z += spawnOffset;
                GameObject newTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity, tileParent);
                newTile.name = "Tile [" + x + "," + y + "]";
                newTile.tag = "Tile";
                tiles[x, y] = newTile;
            }
        }
    }

    public void SpawnGrid(float a_yPos)
    {
        Vector3 spawnPosition = new Vector3(0, a_yPos, 0);
        tiles = new GameObject[gridDimensions.x, gridDimensions.y];
        nodes = new Djikstras.Node[gridDimensions.x, gridDimensions.y];

        for (int x = 0; x < gridDimensions.x; x++)
        {
            spawnPosition.x += spawnOffset;
            spawnPosition.z = 0;
            for (int y = 0; y < gridDimensions.y; y++)
            {
                spawnPosition.z += spawnOffset;
                GameObject newTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity, tileParent);
                newTile.name = "Tile [" + x + "," + y + "]";
                newTile.tag = "Tile";
                tiles[x, y] = newTile;
            }
        }
    }

    [ContextMenu("Destroy Grid")]
    public void DestroyGrid()
    {
        GameObject[] gridTiles = GameObject.FindGameObjectsWithTag("Tile");
        if (gridTiles.Length == 0)
        {
            Debug.Log("No Tiles Found.");
            return;
        }

        for (int i = 0; i < gridTiles.Length; i++)
        {
            if (gridTiles[i] != null)
                DestroyImmediate(gridTiles[i]);
        }
    }

    #region Djikstra's Shortest Path
    public void AddNodeComponentsToTiles()
    {
        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                nodes[x, y] = tiles[x, y].AddComponent<Djikstras.Node>();
                nodes[x, y].SetGridCoordinates(new IntVector2(x, y));
            }
        }
    }

    public void AllocateNeighbourTiles()
    {
        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                // Connects Nodes horziontally
                if (x < gridDimensions.x - 1)
                    CreateEdge(nodes[x, y], nodes[x + 1, y], spawnOffset);

                // Connects Nodes vertically
                if (y < gridDimensions.y - 1)
                    CreateEdge(nodes[x, y], nodes[x, y + 1], spawnOffset);

                // Connects Nodes diagonally
                float diagonalOffset = 1.414213562f;
                // Down to the right '/'
                if (y > 0 && x < gridDimensions.y - 1)
                    CreateEdge(nodes[x, y], nodes[x + 1, y - 1], spawnOffset * diagonalOffset);
                // Up to the right '/'
                if (x < gridDimensions.x - 1 && y < gridDimensions.y - 1)
                    CreateEdge(nodes[x, y], nodes[x + 1, y + 1], spawnOffset * diagonalOffset);

            }
        }
    }

    void CreateEdge(Djikstras.Node a_nodeA, Djikstras.Node a_nodeB, float a_cost = 1.0f)
    {
        Djikstras.Edge newEdge = new Djikstras.Edge(a_nodeA, a_nodeB, a_cost);
    }

    public List<Djikstras.Node> GetPathDjikstra(GameObject a_start, GameObject a_end)
    {
        // Gets GameObject from Node components
        Djikstras.Node startNode = a_start.GetComponent<Djikstras.Node>();
        Djikstras.Node endNode = a_end.GetComponent<Djikstras.Node>();

        // Return path
        return GetPathDjikstra(startNode, endNode);
    }

    public List<Djikstras.Node> GetPathDjikstra(Djikstras.Node a_start, Djikstras.Node a_end)
    {
        List<Djikstras.Node> path = new List<Djikstras.Node>();

        // Resets everything to default values
        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                nodes[x, y].ResetNode();
            }
        }

        path.Clear();

        if (!a_start || !a_end)
        {
            Debug.LogWarning("Start or End node is equal to NULL");
            return path;
        }

        if (a_start == a_end)
        {
            path.Add(a_start);
            return path;
        }

        Queue<Djikstras.Node> openList = new Queue<Djikstras.Node>();
        List<Djikstras.Node> closedList = new List<Djikstras.Node>();

        openList.Enqueue(a_start);

        Djikstras.Node currentNode;
        while (openList.Count > 0)
        {
            currentNode = openList.Dequeue();
            closedList.Add(currentNode);

            List<Djikstras.Edge> edges = currentNode.GetEdges();
            for (int i = 0; i < edges.Count; i++)
            {
                // If edge is not valid, move along
                if (!edges[i].IsValid())
                    continue;

                // 
                Djikstras.Node otherNode = null;
                if (edges[i].GetNodes()[0] == currentNode)
                    otherNode = edges[i].GetNodes()[1];
                else
                    otherNode = edges[i].GetNodes()[0];

                // If the other node is not in the closed list
                if (!closedList.Contains(otherNode))
                {
                    float currentGScore = currentNode.GetGScore() + edges[i].GetCost();

                    // If the other node is not in the open list
                    if (!openList.Contains(otherNode))
                    {
                        // Set the GScore to the current node's score + the cost of the edge
                        otherNode.SetGScore(currentNode.GetGScore() + edges[i].GetCost());

                        otherNode.SetPreviousNode(currentNode);

                        // Add the other node to the open list
                        openList.Enqueue(otherNode);
                    }
                    else if (currentGScore < otherNode.GetGScore())
                    {
                        // Override the current gScore
                        otherNode.SetGScore(currentGScore);
                        // and override the previous node
                        otherNode.SetPreviousNode(currentNode);
                    }
                }
            }
        }

        Djikstras.Node endNode = a_end;
        path.Add(endNode);

        while (endNode != a_start)
        {
            if (!endNode)
            {
                path.Clear();
                return path;
            }

            endNode = endNode.GetPreviousNode();
            path.Add(endNode);
        }

        // Return the final path
        return path;
    }

    public List<GameObject> GetPathDjikstraAsGameObject(Djikstras.Node a_start, Djikstras.Node a_end)
    {
        List<Djikstras.Node> pathAsNodes = GetPathDjikstra(a_start, a_end);
        // Adds the gameobject component of every node in the path to another list
        List<GameObject> path = new List<GameObject>();
        for (int i = 0; i < pathAsNodes.Count; i++)
        {
            path.Add(pathAsNodes[i].gameObject);
        }

        // New path of game objects
        return path;
    }

    public List<GameObject> GetPathDjikstraAsGameObject(GameObject a_start, GameObject a_end)
    {
        Djikstras.Node start = a_start.GetComponent<Djikstras.Node>();
        Djikstras.Node end = a_end.GetComponent<Djikstras.Node>();

        return GetPathDjikstraAsGameObject(start, end);
    }
    #endregion

    #region Getters
    // @brief Returns the tile at a_tileCoordinates in the grid
    public GameObject GetTile(IntVector2 a_tileCoordinates)
    {
        return tiles[a_tileCoordinates.x, a_tileCoordinates.y];
    }

    // @brief Returns the tile grid as a 2D array
    public GameObject[,] GetTiles()
    {
        return tiles;
    }

    public Djikstras.Node[,] GetNodes()
    {
        return nodes;
    }

    // @brief Return a random tile from tiles[,]
    public GameObject GetRandomTile()
    {
        return tiles[(int)Random.Range(0, gridDimensions.x), (int)Random.Range(0, gridDimensions.y)];
    }

    // @TODO Ensure random tiles are only picked once
    public GameObject[] GetRandomTiles(int a_tileCount)
    {
        // If a_tileCount is greater than the total amount of tiles in the grid, simply return the entire grid.
        if (a_tileCount >= gridDimensions.x * gridDimensions.y)
            return TwoDimensionToOneDimensionArray(GetTiles());

        GameObject[] tiles = new GameObject[a_tileCount];
        for (int i = 0; i < a_tileCount; i++)
        {
            tiles[i] = GetRandomTile();
        }

        return tiles;
    }

    // @brief Returns the entire row at [i , a_y]
    public GameObject[] GetRow(int a_y)
    {
        int rowLength = tiles.GetLength(0);
        GameObject[] row = new GameObject[rowLength];

        for (int i = 0; i < rowLength; i++)
        {
            row[i] = tiles[i, a_y];
        }

        return row;
    }

    // @brief Returns the entire column at [a_x , i]
    public GameObject[] GetColumn(int a_x)
    {
        int columnLength = tiles.GetLength(0);
        GameObject[] column = new GameObject[columnLength];

        for (int i = 0; i < columnLength; i++)
        {
            column[i] = tiles[a_x, i];
        }

        return column;
    }

    // @brief Returns a_lineLength amount of tiles in a line originating from a_pointCoordinates in a_direction
    // @param Direction int: 0 = North, 1 = East, 2 = South, 3 = West
    // @param Does NOT include original point
    public GameObject[] GetLineFromPoint(IntVector2 a_pointCoordinates, int a_direction, int a_lineLength)
    {
        GameObject[] line = new GameObject[a_lineLength];

        switch (a_direction)
        {
            // North
            case 0:
                for (int i = 0; i < a_lineLength; i++)
                {
                    if (a_pointCoordinates.y + 1 + i < gridDimensions.y)
                        if (tiles[a_pointCoordinates.x, a_pointCoordinates.y + 1 + i] != null)
                            line[i] = tiles[a_pointCoordinates.x, a_pointCoordinates.y + 1 + i];
                }
                break;

            // East
            case 1:
                for (int i = 0; i < a_lineLength; i++)
                {
                    if (a_pointCoordinates.x + 1 + i < gridDimensions.x)
                        if (tiles[a_pointCoordinates.x + 1 + i, a_pointCoordinates.y] != null)
                            line[i] = tiles[a_pointCoordinates.x + 1 + i, a_pointCoordinates.y];
                }
                break;

            // South
            case 2:
                for (int i = 0; i < a_lineLength; i++)
                {
                    if (a_pointCoordinates.y - 1 - i >= 0)
                        if (tiles[a_pointCoordinates.x, a_pointCoordinates.y - 1 - i] != null)
                            line[i] = tiles[a_pointCoordinates.x, a_pointCoordinates.y - 1 - i];
                }
                break;

            // West
            case 3:
                for (int i = 0; i < a_lineLength; i++)
                {
                    if (a_pointCoordinates.x - 1 - i >= 0)
                        if (tiles[a_pointCoordinates.x - 1 - i, a_pointCoordinates.y] != null)
                            line[i] = tiles[a_pointCoordinates.x - 1 - i, a_pointCoordinates.y];
                }
                break;
        }
        return line;
    }

    // @brief Returns a_lineLength amount of tiles in a line originating from a_pointCoordinates in a_direction
    // @param Direction int: 0 = North, 1 = East, 2 = South, 3 = West
    // @param DOES include original point
    public GameObject[] GetLineWithPoint(IntVector2 a_pointCoordinates, int a_direction, int a_lineLength)
    {
        GameObject[] line = new GameObject[a_lineLength];

        switch (a_direction)
        {
            // North
            case 0:
                for (int i = 0; i < a_lineLength; i++)
                {
                    line[i] = tiles[a_pointCoordinates.x, a_pointCoordinates.y + i];
                }
                break;

            // East
            case 1:
                for (int i = 0; i < a_lineLength; i++)
                {
                    line[i] = tiles[a_pointCoordinates.x + i, a_pointCoordinates.y];
                }
                break;

            // South
            case 2:
                for (int i = 0; i < a_lineLength; i++)
                {
                    line[i] = tiles[a_pointCoordinates.x, a_pointCoordinates.y - i];
                }
                break;

            // West
            case 3:
                for (int i = 0; i < a_lineLength; i++)
                {
                    line[i] = tiles[a_pointCoordinates.x - i, a_pointCoordinates.y];
                }
                break;
        }
        return line;
    }


    public GameObject[] GetCrossFromPoint(IntVector2 a_pointCoordinates, int a_lineLength)
    {
        GameObject[] cross = new GameObject[a_lineLength * 4];
        int index = 0;

        for (int j = 0; j < 4; j++)
        {
            GameObject[] line = GetLineFromPoint(a_pointCoordinates, j, a_lineLength);
            for (int k = 0; k < line.Length; k++)
            {
                if (line[k] != null)
                    cross[index + k] = line[k];
            }
            index += a_lineLength;
        }

        return cross;
    }

    public GameObject[] GetCrossWithPoint(IntVector2 a_pointCoordinates, int a_lineLength)
    {
        // Creates new array of GameObjects, size of a_lineLength-1 * 4 + 1 (the length of a line in four directions, +1 for middle point, -1 per line for middle point)
        GameObject[] cross = new GameObject[((a_lineLength - 1) * 4) + 1];

        // Sets index at [1] and sets element 0 as center point
        int index = 1;
        cross[0] = tiles[a_pointCoordinates.x, a_pointCoordinates.y];

        for (int j = 0; j < 4; j++)
        {
            GameObject[] line = GetLineFromPoint(a_pointCoordinates, j, a_lineLength - 1);
            for (int k = 0; k < line.Length; k++)
            {
                cross[index + k] = line[k];
            }
            index += a_lineLength - 1;
        }

        return cross;
    }

    // @TODO Change to GetCrossFromPoint
    GameObject[] GetNeighbourTiles(IntVector2 a_pointCoordinates)
    {
        return GetCrossFromPoint(a_pointCoordinates, 1);
    }
    GameObject[] GetNeighbourTiles(GameObject a_tile)
    {
        if (a_tile == null)
        {
            Debug.LogWarning("Tile is equal to NULL");
            return null;
        }

        if (a_tile.GetComponent<Djikstras.Node>() != null)
            return GetCrossFromPoint(a_tile.GetComponent<Djikstras.Node>().GetGridCoordinates(), 1);
        else
            return null;
    }

    // @TODO: Make sure circle does not include points already added to final list
    // @brief Returns a circle of tiles originating from a_point coordinates with a radius of a_radius
    // @param Does NOT include the originating point
    public GameObject[] GetCircleFromPoint(IntVector2 a_pointCoordinates, int a_radius)
    {
        if (a_radius == 0)
            return null;
        else if (a_radius == 1)
            return GetNeighbourTiles(a_pointCoordinates);

        Queue<GameObject> openList = new Queue<GameObject>();
        List<GameObject> finalList = new List<GameObject>();

        // Gets first set of neighbour tiles
        GameObject[] neighbours = GetNeighbourTiles(a_pointCoordinates);
        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours[i] != null)
                openList.Enqueue(neighbours[i]);
        }

        // While there are tiles in the open list...
        int loopCount = a_radius - 1;
        while (openList.Count > 0)
        {
            int openCount = openList.Count;
            // If the while loop has looped through a_radius - 1 times...
            if (loopCount <= 0)
            {
                // Add remaining tiles from open list in to final list
                for (int i = 0; i < openCount; i++)
                {
                    finalList.Add(openList.Dequeue());
                }
                break;
            }

            openCount = openList.Count;
            for (int i = 0; i < openCount; i++)
            {
                GameObject tile = openList.Dequeue();

                finalList.Add(tile);
                neighbours = GetNeighbourTiles(tile);

                for (int j = 0; j < neighbours.Length; j++)
                {
                    if (neighbours[j] != null && !openList.Contains(neighbours[j]) && !finalList.Contains(neighbours[j]) && neighbours[j] != GetTile(a_pointCoordinates))
                        openList.Enqueue(neighbours[j]);
                }
            }


            loopCount--;
        }



        GameObject[] circle = new GameObject[finalList.Count];
        for (int k = 0; k < finalList.Count; k++)
        {
            circle[k] = finalList[k];
        }

        return circle;
    }

    public GameObject[] GetCircleFromPoint(GameObject a_tile, int a_radius)
    {
        IntVector2 a_pointCoordinates;
        if (a_tile.GetComponent<Djikstras.Node>() != null)
            a_pointCoordinates = a_tile.GetComponent<Djikstras.Node>().GetGridCoordinates();
        else
            return null;



        if (a_radius == 0)
            return null;
        else if (a_radius == 1)
            return GetNeighbourTiles(a_pointCoordinates);

        Queue<GameObject> openList = new Queue<GameObject>();
        List<GameObject> finalList = new List<GameObject>();

        // Gets first set of neighbour tiles
        GameObject[] neighbours = GetNeighbourTiles(a_pointCoordinates);
        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours[i] != null)
                openList.Enqueue(neighbours[i]);
        }

        // While there are tiles in the open list...
        int loopCount = a_radius - 1;
        while (openList.Count > 0)
        {
            int openCount = openList.Count;
            // If the while loop has looped through a_radius - 1 times...
            if (loopCount <= 0)
            {
                // Add remaining tiles from open list in to final list
                for (int i = 0; i < openCount; i++)
                {
                    finalList.Add(openList.Dequeue());
                }
                break;
            }

            openCount = openList.Count;
            for (int i = 0; i < openCount; i++)
            {
                GameObject tile = openList.Dequeue();

                finalList.Add(tile);
                neighbours = GetNeighbourTiles(tile);

                for (int j = 0; j < neighbours.Length; j++)
                {
                    if (neighbours[j] != null && !openList.Contains(neighbours[j]) && !finalList.Contains(neighbours[j]) && neighbours[j] != GetTile(a_pointCoordinates))
                        openList.Enqueue(neighbours[j]);
                }
            }


            loopCount--;
        }



        GameObject[] circle = new GameObject[finalList.Count];
        for (int k = 0; k < finalList.Count; k++)
        {
            circle[k] = finalList[k];
        }

        return circle;
    }

    // @brief Returns a circle of tiles originating from a_point coordinates with a radius of a_radius
    // @param DOES include the originating point
    public GameObject[] GetCircleWithPoint(IntVector2 a_pointCoordinates, int a_radius)
    {
        if (a_radius == 1)
            return GetNeighbourTiles(a_pointCoordinates);

        Queue<GameObject> openList = new Queue<GameObject>();
        List<GameObject> finalList = new List<GameObject>();
        finalList.Add(GetTile(a_pointCoordinates));

        if (a_radius == 0)
        {
            GameObject[] startPoint = new GameObject[1];
            startPoint[0] = finalList[0];
            return startPoint;
        }

        // Gets first set of neighbour tiles
        GameObject[] neighbours = GetNeighbourTiles(a_pointCoordinates);
        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours[i] != null)
                openList.Enqueue(neighbours[i]);
        }

        // While there are tiles in the open list...
        int loopCount = a_radius - 1;
        while (openList.Count > 0)
        {
            int openCount = openList.Count;
            // If the while loop has looped through a_radius - 1 times...
            if (loopCount <= 0)
            {
                // Add remaining tiles from open list in to final list
                for (int i = 0; i < openCount; i++)
                {
                    finalList.Add(openList.Dequeue());
                }
                break;
            }

            openCount = openList.Count;
            for (int i = 0; i < openCount; i++)
            {
                GameObject tile = openList.Dequeue();

                finalList.Add(tile);
                neighbours = GetNeighbourTiles(tile);

                for (int j = 0; j < neighbours.Length; j++)
                {
                    if (neighbours[j] != null && !openList.Contains(neighbours[j]) && !finalList.Contains(neighbours[j]) && neighbours[j] != GetTile(a_pointCoordinates))
                        openList.Enqueue(neighbours[j]);
                }
            }


            loopCount--;
        }



        GameObject[] circle = new GameObject[finalList.Count];
        for (int k = 0; k < finalList.Count; k++)
        {
            circle[k] = finalList[k];
        }

        return circle;
    }

    public GameObject[] GetDiagonalCrossFromPoint(IntVector2 a_pointCoordinates, int a_diagonalLength)
    {
        List<GameObject> crossList = new List<GameObject>();

        // Toward top right '/'
        for (int i = 1; i < a_diagonalLength + 1; i++)
        {
            if (a_pointCoordinates.x + i < gridDimensions.x && a_pointCoordinates.y + i < gridDimensions.y)
                crossList.Add(tiles[a_pointCoordinates.x + i, a_pointCoordinates.y + i]);
        }

        // Bottom left '/'
        for (int i = 1; i < a_diagonalLength + 1; i++)
        {
            if (a_pointCoordinates.x - i >= 0 && a_pointCoordinates.y - i >= 0)
                crossList.Add(tiles[a_pointCoordinates.x - i, a_pointCoordinates.y - i]);
        }

        // Bottom right '\'
        for (int i = 1; i < a_diagonalLength + 1; i++)
        {
            if (a_pointCoordinates.x + i < gridDimensions.x && a_pointCoordinates.y - i >= 0)
                crossList.Add(tiles[a_pointCoordinates.x + i, a_pointCoordinates.y - i]);
        }

        // Top left '\'
        for (int i = 1; i < a_diagonalLength + 1; i++)
        {
            if (a_pointCoordinates.x - i >= 0 && a_pointCoordinates.y + i < gridDimensions.y)
                crossList.Add(tiles[a_pointCoordinates.x - i, a_pointCoordinates.y + i]);
        }

        // Loops through list and adds all elements to an array and returns it
        GameObject[] cross = new GameObject[crossList.Count];
        for (int i = 0; i < crossList.Count; i++)
        {
            cross[i] = crossList[i];
        }

        return cross;
    }

    public GameObject[] GetDiagonalCrossWithPoint(IntVector2 a_pointCoordinates, int a_diagonalLength)
    {
        List<GameObject> crossList = new List<GameObject>();

        // Adds originating tile
        crossList.Add(tiles[a_pointCoordinates.x, a_pointCoordinates.y]);

        // Toward top right '/'
        for (int i = 1; i < a_diagonalLength + 1; i++)
        {
            if (a_pointCoordinates.x + i < gridDimensions.x && a_pointCoordinates.y + i < gridDimensions.y)
                crossList.Add(tiles[a_pointCoordinates.x + i, a_pointCoordinates.y + i]);
        }

        // Bottom left '/'
        for (int i = 1; i < a_diagonalLength + 1; i++)
        {
            if (a_pointCoordinates.x - i >= 0 && a_pointCoordinates.y - i >= 0)
                crossList.Add(tiles[a_pointCoordinates.x - i, a_pointCoordinates.y - i]);
        }

        // Bottom right '\'
        for (int i = 1; i < a_diagonalLength + 1; i++)
        {
            if (a_pointCoordinates.x + i < gridDimensions.x && a_pointCoordinates.y - i >= 0)
                crossList.Add(tiles[a_pointCoordinates.x + i, a_pointCoordinates.y - i]);
        }

        // Top left '\'
        for (int i = 1; i < a_diagonalLength + 1; i++)
        {
            if (a_pointCoordinates.x - i >= 0 && a_pointCoordinates.y + i < gridDimensions.y)
                crossList.Add(tiles[a_pointCoordinates.x - i, a_pointCoordinates.y + i]);
        }

        // Loops through list and adds all elements to an array and returns it
        GameObject[] cross = new GameObject[crossList.Count];
        for (int i = 0; i < crossList.Count; i++)
        {
            cross[i] = crossList[i];
        }

        return cross;
    }

    public int GetWidth()
    {
        return gridDimensions.x;
    }

    public int GetHeight()
    {
        return gridDimensions.y;
    }
    #endregion

}
