using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHPPresenter : BaseUI
{
    PlayerManager player;
    Slider hpSlider;
    TMP_Text hpText;

    private void Start()
    {
        player = Manager.Player;
        player.PlayerHPChanged += OnHPChanged;

        hpSlider = GetUI<Slider>("HPBar");
        hpText = GetUI<TMP_Text>("HPText");
        UpdateView();
    }

    public void UpdateView()
    {
        if (player == null || hpText == null)
            return;

        hpSlider.value = player.CurHP;
        hpText.text = $"{player.CurHP} / {player.maxHP}";
    }

    public void OnHPChanged()
    {
        UpdateView();
    }
}
