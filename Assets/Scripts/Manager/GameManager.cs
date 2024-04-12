using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    // Components
    [SerializeField] GameObject warriorPrefab;

    // Player Skills
    private PlayerAttack playerAttack;
    private PlayerSkillDataSO[] skillSlots = new PlayerSkillDataSO[4];
    private bool hasPassive;

    // Player Buffs
    private List<PlayerBuffSO> buffs = new List<PlayerBuffSO>();
    public List<PlayerBuffSO> Buffs => buffs;

    // MapGen
    MapGenerator mapGenerator;
    CardDeck deck;

    // Events
    public event Action<PlayerSkillDataSO> SkillPicked;
    public event Action<PlayerBuffSO> BuffPicked;
    public event Action<ConsumableItemSO> ItemPicked;

    private void Start()
    {
        mapGenerator = GetComponentInChildren<MapGenerator>();
    }

    #region Player Skills
    public void AssignPlayer(PlayerAttack player)
    {
        playerAttack = player;
    }

    public float GetCooldownRatio(int id)
    {
        if (playerAttack == null)
            return 0;

        return playerAttack.GetCooldownRatio(id);
    }

    public void AnnounceSkillPicked(PlayerSkillDataSO data)
    {
        SkillPicked?.Invoke(data);
    }
    public void AnnounceBuffPicked(PlayerBuffSO data)
    {
        buffs.Add(data);
        BuffPicked?.Invoke(data);
    }
    public void AnnounceItemPicked(ConsumableItemSO data)
    {
        ItemPicked?.Invoke(data);
    }

    public void UpdateSkillSlot(int prevIdx, int newIdx, PlayerSkillDataSO skillData)
    {
        if(prevIdx == -1) // Add to empty slot
        {
            skillSlots[newIdx] = skillData;
        }
        else // Swap skill slots
        {
            // Swap data
            PlayerSkillDataSO tempData = skillSlots[newIdx];
            skillSlots[newIdx] = skillData;
            skillSlots[prevIdx] = tempData;
        }
    }

    public PlayerSkillDataSO GetSkillInSlot(int idx)
    {
        return skillSlots[idx];
    }

    public void ObtainPassiveSkill()
    {
        hasPassive = true;
    }

    public void ShowCards()
    {
        deck = GetComponentInChildren<CardDeck>();
        deck.ShowCards();
    }

    #endregion

    #region Map Generation
    public void SpawnRooms()
    {
        mapGenerator.GenerateMap();
    }

    public void StartStage()
    {
        mapGenerator.EnterFirstRoom();
    }

    public void SpawnChest()
    {
        GameObject chest = Manager.Resource.Load<GameObject>("Prefabs/Chest");
        float xOffset = UnityEngine.Random.Range(-5f, 5f);
        float zOffset = UnityEngine.Random.Range(-5f, 5f);
        Vector3 spawnPos = new Vector3(playerAttack.transform.position.x + xOffset, 0, playerAttack.transform.position.z + zOffset);
        Instantiate(chest, spawnPos, Quaternion.identity);
    }

    public void CreatePools()
    {
        Manager.Pool.CreatePool(Manager.Resource.Load<PooledObject>("Effects/WarriorPierce1"), 2, 4);
        Manager.Pool.CreatePool(Manager.Resource.Load<PooledObject>("Effects/WarriorSlash"), 2, 4);
        Manager.Pool.CreatePool(Manager.Resource.Load<PooledObject>("Effects/WarriorSlam"), 2, 4);
        Manager.Pool.CreatePool(Manager.Resource.Load<PooledObject>("Effects/WarriorSkill1"), 2, 4);
        Manager.Pool.CreatePool(Manager.Resource.Load<PooledObject>("Effects/WarriorSkill2"), 2, 4);
        Manager.Pool.CreatePool(Manager.Resource.Load<PooledObject>("Effects/WarriorSkill3"), 3, 6);
        Manager.Pool.CreatePool(Manager.Resource.Load<PooledObject>("Effects/WarriorSkill4"), 5, 10);
        Manager.Pool.CreatePool(Manager.Resource.Load<PooledObject>("Effects/WarriorSkill4Explosion"), 5, 10);
        Manager.Pool.CreatePool(Manager.Resource.Load<PooledObject>("Effects/WarriorSkill5"), 2, 4);
        Manager.Pool.CreatePool(Manager.Resource.Load<PooledObject>("Effects/WarriorSkill6"), 2, 4);
    }
    #endregion
}
