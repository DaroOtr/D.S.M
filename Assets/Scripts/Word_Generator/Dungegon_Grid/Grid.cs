using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class For The Word Grid Calculation
/// </summary>
[Serializable]
public class Grid : MonoBehaviour
{
    [Serializable]
    public struct NodeGrid
    {
        public List<Node2D> nodes;
    }
    public Node2D[,,] grid;
    public int sizeX = 10;
    public int sizeY = 1;
    public int sizeZ = 10;
    public bool showGrid;
    public float pointsInGridSize = 0.1f;
    public static float delta = 1f;

    /// <summary>
    /// Start Generating a Grid Of Nodes
    /// </summary>
    public void StartGrid() 
    {
        grid = new Node2D[sizeX, sizeY, sizeZ];

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int z = 0; z < grid.GetLength(2); z++)
                {
                    RNode_Type currentSelec;
                    Vector3 newpos = new Vector3(x * delta, y * delta, z * delta);
                    Vector3Int newGridpos = new Vector3Int(x,y,z);
                    grid[x, y, z] = new Node2D(newpos, newGridpos);
                    grid[x, y, z].state = Node_States.UnCollapsed;
                }
            }
        }
    }

    /// <summary>
    /// Returns the size for this 2D grid
    /// </summary>
    public Vector2Int Size => new(sizeX, sizeZ);

    public ref Node2D this[int x, int z] => ref grid[x, 0, z];

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        { return; }

        if (!showGrid)
            return;

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int z = 0; z < grid.GetLength(2); z++)
                {

                    if (grid[x, y, z].state != Node_States.UnCollapsed)
                    {
                        Gizmos.color = Color.black;
                        Gizmos.DrawSphere(grid[x, y, z].pos, pointsInGridSize);
                    }
                    else
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawSphere(grid[x, y, z].pos, pointsInGridSize);
                    }
                }
            }
        }
    }
}
