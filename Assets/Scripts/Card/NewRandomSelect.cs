using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRandomSelect : MonoBehaviour
{
    public List<PlayerSkillDataSO> skillDeck = new List<PlayerSkillDataSO>();
    public List<PlayerBuffSO> buffDeck = new List<PlayerBuffSO>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SpawnCards();
        }
    }

    private void SpawnCards()
    {

    }

    private void SpawnCard()
    {

    }
}
