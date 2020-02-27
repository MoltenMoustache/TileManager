using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    GameObject[,] tiles;
    [SerializeField] IntVector2 gridDimensions;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] Transform tileParent;
    [SerializeField] float spawnOffset;

    private void Start()
    {
        SpawnGrid();
    }

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

    public void AllocateNeighbourTiles()
    {

    }

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
                    line[i] = tiles[a_pointCoordinates.x, a_pointCoordinates.y + 1 + i];
                }
                break;

            // East
            case 1:
                for (int i = 0; i < a_lineLength; i++)
                {
                    line[i] = tiles[a_pointCoordinates.x + 1 + i, a_pointCoordinates.y];
                }
                break;

            // South
            case 2:
                for (int i = 0; i < a_lineLength; i++)
                {
                    line[i] = tiles[a_pointCoordinates.x, a_pointCoordinates.y - 1 - i];
                }
                break;

            // West
            case 3:
                for (int i = 0; i < a_lineLength; i++)
                {
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

    // @brief Returns a circle of tiles originating from a_point coordinates with a radius of a_radius
    // @param Does NOT include the originating point
    public GameObject[] GetCircleFromPoint(IntVector2 a_pointCoordinates, int a_radius)
    {
        return null;
    }

    // @brief Returns a circle of tiles originating from a_point coordinates with a radius of a_radius
    // @param DOES include the originating point
    public GameObject[] GetCircleWithPoint(IntVector2 a_pointCoordinates, int a_radius)
    {
        return null;
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
    #endregion

}
