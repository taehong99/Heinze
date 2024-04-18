using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlots : MonoBehaviour
{
    ItemSlot[] itemSlots;

    private void Start()
    {
        itemSlots = GetComponentsInChildren<ItemSlot>();
    }

    public void UpdateSlot(int count)
    {
        
    }
}
