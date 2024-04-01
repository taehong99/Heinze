using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    // Data
    [SerializeField] PlayerDataSO data;

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

    public void Heal(int amount)
    {
        curHP += amount;
        curHP = Mathf.Clamp(curHP, minHP, maxHP);
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
