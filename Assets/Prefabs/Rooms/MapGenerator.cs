using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MapGenerator : MonoBehaviour
{
    [SerializeField] int numRooms;
    RoomTemplates1 roomTemplates;

    void Start()
    {
        roomTemplates = GetComponent<RoomTemplates1>();
        //GenerateLevel();
    }

    private void GenerateLevel()
    {
        // Instantiate starting room (all 4 sides are open)
        Instantiate(roomTemplates.rooms[0], Vector3.zero, Quaternion.identity);

        // iterate through each opening and add a random possible room until no openings remain
    }
}
