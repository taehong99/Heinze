using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlots : MonoBehaviour
{
    public class DragData
    {
        public SkillSlot slot;
        public Transform transform;
    }

    public SkillSlot[] inventorySlots;
    [SerializeField] PassiveSlot passiveSlot;

    [HideInInspector] public DragData dragData;
    bool slotsFull;

    private void Start()
    {
        inventorySlots = GetComponentsInChildren<SkillSlot>();
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].InstantiateSlot(i, this);
        }
        GetSkillSlotsOnStart();
        Manager.Game.SkillPicked += AddItem;
    }

    private void OnDestroy()
    {
        Manager.Game.SkillPicked -= AddItem;
    }

    private void GetSkillSlotsOnStart()
    {
        for(int i = 0; i < 4; i++)
        {
            if (Manager.Game.GetSkillInSlot(i) == null)
                continue;
            inventorySlots[i].skillData = Manager.Game.GetSkillInSlot(i);
            inventorySlots[i].UpdateIcon();
        }
        if (Manager.Game.passiveSlot != null)
        {
            passiveSlot.SetSlot(Manager.Game.passiveSlot);
        }
    }

    public PlayerSkillDataSO GetSkillData(int idx)
    {
        return inventorySlots[idx].skillData;
    }

    public void AddItem(PlayerSkillDataSO skillData)
    {
        // Add Passive Skill
        if(skillData.id >= 10)
        {
            passiveSlot.SetSlot(skillData);
            return;
        }

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
            Manager.Game.UpdateSkillSlot(-1, i, skillData);
            return;
        }

        slotsFull = true;
        // TODO: Swap skill when slots full prompt
    }
}
