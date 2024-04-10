using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum IncreaseRate { Flat, Percent }
[CreateAssetMenu(menuName = "Data/Upgrades/PlayerBuffData", fileName = "PlayerBuffData")]
public class PlayerBuffSO : PlayerUpgradeSO
{
    [Header("Data")]
    public Stat affectedStat;
    public IncreaseRate increaseRate;
    public float value;
}
