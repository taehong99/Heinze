using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { North, South, East, West }
public enum Stage { Normal, MidBoss, Boss }

[RequireComponent(typeof(RoomTemplates))]
public class MapGenerator : MonoBehaviour
{
    [SerializeField] int numRooms;
    [SerializeField] float roomOffset;
    public float RoomOffset => roomOffset;
    RoomTemplates templates;

    Dictionary<Vector3, Room> rooms = new Dictionary<Vector3, Room>();
    HashSet<Vector3> takenPositions = new HashSet<Vector3>();

    //int numRooms;
    int CCount;
    int DCount;

    void Start()
    {
        templates = GetComponent<RoomTemplates>();
    }

    public void GenerateMap(Stage stage)
    {
        rooms.Clear();
        takenPositions.Clear();
        GenerateLevel(stage);
        ConnectRooms();
    }

    public void EnterFirstRoom()
    {
        rooms[Vector3.zero].EnterRoom();
    }

    // Utils
    private Queue<char> CreateRoomPool()
    {
        int originalRooms = numRooms;
        List<char> order = new List<char>();
        CCount = 1; //Random.Range(1, 3);
        DCount = 1; //Random.Range(1, 3);
        numRooms -= (CCount + DCount);

        for (int i = 0; i < numRooms / 2; i++)
        {
            order.Add('A');
            order.Add('B');
        }
        for(int i = 0; i < CCount; i++)
        {
            order.Add('C');
        }
        for (int i = 0; i < DCount; i++)
        {
            order.Add('D');
        }

        ShuffleList(order);
        while (!(order[0] == 'A' || order[0] == 'B')) // Ensure first room is A or B
        {
            ShuffleList(order);
        }
        numRooms = originalRooms;
        return new Queue<char>(order);
    }

    private void ShuffleList(List<char> rooms)
    {
        int n = rooms.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            char temp = rooms[i];
            rooms[i] = rooms[j];
            rooms[j] = temp;
        }
    }

    private void GenerateLevel(Stage stage)
    {
        // Queue to check which type of room to instantiate next
        Queue<char> roomPool = CreateRoomPool();
        Debug.Log(roomPool.Count);

        // Instantiate starting room with 2 ~ 4 doors
        /*Room curRoom = templates.roomA.GetComponent<Room>();
        PlaceRoom(templates.roomA, Vector3.zero);*/
        char roomType = roomPool.Dequeue();
        if(roomType == 'A')
        {
            PlaceRoom(templates.roomA[Random.Range(0, templates.roomA.Length)], Vector3.zero);
        }
        else
        {
            PlaceRoom(templates.roomB[Random.Range(0, templates.roomB.Length)], Vector3.zero);
        }

        // Add openings to queue
        Queue<Vector3> spawnPoints = new Queue<Vector3>();
        spawnPoints.Enqueue(GetAdjPos(Vector3.zero, Direction.West));
        spawnPoints.Enqueue(GetAdjPos(Vector3.zero, Direction.East));
        if (Random.Range(0f, 1f) <= 0.5f)
        {
            spawnPoints.Enqueue(GetAdjPos(Vector3.zero, Direction.North));
        }
        if (Random.Range(0f, 1f) <= 0.5f)
        {
            spawnPoints.Enqueue(GetAdjPos(Vector3.zero, Direction.South));
        }

        // Create N open rooms
        while (roomPool.Count > 0)
        {
            Vector3 nextPos = spawnPoints.Dequeue();
            if (takenPositions.Contains(nextPos))
                continue;

            char nextRoomType = roomPool.Dequeue();
            GameObject nextRoom;
            switch (nextRoomType)
            {
                case 'A':
                    nextRoom = templates.roomA[Random.Range(0, templates.roomA.Length)];
                    break;
                case 'B':
                    nextRoom = templates.roomB[Random.Range(0, templates.roomB.Length)];
                    break;
                case 'C':
                    nextRoom = templates.roomC[Random.Range(0, templates.roomC.Length)];
                    break;
                default:
                    nextRoom = templates.roomD[Random.Range(0, templates.roomD.Length)];
                    break;
            }
            PlaceRoom(nextRoom, nextPos);
            
            int numOpenings = Random.Range(1, 4);
            int count = 0;
            while (count < numOpenings) {
                Direction dir = (Direction)count;
                if(takenPositions.Contains(GetAdjPos(nextPos, dir))){
                    count++;
                    continue;
                }

                spawnPoints.Enqueue(GetAdjPos(nextPos, dir));
                count++;
            }
        }

        // Create Boss Room if needed
        Vector3 furthestPoint = Vector3.zero;
        while (spawnPoints.Count > 1)
        {
            Vector3 cur = spawnPoints.Dequeue();
            furthestPoint = cur.sqrMagnitude > furthestPoint.sqrMagnitude ? cur : furthestPoint;
        }

        if(stage == Stage.MidBoss)
        {
            PlaceRoom(templates.roomMidBoss, furthestPoint);
        }
        else if(stage == Stage.Boss)
        {
            PlaceRoom(templates.roomBoss, furthestPoint);
        }
        else
        {
            PlaceRoom(templates.roomStairs, furthestPoint);
        }
    }

    private void PlaceRoom(GameObject room, Vector3 position)
    {
        Room newRoom = Instantiate(room, position, Quaternion.identity).GetComponent<Room>();
        rooms.Add(position, newRoom);
        takenPositions.Add(position);
    }

    private Vector3 GetAdjPos(Vector3 origin, Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return origin + Vector3.forward * roomOffset;
            case Direction.South:
                return origin + Vector3.back * roomOffset;
            case Direction.East:
                return origin + Vector3.right * roomOffset;
            default:
                return origin + Vector3.left * roomOffset;
        }
    }

    private void ConnectRooms()
    {
        foreach (var entry in rooms)
        {
            if (entry.Value.RoomType != Stage.Normal)
            {
                continue;
            }

            Vector3 curPos = entry.Key;
            for(int i = 0; i < 4; i++)
            {
                if (rooms.ContainsKey(GetAdjPos(curPos, (Direction)i)))
                {
                    // Break Gate/Wall
                    //entry.Value.OpenGate((Direction)i);

                    // Create Portal to neighbor
                    //entry.Value.ActivatePortal((Direction)i);
                    entry.Value.ConnectPortal((Direction)i, rooms[GetAdjPos(curPos, (Direction)i)].portals[(int)Extension.GetOppositeDirection((Direction)i)]);
                }
            }
        }
    }
}
