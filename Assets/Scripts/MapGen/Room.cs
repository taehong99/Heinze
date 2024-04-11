using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] Stage roomType;
    //[SerializeField] GenerateEnemies[] spawners;
    public Stage RoomType => roomType;

    // Direction Ordering: North South East West
    //public List<RoomInfo> exits = new List<RoomInfo>();
    //public Transform[] gates = new Transform[4];
    //public Transform[] spawnPositions = new Transform[4];
    public Portal[] portals = new Portal[4];

    public void SpawnMonsters()
    {
        //foreach(var spawner in spawners){
        //  spawner.SpawnMonsters();
        //}
    }

    public void ActivatePortal(Direction direction)
    {
        portals[(int)direction].gameObject.SetActive(true);
    }

    public void ActivatePortals()
    {
        foreach(Portal portal in portals)
        {
            if(portal.destination != null)
            {
                portal.gameObject.SetActive(true);
            }
        }
    }

    public void ConnectPortal(Direction direction, Portal neighbor)
    {
        portals[(int)direction].destination = neighbor;
    }
}