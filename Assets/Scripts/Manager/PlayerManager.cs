using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    const int minHP = 0;

    public event Action PlayerHPChanged;
    public event Action PlayerDied;
    const int startingHP = 100;
    public int maxHP = 100;

    private int curHP;
    public int CurHP { get => curHP; set { curHP = value; PlayerHPChanged?.Invoke(); } }

    private void Start()
    {
        curHP = startingHP;
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
}
