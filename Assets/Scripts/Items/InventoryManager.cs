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
    // 카드를 추가하는 로직을 작성합니다.
    // 예를 들어, 인벤토리 슬롯에 빈 공간을 찾아서 해당 카드를 추가하고 성공 여부를 반환합니다.

    //    for (int i = 0; i < inventorySlots.Length; i++)
    //    {
    //        // 인벤토리 슬롯을 확인하여 빈 공간이 있는지 검사합니다.
    //        if (inventorySlots[i].IsEmpty())
    //        {
    //            // 빈 슬롯을 찾았을 경우 해당 슬롯에 카드를 추가하고 성공을 반환합니다.
    //            inventorySlots[i].AddItem(card);
    //            return true;
    //        }
    //    }
    //    return false;
    //}



    void SpawnItem(Item item, InventorySlot slot) //아이템 생성
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        DraggableItem draggableItem = newItemGo.GetComponent<DraggableItem>();
    }


}


