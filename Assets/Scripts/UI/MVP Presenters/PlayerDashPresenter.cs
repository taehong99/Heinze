using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDashPresenter : BaseUI
{
    PlayerController player;
    Slider dashSlider;
    Image[] dashSlots = new Image[3];

    void Start()
    {
        StartCoroutine(FindPlayerRoutine());
        dashSlider = GetUI<Slider>("DashBar");
        
        dashSlots[0] = GetUI<Image>("DashOrb1");
        dashSlots[1] = GetUI<Image>("DashOrb2");
        dashSlots[2] = GetUI<Image>("DashOrb3");
    }

    IEnumerator FindPlayerRoutine()
    {
        while (Manager.Player.controller == null)
        {
            yield return null;
        }
        player = Manager.Player.controller;
        player.DashCountChanged += OnDashChanged;
        dashSlider.maxValue = player.DashCooldown;
    }

    private void OnDestroy()
    {
        player.DashCountChanged -= OnDashChanged;
    }

    void Update()
    {
        if (player == null)
            return;
        dashSlider.value = player.DashCooldownProgress;
    }

    private void UpdateView(int charges)
    {
        for(int i = 0; i < 3; i++)
        {
            if(i < charges)
            {
                //dashSlots[i].sprite = dashReadySprite;
                dashSlots[i].gameObject.SetActive(true);
            }
            else
            {
                //dashSlots[i].sprite = dashEmptySprite;
                dashSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnDashChanged(int charges)
    {
        UpdateView(charges);
    }
}
