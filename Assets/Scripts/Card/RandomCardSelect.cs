using SharpNav.Crowds;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCardSelect : MonoBehaviour
{
    public List<PlayerSkillDataSO> skillDeck = new List<PlayerSkillDataSO>();
    public List<PlayerBuffSO> buffDeck = new List<PlayerBuffSO>();
    public GameObject cardPrefab;
    public Transform parent;
    public Transform panel;

    List<CardUI> cards = new List<CardUI>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("Pressed 0");
            StartCoroutine(SpawnCards());
        }
    }

    private IEnumerator SpawnCards()
    {
        //Choose cards
        //yield return new WaitForSeconds(delayTime);
        for (int i = 0; i < 3; i++)
        {
            PlayerUpgradeSO card = GetRandomCard();
            CardUI cardUI = Instantiate(cardPrefab, parent).GetComponent<CardUI>();
            cardUI.CardUISet(card);
            cardUI.transform.parent = panel;
            cards.Add(cardUI);
            yield return new WaitForSeconds(0.07f);
        }

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].Flip();
            yield return new WaitForSeconds(0.3f);
        }
    }

    public PlayerUpgradeSO GetRandomCard()
    {
        float rand = Random.Range(0f, 1f);
        Debug.Log(rand);
        PlayerUpgradeSO chosen = new PlayerUpgradeSO();
        int randomIdx;
        if (rand < 0.6f) // Buff 60%
        {
            randomIdx = Random.Range(0, buffDeck.Count);
            chosen = buffDeck[randomIdx];
            buffDeck.RemoveAt(randomIdx);
        }
        else if (rand < 0.9f) // Skill 30%
        {
            randomIdx = Random.Range(0, skillDeck.Count);
            chosen = skillDeck[randomIdx];
            skillDeck.RemoveAt(randomIdx);
        }
        else // Trash 10%  
        {
            Debug.Log("Trash");
        }
        return chosen;
    }

    private void SpawnCard()
    {

    }
}
