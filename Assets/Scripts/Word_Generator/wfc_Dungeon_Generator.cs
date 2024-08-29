using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class To Handle The Wave Function Collapse Algorithm
/// </summary>
public class wfc_Dungeon_Generator : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] public Transform playerContainer;

    [SerializeField] private Grid gr;
    [SerializeField] private W_F_C wave;
    [SerializeField] private Vector3 ofset;
    [SerializeField] private int boss_RoomsCant;
    [SerializeField] private int spawn_RoomsCant;
    [SerializeField] private Transform gridHolder;
    [SerializeField] private List<GameObject> spawn_rooms_Prefab; // Empty = 0, Spawn = 1, Trap = 2, Norma = 3, Boss = 4
    [SerializeField] private List<GameObject> trap_rooms_Prefab; // Empty = 0, Spawn = 1, Trap = 2, Norma = 3, Boss = 4
    [SerializeField] private List<GameObject> normal_rooms_Prefab; // Empty = 0, Spawn = 1, Trap = 2, Norma = 3, Boss = 4
    [SerializeField] private List<GameObject> boss_rooms_Prefab; // Empty = 0, Spawn = 1, Trap = 2, Norma = 3, Boss = 4
    [SerializeField] private List<Room_Behaviour> dungegonRooms;
    [SerializeField] private Player_Data_Source player_data;
    private int currentRoomIndex = -1;
    private Room_Behaviour currentRoom;





    void Start()
    {
        wave.StartWFC();
        CheckRoomCant();
        InstantiatePrefabs();

        List<Transform> rooms = transform.Cast<Transform>().ToList();

        foreach (Transform item in rooms)
        {
            if (item.CompareTag("Respawn"))
            {
                player_data._player.GetPlayerRigidbody().transform.position = item.position;
            }
            dungegonRooms.Add(item.GetComponent<Room_Behaviour>());
        }
        foreach (var item in dungegonRooms)
        {
            item.SetAdjRooms();
        }
    }

    void Update()
    {
        CheckPlayerPos();
    }

    /// <summary>
    /// Check If The Player Is Inside a Room To Make it Visible
    /// </summary>
    void CheckPlayerPos()
    {
        var aux = currentRoom;
        var count = 0f;

        for (int i = 0; i < dungegonRooms.Count; i++)
        {
            if (dungegonRooms[i].isPointInside(player_data._player.transform.position))
            {
                currentRoom = dungegonRooms[i];
                dungegonRooms[i].SetRoomVisible(true);
                dungegonRooms[i].SetRoomCheked(true);
            }
            else
            {
                dungegonRooms[i].SetRoomVisible(false);
                dungegonRooms[i].SetRoomCheked(false);
                count++;
            }

        }

        if (count == dungegonRooms.Count)
        {
            currentRoom = aux;
            currentRoom.SetRoomVisible(true);
            currentRoom.SetRoomCheked(true);
        }
        currentRoom.ShowAdjRooms();
    }

    /// <summary>
    /// Check The Specifics Room amount of Boos and Spawn Rooms so they Spawn Only a specific amount
    /// </summary>
    private void CheckRoomCant()
    {
        List<Node2D> boss_Rooms = new List<Node2D>();
        List<Node2D> spawn_Rooms = new List<Node2D>();

        for (int x = 0; x < gr.grid.GetLength(0); x++)
        {
            for (int y = 0; y < gr.grid.GetLength(1); y++)
            {
                for (int z = 0; z < gr.grid.GetLength(2); z++)
                {
                    // Check the Boss Rooms
                    if (gr.grid[x, y, z].type == RNode_Type.Boss)
                    {
                        boss_Rooms.Add(gr.grid[x, y, z]);
                    }

                    // Check the spawn Rooms
                    if (gr.grid[x, y, z].type == RNode_Type.Spawn)
                    {
                        spawn_Rooms.Add(gr.grid[x, y, z]);
                    }
                }
            }
        }

        if (boss_Rooms.Count == boss_RoomsCant && spawn_Rooms.Count == spawn_RoomsCant)
        {
            return;
        }
        else
        {
            if (boss_Rooms.Count > boss_RoomsCant || spawn_Rooms.Count > spawn_RoomsCant)
            {
                while (boss_Rooms.Count > boss_RoomsCant)
                {
                    List<RNode_Type> randList;

                    int rand = UnityEngine.Random.Range(0, boss_Rooms.Count);
                    randList = wave.GetNodePossibilities(boss_Rooms[rand]);
                    randList.Remove(RNode_Type.Boss);
                    randList.Remove(RNode_Type.Spawn);

                    int rand2 = UnityEngine.Random.Range(0, randList.Count);
                    gr.grid[boss_Rooms[rand].gridpos.x, boss_Rooms[rand].gridpos.y, boss_Rooms[rand].gridpos.z].type = randList[rand2];

                    boss_Rooms.RemoveAt(rand);
                }
                boss_Rooms.Clear();

                while (spawn_Rooms.Count > spawn_RoomsCant)
                {
                    List<RNode_Type> randList;

                    int rand = UnityEngine.Random.Range(0, spawn_Rooms.Count);
                    randList = wave.GetNodePossibilities(spawn_Rooms[rand]);
                    randList.Remove(RNode_Type.Boss);
                    randList.Remove(RNode_Type.Spawn);

                    int rand2 = UnityEngine.Random.Range(0, randList.Count);
                    gr.grid[spawn_Rooms[rand].gridpos.x, spawn_Rooms[rand].gridpos.y, spawn_Rooms[rand].gridpos.z].type = randList[rand2];

                    spawn_Rooms.RemoveAt(rand);
                }
                spawn_Rooms.Clear();
            }
            else
            {
                wave.StartWFC();
                CheckRoomCant();
            }
        }
    }

    /// <summary>
    /// Instantiate The Rooms Prefab
    /// </summary>
    private void InstantiatePrefabs()
    {
        for (int x = 0; x < gr.grid.GetLength(0); x++)
        {
            for (int y = 0; y < gr.grid.GetLength(1); y++)
            {
                for (int z = 0; z < gr.grid.GetLength(2); z++)
                {
                    int index;
                    Vector3 wordPOs = new Vector3();
                    wordPOs.x = gr.grid[x, y, z].pos.x * ofset.x;
                    wordPOs.y = gr.grid[x, y, z].pos.y * ofset.y;
                    wordPOs.z = gr.grid[x, y, z].pos.z * ofset.z;

                    switch (gr.grid[x, y, z].type)
                    {
                        case RNode_Type.Empty:
                            break;
                        case RNode_Type.Spawn:
                            index = UnityEngine.Random.Range(0, spawn_rooms_Prefab.Count);
                            Instantiate(spawn_rooms_Prefab[index], wordPOs, Quaternion.identity, gridHolder);
                            break;
                        case RNode_Type.Trap:
                            index = UnityEngine.Random.Range(0, trap_rooms_Prefab.Count);
                            Instantiate(trap_rooms_Prefab[index], wordPOs, Quaternion.identity, gridHolder);
                            break;
                        case RNode_Type.Normal:
                            index = UnityEngine.Random.Range(0, normal_rooms_Prefab.Count);
                            Instantiate(normal_rooms_Prefab[index], wordPOs, Quaternion.identity, gridHolder);
                            break;
                        case RNode_Type.Boss:
                            index = UnityEngine.Random.Range(0, boss_rooms_Prefab.Count);
                            Debug.LogWarning("Boss Room Count " + boss_rooms_Prefab.Count);
                            Debug.LogWarning("Boss Room Index " + index);
                            Instantiate(boss_rooms_Prefab[index], wordPOs, Quaternion.identity, gridHolder);
                            break;
                        default:
                            Debug.LogError("No Rooms To spawn");
                            break;
                    }
                }
            }
        }
    }
}
