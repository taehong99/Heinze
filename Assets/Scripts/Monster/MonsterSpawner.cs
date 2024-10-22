using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] MonsterPoolSO monsterPool;
    [SerializeField] MonsterGroup monsterGroup;
    GameObject[] EnemyPrefabs;
    [SerializeField] int enemiesToSpawn;
    [SerializeField] float xRadius;
    [SerializeField] float zRadius;

    private float xOffset;
    private float zOffset;
    private int enemyCount;

    private void Start()
    {
        if (monsterGroup == MonsterGroup.MidBoss1 || monsterGroup == MonsterGroup.Boss1)
            return;

        switch (monsterGroup)
        {
            case MonsterGroup.All:
                EnemyPrefabs = monsterPool.Stage1All;
                break;
            case MonsterGroup.Dolguns:
                EnemyPrefabs = monsterPool.Stage1Dolguns;
                break;
            case MonsterGroup.Bees:
                EnemyPrefabs = monsterPool.Stage1Bees;
                break;
            case MonsterGroup.Buseuls:
                EnemyPrefabs = monsterPool.Stage1Buseuls;
                break;
            case MonsterGroup.Slimes:
                EnemyPrefabs = monsterPool.Stage1Slimes;
                break;
            case MonsterGroup.Trees:
                EnemyPrefabs = monsterPool.Stage1Trees;
                break;
            default:
                break;
        }
        //SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        if (monsterGroup == MonsterGroup.MidBoss1)
        {
            Instantiate(monsterPool.Stage1MidBoss, transform.position, Quaternion.identity);
            return;
        }
        else if(monsterGroup == MonsterGroup.Boss1)
        {
            Instantiate(monsterPool.Stage1Boss, transform.position, Quaternion.identity);
            return;
        }

        Vector3 spawnPos = new Vector3();
        for(int i = 0; i < enemiesToSpawn; i++)
        {
            xRadius /= 2;
            zRadius /= 2;
            xOffset = Random.Range(-xRadius, xRadius);
            zOffset = Random.Range(-zRadius, zRadius);
            spawnPos = new Vector3(transform.position.x + xOffset, 0, transform.position.z + zOffset);
            Instantiate(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)], spawnPos, Quaternion.identity);
            Manager.Event.voidEventDic["enemySpawned"].RaiseEvent();
        }
    }
}

