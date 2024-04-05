using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerDownHandler
{
    public Image chr;
    public Text cardName;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    // 카드의 정보를 초기화
    public void CardUISet(Card card)
    {
        //chr.sprite = card.cardImage;
        //cardName.text = card.cardName;
    }
    // 카드가 클릭되면 뒤집는 애니메이션 재생

    public void Flip()
        => animator.SetTrigger("Flip");

    GameObject controller;
    public void SetDataContorller(GameObject _cont)
    {
        //데이터 연결할 객체를 전달
        controller = _cont;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //연결된 객체에 데이터를 삽입
        //
    }
}
