using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Data/Upgrades/PlayerSkillData", fileName = "PlayerSkillData")]
public class PlayerSkillDataSO : ScriptableObject
{
    [Header("Data")]
    public int skillID;

    [Header("Visuals")]
    public Sprite skillIcon;
    public string skillName;
    public string skillDescription;
    public string skillCaption;
}
