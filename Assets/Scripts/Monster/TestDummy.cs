using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDummy : MonoBehaviour, IDamagable
{
    Renderer rend; 
    Color og = Color.white;
    Color red = Color.red;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Took damage");
        StartCoroutine(ColorChangeRoutine());
        Manager.Game.ShowCards();
    }



    IEnumerator ColorChangeRoutine()
    {
        rend.material.color = red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = og;
        StopAllCoroutines();
    }
}
