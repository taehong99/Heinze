using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Upgrades/WarriorDeck", fileName = "WarriorDeck")]
public class WarriorCardDeckSO : ScriptableObject
{
    public List<PlayerSkillDataSO> skills;
    public List<PlayerBuffSO> buffs;
    public List<ConsumableItemSO> items;
}
