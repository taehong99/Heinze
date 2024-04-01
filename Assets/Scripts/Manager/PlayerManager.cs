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
    private float defense;
    public float Attack => attack;
    public float Defense => defense;

    // Events
    public event Action PlayerHPChanged;
    public event Action PlayerDied;

    private void Start()
    {
        maxHP = data.baseHP;
        curHP = maxHP;
        attack = data.baseAttack;
        defense = data.baseDefense;
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
