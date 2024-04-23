using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerJob { Warrior, Archer, Magician }
public enum Stat { Attack, Crit, Defense, Speed, Health}
public class PlayerManager : Singleton<PlayerManager>
{
    // Data
    [SerializeField] PlayerDataSO data;
    public PlayerController controller;
    PlayerJob job = PlayerJob.Warrior; // TODO: Change this
    public PlayerJob Job;
    public string jobText;

    // HP
    const int minHP = 0;
    public int maxHP;
    private int curHP;
    public int CurHP { get => curHP; set { curHP = value; PlayerHPChanged?.Invoke(); } }

    // Stats
    public PlayerStat Attack;
    public PlayerStat Defense;
    public PlayerStat CritRate;
    public PlayerStat MoveSpeed;
    public PlayerStat LifeSteal;
    
    // private int attack;
    // [Range(0f, 1f)]
    // private float critRate;
    // private float defense;
    // private float moveSpeed;
    // private float lifeSteal;
    // public int Attack => attack;
    // public float CritRate => critRate;
    // public float Defense => defense;
    // public float MoveSpeed => moveSpeed;

    // Events
    public event Action PlayerHPChanged;
    public event Action PlayerDied;
    public event Action<int> PlayerHealed;

    private void Start()
    {
        SetBaseStats();
        Manager.Game.BuffPicked += ObtainBuff;
    }

    #region Player Stats
    public void SetBaseStats()
    {
        maxHP = data.baseHP;
        CurHP = maxHP;
        Attack = new PlayerStat(data.baseAttack);
        Defense = new PlayerStat(data.baseDefense);
        CritRate = new PlayerStat(data.baseCritRate);
        MoveSpeed = new PlayerStat(data.baseMoveSpeed);
        LifeSteal = new PlayerStat(0);
    }
    
    public void ObtainBuff(PlayerBuffSO buff)
    {
        UpdateStat(buff.affectedStat, new StatModifier(buff.value, buff.increaseRate));
    }
    
    public int CalculateDamage(float multiplier) // multiplier = x%
    {
        // Determine Base Attack Value
        int attackMax = Mathf.CeilToInt(Attack.Value * 1.3f);
        int rawAttack = UnityEngine.Random.Range((int)Attack.Value, attackMax + 1);
        
        // Check for Critical Strike
        float rand = UnityEngine.Random.Range(0f, 1f);
        if(rand <= CritRate.Value)
        {
            rawAttack *= 2;
        }
        
        // Calculate Final Damage
        int finalDamage = Mathf.CeilToInt(rawAttack * multiplier / 100);
        
        // Apply Lifesteal
        if(LifeSteal.Value > 0f)
        {
            Heal(Mathf.RoundToInt(finalDamage * LifeSteal.Value));
        }
        
        return finalDamage;
    }

    public void GainLifeSteal(float percentage)
    {
        LifeSteal.AddModifier(new StatModifier(0.03f, IncreaseRate.Percent));
    }

    public void UpdateStat(Stat stat, StatModifier mod)
    {
        switch (stat)
        {
            case Stat.Attack:
                Attack.AddModifier(mod);
                break;
            case Stat.Defense:
                Defense.AddModifier(mod);
                break;
            case Stat.Crit:
                CritRate.AddModifier(mod);
                break;
            case Stat.Speed:
                MoveSpeed.AddModifier(mod);
                break;
            case Stat.Health:
                if (mod.Type == IncreaseRate.Flat)
                {
                    maxHP += (int)mod.Value;
                    CurHP += (int)mod.Value;
                }
                else
                {
                    int sum = Mathf.CeilToInt(maxHP * mod.Value);
                    maxHP += sum;
                    CurHP += sum;
                }
                break;
        }
    }

    public int CalculateTakenDamage(int damage)
    {
        int reducedDamage = damage - (int)Defense.Value;
        return Mathf.Clamp(reducedDamage, 0, damage);
    }

    public void TakeDamage(int amount)
    {
        CurHP -= amount;
        CurHP = Mathf.Clamp(curHP, minHP, maxHP);
        if(CurHP <= 0)
        {
            // Player died event
            PlayerDied?.Invoke();
        }
    }
    
    public void Heal(int amount)
    {
        int missingHP = maxHP - curHP;
        int healedAmt = Math.Min(missingHP, amount);
        CurHP += healedAmt;
        PlayerHealed?.Invoke(healedAmt);
    }
    #endregion

    public void AssignPlayer(PlayerController player)
    {
        controller = player;
    }

    public void ChooseJob(PlayerJob job)
    {
        this.job = job;
    }
    
    public void Freeze()
    {
        controller.Freeze();
    }

    public void UnFreeze()
    {
        controller.UnFreeze();
    }
}
