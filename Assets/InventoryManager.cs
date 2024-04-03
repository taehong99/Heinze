using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.LongLength; i++)
        {
            InventorySlot slot = inventorySlots[i];
            DraggableItem itemInSlot = slot.GetComponentInChildren<DraggableItem>();
            if (itemInSlot == null)
            {
                SpawnItem(item, slot);
                return true;
            }
        }
        return false;
    }
    void SpawnItem(Item item, InventorySlot slot) //아이템 생성
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        DraggableItem draggableItem = newItemGo.GetComponent<DraggableItem>();
    }
}
