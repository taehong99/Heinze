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
    PlayerJob job = PlayerJob.Warrior; // TODO: Change this
    public PlayerJob Job;

    // HP
    const int minHP = 0;
    public int maxHP;
    private int curHP;
    public int CurHP { get => curHP; set { curHP = value; PlayerHPChanged?.Invoke(); } }

    // Stats
    private float attack;
    [Range(0f, 1f)]
    private float critRate;
    private float defense;
    private float moveSpeed;
    private float lifeSteal;
    public float Attack => attack;
    public float CritRate => critRate;
    public float Defense => defense;
    public float MoveSpeed => moveSpeed;

    // Events
    public event Action PlayerHPChanged;
    public event Action PlayerDied;

    private void Start()
    {
        maxHP = data.baseHP;
        curHP = maxHP;
        attack = data.baseAttack;
        critRate = data.baseCritRate;
        defense = data.baseDefense;
        moveSpeed = data.baseMoveSpeed;
    }

    public void ChooseJob(PlayerJob job)
    {
        this.job = job;
    }

    public void Heal(int amount)
    {
        curHP += amount;
        curHP = Mathf.Clamp(curHP, minHP, maxHP);
    }

    public void UpdateStat(Stat stat, IncreaseRate rate, float delta)
    {
        switch (stat)
        {
            case Stat.Attack:
                if(rate == IncreaseRate.Flat)
                {
                    attack += delta;
                }
                else
                {
                    attack = Mathf.CeilToInt(attack * (1 + delta));
                }
                break;
            case Stat.Crit:
                if (rate == IncreaseRate.Flat)
                {
                    critRate += delta;
                }
                else
                {
                    critRate = Mathf.CeilToInt(critRate * (1 + delta));
                }
                break;
            case Stat.Defense:
                if (rate == IncreaseRate.Flat)
                {
                    defense += delta;
                }
                else
                {
                    defense = Mathf.CeilToInt(defense * (1 + delta));
                }
                break;
            case Stat.Speed:
                if (rate == IncreaseRate.Flat)
                {
                    moveSpeed += delta;
                }
                else
                {
                    moveSpeed = Mathf.CeilToInt(moveSpeed * (1 + delta));
                }
                break;
            case Stat.Health:
                break;
        }
    }

    public void TakeDamage(int amount)
    {
        curHP -= amount;
        curHP = Mathf.Clamp(curHP, minHP, maxHP);
        if(curHP == 0)
        {
            // Player died event
            PlayerDied?.Invoke();
        }
    }

    public void Reset() // Call on game restart
    {
        maxHP = data.baseHP;
        curHP = maxHP;
        attack = data.baseAttack;
        defense = data.baseDefense;
    }
}
