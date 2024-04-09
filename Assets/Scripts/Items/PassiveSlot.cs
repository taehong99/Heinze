using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveSlot : BaseUI
{
    [SerializeField] Sprite icon;

    public void SetIcon()
    {
        GetUI<Image>("SkillIcon").sprite = icon;
    }
}
