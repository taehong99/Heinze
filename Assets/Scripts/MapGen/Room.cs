using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class Room : MonoBehaviour
{
    [SerializeField] Stage roomType;
    [SerializeField] Stairs stairs;
    public Stage RoomType => roomType;

    // Direction Ordering: North South East West
    public Portal[] portals = new Portal[4];

    NavMeshSurface navMeshSurface;
    MonsterSpawner[] spawners;
    private int monsterCount = 0;
    bool cleared = false;
    bool bossDefeated = false;

    private void Awake()
    {
        spawners = GetComponentsInChildren<MonsterSpawner>();
        navMeshSurface = GetComponentInChildren<NavMeshSurface>();
    }

    private void Start()
    {
        navMeshSurface.BuildNavMesh();

        if (roomType == Stage.MidBoss || roomType == Stage.Boss)
        {
            Manager.Event.voidEventDic["bossDefeated"].OnEventRaised += OnBossDefeat;
        }
    }

    private void OnBossDefeat()
    {
        bossDefeated = true;
        Manager.Event.voidEventDic["bossDefeated"].OnEventRaised -= OnBossDefeat;
    }

    void AddCount() => monsterCount++;
    void SubtractCount() => monsterCount--;
    public void EnterRoom()
    {
        if (cleared)
            return;

        if(roomType == Stage.MidBoss)
            Manager.Sound.PlayBGM(Manager.Sound.AudioClips.midbossBGM);
        else if(roomType == Stage.Boss)
            Manager.Sound.PlayBGM(Manager.Sound.AudioClips.bossBGM);

        Manager.Event.voidEventDic["enemySpawned"].OnEventRaised += AddCount;
        Manager.Event.voidEventDic["enemyDied"].OnEventRaised += SubtractCount;
        SpawnMonsters();
        StartCoroutine(RoomBattleRoutine());
    }

    IEnumerator RoomBattleRoutine()
    {
        if(roomType == Stage.MidBoss || roomType == Stage.Boss)
        {
            while (bossDefeated == false)
            {
                yield return null;
            }
        }
        else
        {
            while (monsterCount > 0)
            {
                yield return null;
            }
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
        cleared = true;
        Manager.Game.SpawnChest();
        ActivatePortals();
        Manager.Event.voidEventDic["enemySpawned"].OnEventRaised -= AddCount;
        Manager.Event.voidEventDic["enemyDied"].OnEventRaised -= SubtractCount;

        if(roomType == Stage.MidBoss || roomType == Stage.Boss)
        {
            stairs.gameObject.SetActive(true);
        }
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
        portals[(int)direction].direction = direction;
        neighbor.direction = Extension.GetOppositeDirection(direction);
    }
}