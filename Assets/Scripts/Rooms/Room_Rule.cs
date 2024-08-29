using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Spawn_Type {Cannot_Spawn,Can_Spawn,Has_To_Spawn}

/// <summary>
/// Room Rule Behaviour for the Dungegon Generator Script
/// </summary>
[Obsolete]public class Room_Rule : MonoBehaviour
{
    [Header("SetUp")]
    [SerializeField] private GameObject room;
    [SerializeField] private Vector2Int minPos;
    [SerializeField] private Vector2Int maxPos;
    [SerializeField] private bool obligatory;

    /// <summary>
    /// Get The Probability of Spawning of the current Room
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public int ProbabilityOfSpawning(int x , int y) 
    {
        if (x >= minPos.x && x <= maxPos.x && y >= minPos.y && y <= maxPos.y)
        {
            return obligatory ? (int)Spawn_Type.Has_To_Spawn : (int)Spawn_Type.Can_Spawn;
        }

        return (int)Spawn_Type.Cannot_Spawn;
    }

    /// <summary>
    /// Get The Current Room GameObject
    /// </summary>
    /// <returns></returns>
    public GameObject GetRoom() 
    {
        return room;
    }

    /// <summary>
    /// Get the Current Room Min Position to Spawn
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetMinPos() 
    {
        return minPos;
    }

    /// <summary>
    /// Get the Current Room Max Position to Spawn
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetMaxPos()
    {
        return maxPos;
    }
}
