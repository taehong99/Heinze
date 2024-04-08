using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : BaseUI, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Slot Data")]
    public PlayerSkillDataSO skillData;
    bool filled;
    InventoryBar parent;
    Image icon;
    int idx;

    void Start()
    {
        icon = GetUI<Image>("SkillIcon");
    }

    public void SetSlot(int idx, InventoryBar parent)
    {
        this.idx = idx;
        this.parent = parent;
        filled = false;
    }

    public void UpdateIcon()
    {
        icon.sprite = skillData.skillIcon;
    }

    public void UpdateIcon(Sprite newIcon)
    {
        icon.sprite = newIcon;
        //filled = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin drag");
        if (parent.dragData == null) // Instantiate if null
            parent.dragData = new InventoryBar.DragData();

        parent.dragData.slot = this;
        parent.dragData.transform = transform;
        icon.transform.SetParent(GetComponentInParent<Canvas>().transform);
        icon.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        icon.transform.position = eventData.position;
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        icon.raycastTarget = true;

        // Item dropped in invalid area
        if (!RectTransformUtility.RectangleContainsScreenPoint((RectTransform)parent.transform, eventData.position))
        {
            icon.transform.SetParent(parent.dragData.transform);
            icon.transform.localPosition = Vector2.zero;
            return;
        }
        else
        {
            InventorySlot[] slots = parent.inventorySlots;
            for (int i = 0; i < slots.Length; i++)
            {
                RectTransform rect = (RectTransform)slots[i].transform;
                if (RectTransformUtility.RectangleContainsScreenPoint(rect, eventData.position))
                {
                    if (slots[i] == parent.dragData.slot) // Slot is same as start
                    {
                        icon.transform.SetParent(parent.dragData.transform);
                        icon.transform.localPosition = Vector2.zero;
                        return;
                    }
                    else // Swap image with slot
                    {
                        Sprite tempSprite = slots[i].icon.sprite;
                        slots[i].icon.sprite = icon.sprite;
                        icon.sprite = tempSprite;
                        if (slots[i].filled) // Filled slot
                        {
                            PlayerSkillDataSO tempData = slots[i].skillData;
                            slots[i].skillData = skillData;
                            skillData = tempData;
                        }
                        else
                        {
                            slots[i].skillData = skillData;
                            skillData = null;
                            filled = false;
                        }
                    }
                }
            }
        }
        icon.transform.SetParent(parent.dragData.transform);
        icon.transform.localPosition = Vector2.zero;
    }

    //public void OnDrop(PointerEventData eventData)
    //{
    //    if (filled)
    //    {

    //    }
    //    else
    //    {

    //    }

    //    if (transform.childCount == 0)
    //    {
    //        GameObject dropped = eventData.pointerDrag;
    //        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
    //        draggableItem.parentAfterDrag = transform;
    //    }
    //}

}    
        
    

    