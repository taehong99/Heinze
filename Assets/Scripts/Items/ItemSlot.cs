using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : BaseUI
{
    Image icon;
    TextMeshProUGUI countText;
    Sprite emptyIcon;
    [SerializeField] Sprite potionIcon;

    private void Start()
    {
        icon = GetUI<Image>("ItemIcon");
        emptyIcon = icon.sprite;
        countText = GetUI<TextMeshProUGUI>("ItemCountText");
        UpdateCount(Manager.Game.PotionCount);
        Manager.Game.potionCountChanged += UpdateCount;
    }

    private void OnDestroy()
    {
        Manager.Game.potionCountChanged -= UpdateCount;
    }

    public void UpdateCount(int count)
    {
        if(count == 0)
        {
            icon.sprite = emptyIcon;
            countText.gameObject.SetActive(false);
            return;
        }

        icon.sprite = potionIcon;
        countText.gameObject.SetActive(true);
        countText.text = count.ToString();
    }
}
