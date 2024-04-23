using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatsUI : PopUpUI
{
    PlayerManager player;
    [SerializeField] GameObject buffSlotPrefab;

    void Start()
    {
        player = Manager.Player;
        UpdateValues();
        UpdateBuffList();
    }

    private void UpdateBuffList()
    {
        foreach(var buff in Manager.Game.Buffs)
        {
            BuffSlotUI buffSlot = Instantiate(buffSlotPrefab).GetComponent<BuffSlotUI>();
            buffSlot.transform.parent = GetUI<Transform>("BuffList");
            buffSlot.SetValues(buff);
        }
    }

    private void UpdateValues()
    {
        GetUI<TextMeshProUGUI>("JobText").text = player.Job.ToString();
        GetUI<TextMeshProUGUI>("HPValueText").text = player.CurHP.ToString();
        GetUI<TextMeshProUGUI>("ATKValueText").text = player.Attack.Value.ToString();
        GetUI<TextMeshProUGUI>("DEFValueText").text = player.Defense.Value.ToString();
        GetUI<TextMeshProUGUI>("CRValueText").text = $"{player.CritRate.Value * 100} %";
        GetUI<TextMeshProUGUI>("MSValueText").text = player.MoveSpeed.Value.ToString();
    }
}
