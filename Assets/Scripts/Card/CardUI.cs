﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardUI : BaseUI, IPointerDownHandler
{
    Animator animator;
    PlayerUpgradeSO cardData;
    GameObject controller;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    // 카드의 정보를 초기화
    public void CardUISet(PlayerUpgradeSO card)
    {
        if (card != null)
        {
            this.cardData = card;
            GetUI<Image>("FrontBG").color = card.cardColor;
            GetUI<Image>("BackBG").color = card.cardColor;
            GetUI<Image>("CardIconImage").sprite = card.icon;
            GetUI<TextMeshProUGUI>("CardName").text = card._name;
            GetUI<TextMeshProUGUI>("CardDescription").text = card.description;
            GetUI<TextMeshProUGUI>("CardCaption").text = card.caption;

            // 버튼 클릭 이벤트 설정
            GetUI<Button>("Front").onClick.AddListener(AddToInventory);
        }
        else
        {
            Debug.LogError("Card is null!");
        }
    }
    
    public void Flip() => animator.SetTrigger("Flip");// 카드가 클릭되면 뒤집는 애니메이션 재생

    public void SetDataController(GameObject _cont) //데이터 연결할 객체를 전달
    {
        controller = _cont;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //연결된 객체에 데이터를 삽입
    }

    public void PlayCardFlipSound()
    {
        Manager.Sound.PlaySFX(Manager.Sound.AudioClips.cardFlipSFX);
    }

    private void AddToInventory()
    {
        Manager.Player.UnFreeze();
        Manager.Sound.PlaySFX(Manager.Sound.AudioClips.cardSelectSFX);
        if (cardData is PlayerSkillDataSO)
        {
            Manager.Game.AnnounceSkillPicked(cardData as PlayerSkillDataSO);
        }
        else if(cardData is PlayerBuffSO)
        {
            Manager.Game.AnnounceBuffPicked(cardData as PlayerBuffSO);
        }
        else
        {
            Manager.Game.AnnounceItemPicked(cardData as ConsumableItemSO);
        }
    }
}
