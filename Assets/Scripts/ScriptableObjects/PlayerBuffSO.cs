using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Data/Upgrades/PlayerBuffData", fileName = "PlayerBuffData")]
public class PlayerBuffSO : ScriptableObject
{
    [Header("Data")]
    public Stat affectedStat;
    public float value;

    [Header("Visuals")]
    public Image buffIcon;
    public string buffName;
    public string buffDescription;
    public string buffCaption;
}
