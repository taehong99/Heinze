using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveSlot : BaseUI
{
    PlayerSkillDataSO data;

    public void SetSlot(PlayerSkillDataSO data)
    {
        this.data = data;
        GetUI<Image>("SkillIcon").sprite = data.icon;
    }
}
