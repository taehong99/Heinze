using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Monster", fileName = "MonsterData")]
public class MonsterDataSO : ScriptableObject
{
    [Header("Stats")]
    public int hp;
    public int attack;
    public int defense;

    //[Header("Details")]
    //public string _name;

    //public float[] dropChances;
    //public Item[] drops;
}
