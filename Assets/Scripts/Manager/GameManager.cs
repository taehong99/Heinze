using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private InventoryManager inventoryManager;
    public List<Item> itemToPickup;
    public void Test()
    {
        Debug.Log(GetInstanceID());
    }


    void Start()
    {
        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();
        for (int i=0; i < itemToPickup.Count; i++)
        {
            bool addRestult = inventoryManager.AddItem(itemToPickup[i]);
        }
    }
}
