using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player", fileName = "PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    [Header("Player Base Stats")]
    public int baseHP;
    public int baseAttack;
    public int baseDefense;
}
