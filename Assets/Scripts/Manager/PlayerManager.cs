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
    PlayerController controller;
    PlayerJob job = PlayerJob.Warrior; // TODO: Change this
    public PlayerJob Job;
    public string jobText;

    // HP
    const int minHP = 0;
    public int maxHP;
    private int curHP;
    public int CurHP { get => curHP; set { curHP = value; PlayerHPChanged?.Invoke(); } }

    // Stats
    private int attack;
    [Range(0f, 1f)]
    private float critRate;
    private float defense;
    private float moveSpeed;
    private float lifeSteal;
    public int Attack => attack;
    public float CritRate => critRate;
    public float Defense => defense;
    public float MoveSpeed => moveSpeed;

    // Events
    public event Action PlayerHPChanged;
    public event Action PlayerDied;
    public event Action<int> PlayerHealed;

    private void Start()
    {
        maxHP = data.baseHP;
        curHP = maxHP;
        attack = data.baseAttack;
        critRate = data.baseCritRate;
        defense = data.baseDefense;
        moveSpeed = data.baseMoveSpeed;
        Manager.Game.BuffPicked += ObtainBuff;
    }

    public void AssignPlayer(PlayerController player)
    {
        controller = player;
    }

    public void Freeze()
    {
        controller.Freeze();
    }

    public void UnFreeze()
    {
        controller.UnFreeze();
    }

    public void ObtainBuff(PlayerBuffSO buff)
    {
        UpdateStat(buff.affectedStat, buff.increaseRate, buff.value);
    }

    public void ChooseJob(PlayerJob job)
    {
        this.job = job;
    }

    public void Heal(int amount)
    {
        int missingHP = maxHP - curHP;
        int healedAmt = Math.Min(missingHP, amount);
        CurHP += healedAmt;
        PlayerHealed?.Invoke(healedAmt);
    }

    public int GetAttack(float multiplier) // multiplier = x%
    {
        int attackMax = Mathf.CeilToInt(attack * 1.3f);
        int rawAttack = UnityEngine.Random.Range(attack, attackMax + 1);
        float rand = UnityEngine.Random.Range(0f, 1f);
        if(rand <= critRate)
        {
            rawAttack *= 2;
        }
        int finalDamage = Mathf.CeilToInt(rawAttack * multiplier / 100);
        if(lifeSteal > 0f)
        {
            Heal(Mathf.RoundToInt(finalDamage * lifeSteal));
        }
        return finalDamage;
    }

    public void GainLifeSteal(float percentage)
    {
        lifeSteal += percentage;
    }

    public void UpdateStat(Stat stat, IncreaseRate rate, float delta)
    {
        switch (stat)
        {
            case Stat.Attack:
                if(rate == IncreaseRate.Flat)
                {
                    attack += (int)delta;
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
                if (rate == IncreaseRate.Flat)
                {
                    maxHP += (int)delta;
                    CurHP += (int)delta;
                }
                else
                {
                    int sum = Mathf.CeilToInt(maxHP * delta);
                    maxHP += sum;
                    CurHP += sum;
                }
                break;
        }
    }

    public int CalculateTakenDamage(int damage)
    {
        int reducedDamage = damage - (int)defense;
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

    public void Die()
    {

    }

    public void Reset() // Call on game restart
    {
        maxHP = data.baseHP;
        curHP = maxHP;
        attack = data.baseAttack;
        defense = data.baseDefense;
    }
}
