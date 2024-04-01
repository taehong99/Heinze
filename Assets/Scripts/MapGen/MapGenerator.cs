using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LevelGenerator;
public class MapGenerator : MonoBehaviour
{
    [SerializeField] int numRooms;
    [SerializeField] float roomOffset;
    RoomTemplates1 roomTemplates;

    void Start()
    {
        roomTemplates = GetComponent<RoomTemplates1>();
        Instantiate(roomTemplates.rooms[0], Vector3.zero, Quaternion.identity);
        //GenerateLevel();
    }

    private void GenerateLevel()
    {
        // Instantiate starting room (all 4 sides are open)
        Instantiate(roomTemplates.rooms[0], Vector3.zero, Quaternion.identity);
        Queue<EmptySlot> opens = new Queue<EmptySlot>();
        opens.Enqueue(new EmptySlot(GetAdjPos(Vector3.zero, Direction.North), Direction.South));
        opens.Enqueue(new EmptySlot(GetAdjPos(Vector3.zero, Direction.East), Direction.West));

        // Create N open rooms
        for (int i = 0; i < numRooms; i++)
        {
            int rand;
            Room2 roomToAdd;
            EmptySlot curSlot = opens.Dequeue();
            switch (curSlot.openDir)
            {
                case Direction.South:
                    rand = Random.Range(0, roomTemplates.rooms.Length);
                    roomToAdd = roomTemplates.rooms[rand].GetComponent<Room2>();
                    Instantiate(roomTemplates.rooms[rand], curSlot.pos, Quaternion.identity);
                    opens.Enqueue(new EmptySlot(GetAdjPos(curSlot.pos, Direction.North), Direction.South));
                    opens.Enqueue(new EmptySlot(GetAdjPos(curSlot.pos, Direction.East), Direction.West));
                    break;
                case Direction.West:
                    rand = Random.Range(0, roomTemplates.rooms.Length);
                    roomToAdd = roomTemplates.rooms[rand].GetComponent<Room2>();
                    Instantiate(roomTemplates.rooms[rand], curSlot.pos, Quaternion.identity);
                    opens.Enqueue(new EmptySlot(GetAdjPos(curSlot.pos, Direction.North), Direction.South));
                    opens.Enqueue(new EmptySlot(GetAdjPos(curSlot.pos, Direction.East), Direction.West));
                    break;
                default:
                    break;
            }
            Debug.Log($"Room instantiated at: {curSlot.pos}");
        }

        // Create Boss Room
        while(opens.Count > 1)
        {
            opens.Dequeue();
        }
        EmptySlot bossSlot = opens.Dequeue();
        Instantiate(roomTemplates.bossRoom, bossSlot.pos, Quaternion.identity);
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
}
