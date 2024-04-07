using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Scriptable object/Item")]

public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public string itemDescription;
    public int cost;
    public Sprite icon;
    public bool stackable = true;
    public bool isTool = false;
    public bool isCraft = false;
}
