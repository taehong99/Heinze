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
    public Text cardNameText;
    public Button addToInventoryButton; // 인벤토리에 추가하는 버튼

    private PlayerSkillDataSO card;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    // 카드의 정보를 초기화
    public void CardUISet(PlayerSkillDataSO card)
    {
        if (card != null)
        {
            this.card = card;
            cardNameText.text = card.skillName;

            // 버튼 클릭 이벤트 설정
            addToInventoryButton.onClick.AddListener(AddToInventory);
        }
        else
        {
            Debug.LogError("Card is null!");
        }

       
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
    }

    private void AddToInventory()
    {
        Debug.Log("인벤토리에 추가함.");
    }
}
