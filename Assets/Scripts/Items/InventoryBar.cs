using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBar : MonoBehaviour
{
    public class DragData
    {
        public InventorySlot slot;
        public Transform transform;
    }

    public InventorySlot[] inventorySlots;
    [HideInInspector] public DragData dragData;
    bool slotsFull;

    private void Start()
    {
        inventorySlots = GetComponentsInChildren<InventorySlot>();
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].SetSlot(i, this);
        }
    }

    public PlayerSkillDataSO GetSkillData(int idx)
    {
        return inventorySlots[idx].skillData;
    }

    public void AddItem(PlayerSkillDataSO skillData)
    {
        if (slotsFull)
        {
            // TODO: Swap skill when slots full prompt
            return;
        }

        for(int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].skillData != null)
                continue;
            inventorySlots[i].skillData = skillData;
            inventorySlots[i].UpdateIcon();
            return;
        }

        slotsFull = true;
        // TODO: Swap skill when slots full prompt
    }
}
