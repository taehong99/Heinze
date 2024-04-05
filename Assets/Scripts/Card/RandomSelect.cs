﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelect : MonoBehaviour
{
    public List<Card> deck = new List<Card>();  // 카드 덱
    public int total = 0;  // 카드들의 가중치 총 합
    bool canExecute = true;
    float delayTime = 1f; // 이 값은 필요에 따라 조절 가능합니다.
    bool isPressed;
    public Transform panelTr;
    void Start()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            // 스크립트가 활성화 되면 카드 덱의 모든 카드의 총 가중치를 구해줍니다.
            total += deck[i].weight;
        }
        isPressed = false;
    }

    void Update()
    {
        if (isPressed)
            return;
        if (canExecute && Input.GetKeyDown(KeyCode.J))
        {
            ResultSelect();
            canExecute = false;
            StartCoroutine(ResetCanExecute());
            isPressed = true;
        }
    }
    List<CardUI> cards = new List<CardUI>();
    IEnumerator ResetCanExecute()
    {
        //yield return new WaitForSeconds(delayTime);
        for (int i = 0; i < 3; i++)
        {
            result.Add(RandomCard());
            // 비어 있는 카드를 생성하고
            CardUI cardUI = Instantiate(cardprefab, parent).GetComponent<CardUI>();
            cardUI.CardUISet(result[i]);
            cardUI.transform.parent = panelTr;
            cards.Add(cardUI);
            yield return new WaitForSeconds(0.07f);
        }

        for (int i = 0; i < cards.Count; i++)
        {
            // 가중치 랜덤을 돌리면서 결과 리스트에 넣어줍니다.
            result.Add(RandomCard());
            // 비어 있는 카드를 생성하고
            //CardUI cardUI = Instantiate(cardprefab, parent).GetComponent<CardUI>();
            // 생성 된 카드에 결과 리스트의 정보를 넣어줍니다.
            //cardUI.CardUISet(result[i]);
            cards[i].Flip();
            yield return new WaitForSeconds(0.3f);
        }
        canExecute = true;
    }

    public List<Card> result = new List<Card>();  // 랜덤하게 선택된 카드를 담을 리스트
    public Transform parent;
    public GameObject cardprefab;
    public void ResultSelect()
    {
        result.Clear(); // 결과 리스트 초기화
        canExecute = true;
        //for (int i = 0; i < 3; i++)
        //{

        //}
    }

    // 가중치 랜덤의 설명은 영상을 참고.
    public Card RandomCard()
    {
        int weight = 0;
        int selectNum = 0;

        selectNum = Random.Range(0, total) + 1;

        for (int i = 0; i < deck.Count; i++)
        {
            weight += deck[i].weight;
            if (selectNum <= weight)
            {
                Card temp = new Card(deck[i]);
                return temp;
            }
        }
        return null;
    }
}