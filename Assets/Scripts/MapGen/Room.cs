using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] Stage roomType;
    public Stage RoomType => roomType;

    // Direction Ordering: North South East West
    public Portal[] portals = new Portal[4];

    GenerateEnemies[] spawners;
    private int monsterCount = 0;

    private void Start()
    {
        spawners = GetComponentsInChildren<GenerateEnemies>();
    }

    void AddCount() => monsterCount++;
    void SubtractCount() => monsterCount--;
    public void EnterRoom()
    {
        Manager.Event.voidEventDic["enemySpawned"].OnEventRaised += AddCount;
        Manager.Event.voidEventDic["enemyDied"].OnEventRaised += SubtractCount;
        SpawnMonsters();
        StartCoroutine(RoomBattleRoutine());
    }

    IEnumerator RoomBattleRoutine()
    {
        while(monsterCount > 0)
        {
            yield return null;
        }
        RoomCleared();
    }

    private void SpawnMonsters()
    {
        foreach (var spawner in spawners)
        {
            spawner.SpawnEnemies();
        }
    }

    private void RoomCleared()
    {
        Manager.Game.SpawnChest();
        ActivatePortals();
        Manager.Event.voidEventDic["enemySpawned"] = null;
        Manager.Event.voidEventDic["enemyDied"] = null;
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