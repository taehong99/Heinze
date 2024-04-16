using SharpNav.Crowds;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    [SerializeField] WarriorCardDeckSO deck;
    [SerializeField] GameObject cardPrefab;

    [SerializeField] List<PlayerSkillDataSO> skillDeck;
    [SerializeField] List<PlayerBuffSO> buffDeck;
    [SerializeField] List<ConsumableItemSO> trashDeck;

    List<CardUI> cards = new List<CardUI>();

    private void Start()
    {
        skillDeck = new List<PlayerSkillDataSO>(deck.skills);
        buffDeck = new List<PlayerBuffSO>(deck.buffs);
        trashDeck = new List<ConsumableItemSO>(deck.items);

        Manager.Game.SkillPicked += ClearCards;
        Manager.Game.BuffPicked += ClearCards;
        Manager.Game.ItemPicked += ClearCards;
    }

    private void OnDestroy()
    {
        Manager.Game.SkillPicked -= ClearCards;
        Manager.Game.BuffPicked -= ClearCards;
        Manager.Game.ItemPicked -= ClearCards;
    }

    public void RefillDeck()
    {
        skillDeck = new List<PlayerSkillDataSO>(deck.skills);
        buffDeck = new List<PlayerBuffSO>(deck.buffs);
        trashDeck = new List<ConsumableItemSO>(deck.items);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("Pressed 0");
            StartCoroutine(SpawnCards());
        }
    }

    public void ShowCards()
    {
        StartCoroutine(SpawnCards());
    }

    public void ClearCards(PlayerUpgradeSO picked)
    {
        if (picked is PlayerSkillDataSO)
        {
            skillDeck.Remove(picked as PlayerSkillDataSO);
        }
        else if(picked is PlayerBuffSO)
        {
            buffDeck.Remove(picked as PlayerBuffSO);
        }

        foreach (var card in cards)
        {
            Destroy(card.gameObject);
        }
        cards.Clear();
    }

    private IEnumerator SpawnCards()
    {
        HashSet<PlayerUpgradeSO> set = new HashSet<PlayerUpgradeSO>();
        //Choose cards
        for (int i = 0; i < 3; i++)
        {
            PlayerUpgradeSO card = GetRandomCard();
            while (set.Contains(card)){
                card = GetRandomCard();
            }
            set.Add(card);
            CardUI cardUI = Instantiate(cardPrefab, Manager.UI.CardSelectUI.transform).GetComponent<CardUI>();
            cardUI.CardUISet(card);
            cards.Add(cardUI);
            yield return new WaitForSeconds(0.07f);
        }

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].Flip();
            yield return new WaitForSeconds(0.3f);
        }
    }

    private PlayerUpgradeSO GetRandomCard()
    {
        float rand = Random.Range(0f, 1f);
        PlayerUpgradeSO chosen;
        int randomIdx;
        if (rand < 0.6f) // Buff 60%
        {
            if (buffDeck.Count == 0)
            {
                randomIdx = Random.Range(0, trashDeck.Count);
                return trashDeck[randomIdx];
            }
            randomIdx = Random.Range(0, buffDeck.Count);
            chosen = buffDeck[randomIdx];
        }
        else if (rand < 0.9f) // Skill 30%
        {
            if(skillDeck.Count == 0)
            {
                randomIdx = Random.Range(0, trashDeck.Count);
                return trashDeck[randomIdx];
            }
            randomIdx = Random.Range(0, skillDeck.Count);
            chosen = skillDeck[randomIdx];
        }
        else // Trash 10%  
        {
            randomIdx = Random.Range(0, trashDeck.Count);
            chosen = trashDeck[randomIdx];
        }
        return chosen;
    }
}
