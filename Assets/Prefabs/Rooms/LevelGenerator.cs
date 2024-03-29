using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { North, South, East, West }
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] int numRooms;
    [SerializeField] GameObject startingRoomPrefab;
    RoomTemplates roomTemplates;

    HashSet<Vector2> roomPresent = new HashSet<Vector2>();

    private void Start()
    {
        roomTemplates = GetComponent<RoomTemplates>();
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        // Instantiate starting room (all 4 sides are open)
        Instantiate(startingRoomPrefab, Vector3.zero, Quaternion.identity);
        Queue<EmptySlot> opens = new Queue<EmptySlot>();
        opens.Enqueue(new EmptySlot(GetAdjPos(Vector3.zero, Direction.North), Direction.South));
        opens.Enqueue(new EmptySlot(GetAdjPos(Vector3.zero, Direction.South), Direction.North));
        opens.Enqueue(new EmptySlot(GetAdjPos(Vector3.zero, Direction.East), Direction.West));
        opens.Enqueue(new EmptySlot(GetAdjPos(Vector3.zero, Direction.West), Direction.East));

        // Create N open rooms
        for (int i = 0; i < numRooms; i++)
        {
            int rand;
            Room roomToAdd;
            EmptySlot curSlot = opens.Dequeue();
            switch (curSlot.openDir)
            {
                case Direction.North:
                    rand = Random.Range(0, roomTemplates.northRooms.Length);
                    roomToAdd = roomTemplates.northRooms[rand].GetComponent<Room>();
                    Instantiate(roomTemplates.northRooms[rand], curSlot.pos, Quaternion.identity);
                    if (roomToAdd.doorEast)
                        opens.Enqueue(new EmptySlot(GetAdjPos(curSlot.pos, Direction.East), Direction.West));
                    if (roomToAdd.doorWest)
                        opens.Enqueue(new EmptySlot(GetAdjPos(curSlot.pos, Direction.West), Direction.East));
                    if (roomToAdd.doorSouth)
                        opens.Enqueue(new EmptySlot(GetAdjPos(curSlot.pos, Direction.South), Direction.North));
                    break;
                case Direction.South:
                    rand = Random.Range(0, roomTemplates.southRooms.Length);
                    roomToAdd = roomTemplates.southRooms[rand].GetComponent<Room>();
                    Instantiate(roomTemplates.southRooms[rand], curSlot.pos, Quaternion.identity);
                    if (roomToAdd.doorEast)
                        opens.Enqueue(new EmptySlot(GetAdjPos(curSlot.pos, Direction.East), Direction.West));
                    if (roomToAdd.doorWest)
                        opens.Enqueue(new EmptySlot(GetAdjPos(curSlot.pos, Direction.West), Direction.East));
                    if (roomToAdd.doorNorth)
                        opens.Enqueue(new EmptySlot(GetAdjPos(curSlot.pos, Direction.North), Direction.South));
                    break;
                case Direction.East:
                    rand = Random.Range(0, roomTemplates.eastRooms.Length);
                    roomToAdd = roomTemplates.eastRooms[rand].GetComponent<Room>();
                    Instantiate(roomTemplates.eastRooms[rand], curSlot.pos, Quaternion.identity);
                    if (roomToAdd.doorSouth)
                        opens.Enqueue(new EmptySlot(GetAdjPos(curSlot.pos, Direction.South), Direction.North));
                    if (roomToAdd.doorWest)
                        opens.Enqueue(new EmptySlot(GetAdjPos(curSlot.pos, Direction.West), Direction.East));
                    if (roomToAdd.doorNorth)
                        opens.Enqueue(new EmptySlot(GetAdjPos(curSlot.pos, Direction.North), Direction.South));
                    break;
                case Direction.West:
                    rand = Random.Range(0, roomTemplates.westRooms.Length);
                    roomToAdd = roomTemplates.westRooms[rand].GetComponent<Room>();
                    Instantiate(roomTemplates.westRooms[rand], curSlot.pos, Quaternion.identity);
                    if (roomToAdd.doorSouth)
                        opens.Enqueue(new EmptySlot(GetAdjPos(curSlot.pos, Direction.South), Direction.North));
                    if (roomToAdd.doorEast)
                        opens.Enqueue(new EmptySlot(GetAdjPos(curSlot.pos, Direction.East), Direction.West));
                    if (roomToAdd.doorNorth)
                        opens.Enqueue(new EmptySlot(GetAdjPos(curSlot.pos, Direction.North), Direction.South));
                    break;
                default:
                    break;
            }
            Debug.Log($"Room instantiated at: {curSlot.pos}");
        }

        // Close all remaining openings with dead end rooms
        while (opens.Count > 0)
        {
            EmptySlot curSlot = opens.Dequeue();
            switch (curSlot.openDir)
            {
                case Direction.North:
                    Instantiate(roomTemplates.northDeadEnd, curSlot.pos, Quaternion.identity);
                    break;
                case Direction.South:
                    Instantiate(roomTemplates.southDeadEnd, curSlot.pos, Quaternion.identity);
                    break;
                case Direction.East:
                    Instantiate(roomTemplates.eastDeadEnd, curSlot.pos, Quaternion.identity);
                    break;
                case Direction.West:
                    Instantiate(roomTemplates.westDeadEnd, curSlot.pos, Quaternion.identity);
                    break;
            }
        }

        // iterate through each opening and add a random possible room until no openings remain
    }

    private void AddOpens(Room room, Queue<EmptySlot> opens)
    {
        
    }

    private Vector3 GetAdjPos(Vector3 origin, Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return origin + Vector3.forward * 3;
            case Direction.South:
                return origin + Vector3.back * 3;
            case Direction.East:
                return origin + Vector3.right * 3;
            case Direction.West:
                return origin + Vector3.left * 3;
            default:
                return Vector3.zero;
        }
    }

    public struct EmptySlot
    {
        public Vector3 pos;       // Room placed at pos
        public Direction openDir; // Room requires opening at openDir

        public EmptySlot(Vector3 pos, Direction openDir)
        {
            this.pos = pos;
            this.openDir = openDir;
        }
    }
}
