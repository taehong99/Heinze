using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameObject warriorPrefab;
    private PlayerSkillDataSO[] skillSlots = new PlayerSkillDataSO[4];
    private List<PlayerBuffSO> buffs = new List<PlayerBuffSO>();
    private bool hasPassive;

    MapGenerator mapGenerator;
    CardDeck deck;
    public event Action<PlayerSkillDataSO> SkillPicked;
    public event Action<PlayerBuffSO> BuffPicked;
    public event Action<ConsumableItemSO> ItemPicked;

    private void Start()
    {
        mapGenerator = GetComponentInChildren<MapGenerator>();
    }

    #region Player Skills + Buffs
    public void AnnounceSkillPicked(PlayerSkillDataSO data)
    {
        SkillPicked.Invoke(data);
    }
    public void AnnounceBuffPicked(PlayerBuffSO data)
    {
        BuffPicked.Invoke(data);
    }

    public void UpdateSkillSlot(int idx, PlayerSkillDataSO skillData)
    {
        skillSlots[idx] = skillData;
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
