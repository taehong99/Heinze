using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffSlotUI : BaseUI
{
    public void SetValues(PlayerBuffSO buff)
    {
        GetUI<Image>("Icon").sprite = buff.icon;
        GetUI<TextMeshProUGUI>("BuffName").text = buff._name;
        GetUI<TextMeshProUGUI>("BuffDescription").text = buff.description;
        GetUI<TextMeshProUGUI>("BuffCaption").text = buff.caption;
    }
}
