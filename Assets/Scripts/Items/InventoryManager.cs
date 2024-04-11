using DungeonArchitect.Editors.LaunchPad;
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
    //public bool AddCard(Card card)
    //{
    // ī�带 �߰��ϴ� ������ �ۼ��մϴ�.
    // ���� ���, �κ��丮 ���Կ� �� ������ ã�Ƽ� �ش� ī�带 �߰��ϰ� ���� ���θ� ��ȯ�մϴ�.

    //    for (int i = 0; i < inventorySlots.Length; i++)
    //    {
    //        // �κ��丮 ������ Ȯ���Ͽ� �� ������ �ִ��� �˻��մϴ�.
    //        if (inventorySlots[i].IsEmpty())
    //        {
    //            // �� ������ ã���� ��� �ش� ���Կ� ī�带 �߰��ϰ� ������ ��ȯ�մϴ�.
    //            inventorySlots[i].AddItem(card);
    //            return true;
    //        }
    //    }
    //    return false;
    //}



    void SpawnItem(Item item, InventorySlot slot) //������ ����
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        DraggableItem draggableItem = newItemGo.GetComponent<DraggableItem>();
    }


}


