using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Potion, Coin }

[CreateAssetMenu(menuName = "Data/Upgrades/ItemData", fileName = "ItemData")]
public class ConsumableItemSO : PlayerUpgradeSO
{
    public ItemType type;
    public int gainAmount;
}
