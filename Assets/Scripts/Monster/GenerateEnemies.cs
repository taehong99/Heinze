using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    [SerializeField] MonsterPoolSO monsterPool;
    [SerializeField] MonsterGroup monsterGroup;
    GameObject[] EnemyPrefabs;
    [SerializeField] int enemiesToSpawn;
    [SerializeField] int xRadius;
    [SerializeField] int zRadius;

    private int xOffset;
    private int zOffset;
    private int enemyCount;
    HashSet<Vector3> spawnedPositions = new HashSet<Vector3>();

    private void Start()
    {
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
        }
        //SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        Debug.Log(EnemyPrefabs == null);
        Vector3 spawnPos = new Vector3();
        for(int i = 0; i < enemiesToSpawn; i++)
        {
            while (spawnedPositions.Contains(spawnPos))
            {
                xOffset = Random.Range(-xRadius, xRadius + 1);
                zOffset = Random.Range(-zRadius, zRadius + 1);
                spawnPos = new Vector3(transform.position.x + xOffset, 0, transform.position.z + zOffset);
            }

            spawnedPositions.Add(spawnPos);
            Instantiate(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)], spawnPos, Quaternion.identity);
            Manager.Event.voidEventDic["enemySpawned"].RaiseEvent();
        }
    }
}

