using System.Collections.Generic;
using UnityEngine;

enum Directionss
{
    UP,
    DOWN,
    RIGHT,
    LEFT
}

/// <summary>
/// Clas To Manage The Room Behaviour
/// </summary>
public class Room_Behaviour : MonoBehaviour
{
    [Header("Room SetUp")]
    [SerializeField] private GameObject[] walls;      // 0 = Up - 1 = Down - 2 = Right - 3 = Left
    [SerializeField] private GameObject[] entrance;      // 0 = Up - 1 = Down - 2 = Right - 3 = Left    
    [SerializeField] private GameObject[] doors;      // 0 = Up - 1 = Down - 2 = Right - 3 = Left    
    [SerializeField] private List<GameObject> props;
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] public Transform playerPos;
    [SerializeField] public Transform playerContainer;
    [SerializeField] private float updateTimer = 2.0f;
    [Header("Room Points")]
    [SerializeField] private const int maxRoomPoints = 8;
    [SerializeField] private const int maxRoomPlanes = 4;
    [SerializeField] private Transform[] roomPoints = new Transform[maxRoomPoints];
    [SerializeField] private Plane[] roomPlanes = new Plane[maxRoomPlanes];
    [Header("Rooms Checks")]
    [SerializeField] private bool isRoomVisible;
    [SerializeField] private bool isRoomCheked;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private float rayDistance;
    [SerializeField] List<Room_Behaviour> adjRooms;



    private void Start()
    {

        for (int i = 0; i < maxRoomPlanes; i++)
        {
            roomPlanes[i] = new Plane();
        }
        roomPlanes[0].Set3Points(roomPoints[0].position, roomPoints[1].position, roomPoints[3].position);
        roomPlanes[1].Set3Points(roomPoints[4].position, roomPoints[0].position, roomPoints[2].position);
        roomPlanes[2].Set3Points(roomPoints[5].position, roomPoints[4].position, roomPoints[6].position);
        roomPlanes[3].Set3Points(roomPoints[1].position, roomPoints[5].position, roomPoints[7].position);
    }

    private void Update()
    {
        if (isRoomVisible)
        {
            Show();
            Check_Enemies_In_Room();
        }
        else
        {
            Hide();
        }

        if (isRoomCheked)
        {
            updateTimer -= Time.deltaTime;
            if (updateTimer < 0.0f)
            {
                updateTimer = 2.0f;
                isRoomCheked = false;
                isRoomVisible = false;
            }
        }
    }
    public bool isPointInside(Vector3 pos)
    {
        for (int i = 0; i < roomPlanes.Length; i++)
        {
            if (!roomPlanes[i].GetSide(pos))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Set a Room Visibility
    /// </summary>
    /// <param name="value"></param>
    public void SetRoomVisible(bool value)
    {
        isRoomVisible = value;
    }

    /// <summary>
    /// Set if We Already Check The Current Room
    /// </summary>
    /// <param name="value"></param>
    public void SetRoomCheked(bool value)
    {
        isRoomCheked = value;
    }

    /// <summary>
    /// Get if the Room is Visible
    /// </summary>
    /// <returns></returns>
    public bool GetRoomVisible()
    {
        return isRoomVisible;
    }

    /// <summary>
    /// Get if the Room is Cheked
    /// </summary>
    /// <returns></returns>
    public bool GetRoomCheked()
    {
        return isRoomCheked;
    }

    /// <summary>
    /// Update The Door && Walls Activation
    /// </summary>
    /// <param name="status"></param>
    public void UpdateRoom(bool[] status)
    {
        for (int i = 0; i < status.Length; i++)
        {
            entrance[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }

    /// <summary>
    /// Check if There are Enemies in the Current Room
    /// </summary>
    public void Check_Enemies_In_Room()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
                enemies.Remove(enemies[i]);
        }

        if (enemies.Count <= 0)
        {
            for (int i = 0; i < entrance.Length; i++)
            {
                if (doors[i].activeInHierarchy)
                    doors[i].transform.position += Vector3.up;
            }
            Open_Adj_Doors();
        }

    }

    /// <summary>
    /// Open the Connected Doors of the Adjacent Rooms
    /// </summary>
    public void Open_Adj_Doors()
    {
        if (adjRooms.Count >= (int)Directionss.UP)
        {
            if (doors[(int)Directionss.UP].activeInHierarchy)
            {
                if (doors.Length >= (int)Directionss.UP)
                {
                    if (adjRooms[(int)Directionss.UP].doors[(int)Directionss.DOWN] != null && adjRooms[(int)Directionss.UP].doors[(int)Directionss.DOWN].activeInHierarchy)
                        adjRooms[(int)Directionss.UP].doors[(int)Directionss.DOWN].transform.position += Vector3.up;
                }
            }
        }

        if (adjRooms.Count > (int)Directionss.DOWN)
        {
            if (doors[(int)Directionss.DOWN].activeInHierarchy)
            {
                if (doors.Length > (int)Directionss.DOWN)
                {
                    if (adjRooms[(int)Directionss.DOWN].doors[(int)Directionss.UP] != null && adjRooms[(int)Directionss.DOWN].doors[(int)Directionss.UP].activeInHierarchy)
                        adjRooms[(int)Directionss.DOWN].doors[(int)Directionss.UP].transform.position += Vector3.up;
                }
            }
        }

        if (adjRooms.Count > (int)Directionss.RIGHT)
        {
            if (doors[(int)Directionss.RIGHT].activeInHierarchy)
            {
                if (doors.Length >= (int)Directionss.RIGHT)
                {
                    if (adjRooms[(int)Directionss.RIGHT].doors[(int)Directionss.LEFT] != null && adjRooms[(int)Directionss.RIGHT].doors[(int)Directionss.LEFT].activeInHierarchy)
                        adjRooms[(int)Directionss.RIGHT].doors[(int)Directionss.LEFT].transform.position += Vector3.up;
                }
            }
        }

        if (adjRooms.Count > (int)Directionss.LEFT + 1 )
        {
            if (doors[(int)Directionss.LEFT].activeInHierarchy)
            {
                if (doors.Length >= (int)Directionss.LEFT)
                {
                    if (adjRooms[(int)Directionss.LEFT].doors[(int)Directionss.RIGHT] != null && adjRooms[(int)Directionss.LEFT].doors[(int)Directionss.RIGHT].activeInHierarchy)
                        adjRooms[(int)Directionss.LEFT].doors[(int)Directionss.RIGHT].transform.position += Vector3.up;
                }
            }
        }
    }

    /// <summary>
    /// Set the Neighbors of the Current Room
    /// </summary>
    public void SetAdjRooms()
    {
        RaycastHit hit;

        if (entrance[(int)Directionss.UP].activeInHierarchy)
        {
            if (Physics.Raycast(rayOrigin.position, Vector3.forward, out hit, rayDistance))
            {
                hit.collider.GetComponentInParent<Room_Behaviour>().entrance[(int)Directionss.DOWN].SetActive(true);
                hit.collider.GetComponentInParent<Room_Behaviour>().walls[(int)Directionss.DOWN].SetActive(false);
                adjRooms.Add(hit.collider.GetComponentInParent<Room_Behaviour>());
            }
        }

        if (entrance[(int)Directionss.DOWN].activeInHierarchy)
        {
            if (Physics.Raycast(rayOrigin.position, -Vector3.forward, out hit, rayDistance))
            {
                hit.collider.GetComponentInParent<Room_Behaviour>().entrance[(int)Directionss.UP].SetActive(true);
                hit.collider.GetComponentInParent<Room_Behaviour>().walls[(int)Directionss.UP].SetActive(false);
                adjRooms.Add(hit.collider.GetComponentInParent<Room_Behaviour>());
            }
        }

        if (entrance[(int)Directionss.RIGHT].activeInHierarchy)
        {
            if (Physics.Raycast(rayOrigin.position, Vector3.right, out hit, rayDistance))
            {
                hit.collider.GetComponentInParent<Room_Behaviour>().entrance[(int)Directionss.LEFT].SetActive(true);
                hit.collider.GetComponentInParent<Room_Behaviour>().walls[(int)Directionss.LEFT].SetActive(false);
                adjRooms.Add(hit.collider.GetComponentInParent<Room_Behaviour>());
            }
        }

        if (entrance[(int)Directionss.LEFT].activeInHierarchy)
        {
            if (Physics.Raycast(rayOrigin.position, Vector3.left, out hit, rayDistance))
            {
                hit.collider.GetComponentInParent<Room_Behaviour>().entrance[(int)Directionss.RIGHT].SetActive(true);
                hit.collider.GetComponentInParent<Room_Behaviour>().walls[(int)Directionss.RIGHT].SetActive(false);
                adjRooms.Add(hit.collider.GetComponentInParent<Room_Behaviour>());
            }
        }
    }

    /// <summary>
    /// Show the neighbors of his Room
    /// </summary>
    public void ShowAdjRooms()
    {
        foreach (var item in adjRooms)
        {
            item.SetRoomCheked(true);
            item.SetRoomVisible(true);
        }
    }

    /// <summary>
    /// Hide The Current Room
    /// </summary>
    public void Hide()
    {
        MeshRenderer[] mesh = GetComponentsInChildren<MeshRenderer>();


        for (int i = 0; i < mesh.Length; i++)
        {
            mesh[i].enabled = false;
        }

        if (enemies != null)
        {

            if (enemies.Count > 0)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i] != null)
                        enemies[i].active = false;
                }
            }
        }

        if (props != null)
        {
            if (props.Count > 0)
            {
                for (int i = 0; i < props.Count; i++)
                {
                    if (props[i] != null)
                        props[i].active = false;
                }
            }
        }
    }

    /// <summary>
    /// Show The Current Room
    /// </summary>
    public void Show()
    {
        MeshRenderer[] mesh = GetComponentsInChildren<MeshRenderer>();


        for (int i = 0; i < mesh.Length; i++)
        {
            mesh[i].enabled = true;
        }

        if (enemies != null)
        {
            if (enemies.Count > 0)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i] != null)
                        enemies[i].active = true;
                }
            }
        }

        if (props != null)
        {
            if (props.Count > 0)
            {

                for (int i = 0; i < props.Count; i++)
                {
                    if (props[i] != null)
                        props[i].active = true;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(rayOrigin.position, Vector3.forward * rayDistance);
        Gizmos.DrawRay(rayOrigin.position, -Vector3.forward * rayDistance);
        Gizmos.DrawRay(rayOrigin.position, Vector3.left * rayDistance);
        Gizmos.DrawRay(rayOrigin.position, Vector3.right * rayDistance);
    }
}
