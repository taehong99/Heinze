using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDashPresenter : BaseUI
{
    [SerializeField] PlayerController player;
    Slider dashSlider;
    Image[] dashSlots = new Image[3];
    [SerializeField] Sprite dashEmptySprite;
    [SerializeField] Sprite dashReadySprite;

    void Start()
    {
        //player = Manager.Player.GetComponent<PlayerController>();
        player.DashCountChanged += OnDashChanged;

        dashSlider = GetUI<Slider>("DashBar");
        dashSlider.maxValue = player.DashCooldown;

        dashSlots[0] = GetUI<Image>("Dash1");
        dashSlots[1] = GetUI<Image>("Dash2");
        dashSlots[2] = GetUI<Image>("Dash3");

    }

    private void OnDestroy()
    {
        player.DashCountChanged -= OnDashChanged;
    }

    void Update()
    {
        dashSlider.value = player.DashCooldownProgress;
    }

    private void UpdateView(int charges)
    {
        for(int i = 0; i < 3; i++)
        {
            if(i < charges)
            {
                dashSlots[i].sprite = dashReadySprite;
            }
            else
            {
                dashSlots[i].sprite = dashEmptySprite;
            }
        }
    }

    public void OnDashChanged(int charges)
    {
        UpdateView(charges);
    }
}
