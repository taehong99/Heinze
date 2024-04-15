using DungeonArchitect.Samples.ShooterGame;
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
    public PlayerSkillDataSO passiveSlot;

    // Player Buffs
    private List<PlayerBuffSO> buffs = new List<PlayerBuffSO>();
    public List<PlayerBuffSO> Buffs => buffs;

    // Player Items
    int potionCount;
    int goldCount;
    public int PotionCount {  get { return potionCount; } set { potionCount = value; potionCountChanged?.Invoke(value); } }
    public int GoldCount {  get { return goldCount; } set { goldCount = value; goldCountChanged?.Invoke(value); } }

    // MapGen
    MapGenerator mapGenerator;
    public MapGenerator MapGenerator => mapGenerator;
    CardDeck deck;

    // Events
    public event Action<PlayerSkillDataSO> SkillPicked;
    public event Action<PlayerBuffSO> BuffPicked;
    public event Action<ConsumableItemSO> ItemPicked;
    public event Action<int> potionCountChanged;
    public event Action<int> goldCountChanged;

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
        if (data.id == 11)
        {
            Manager.Player.GainLifeSteal(0.03f);
            passiveSlot = data;
        }
            
        SkillPicked?.Invoke(data);
    }
    public void AnnounceBuffPicked(PlayerBuffSO data)
    {
        buffs.Add(data);
        BuffPicked?.Invoke(data);
    }
    public void AnnounceItemPicked(ConsumableItemSO data)
    {
        if (data.id == 4)
            ObtainPotions(3);

        if (data.type == ItemType.Coin)
        {
            ObtainGold(data.gainAmount);
        }
        else if(data.type == ItemType.Potion)
        {
            Manager.Player.Heal(data.gainAmount);
        }
        ItemPicked?.Invoke(data);
    }

    public void UpdateSkillSlot(int prevIdx, int newIdx, PlayerSkillDataSO droppedSkill)
    {
        if(prevIdx == -1) // Add to empty slot
        {
            skillSlots[newIdx] = droppedSkill;
        }
        else // Swap skill slots
        {
            // Swap data
            PlayerSkillDataSO temp = skillSlots[newIdx];
            skillSlots[newIdx] = droppedSkill;
            skillSlots[prevIdx] = temp;
        }
    }

    public PlayerSkillDataSO GetSkillInSlot(int idx)
    {
        return skillSlots[idx];
    }

    public void ShowCards()
    {
        deck = GetComponentInChildren<CardDeck>();
        deck.ShowCards();
    }

    #endregion

    #region Player Items
    public void DrinkPotion()
    {
        if (potionCount == 0)
            return;

        PotionCount--;
        Manager.Player.Heal(30);
    }

    public void ObtainPotions(int count)
    {
        Debug.Log(count);
        PotionCount += count;
    }

    public void ObtainGold(int count)
    {
        GoldCount += count;
    }

    #endregion

    #region Map Generation
    public void SpawnRooms(Stage stage)
    {
        mapGenerator.GenerateMap(stage);
    }

    public void StartStage()
    {
        mapGenerator.EnterFirstRoom();
    }

    public void SpawnChest()
    {
        GameObject chest = Manager.Resource.Load<GameObject>("Prefabs/Props/Chest");
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
        Manager.Pool.CreatePool(Manager.Resource.Load<PooledObject>("Effects/WarriorSkill4"), 10, 10);
        Manager.Pool.CreatePool(Manager.Resource.Load<PooledObject>("Effects/WarriorSkill4Explosion"), 5, 10);
        Manager.Pool.CreatePool(Manager.Resource.Load<PooledObject>("Effects/WarriorSkill5"), 2, 4);
        Manager.Pool.CreatePool(Manager.Resource.Load<PooledObject>("Effects/WarriorSkill6"), 2, 4);
    }
    #endregion

    #region Game Over

    public void RestartGame()
    {
        skillSlots = new PlayerSkillDataSO[4];
        passiveSlot = null;
        buffs.Clear();
        PotionCount = 0;
        GoldCount = 0;
    }

    #endregion
}
