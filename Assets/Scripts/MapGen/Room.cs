using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // Direction Ordering: North South East West
    //public Vector2 gridPos;
    //public int type;
    public List<RoomInfo> exits = new List<RoomInfo>();
    public Vector3[] spawnPositions;
}

[Serializable]
public class RoomInfo
{
    public Direction exitDir;
    public Vector3 spawnPos;
}
