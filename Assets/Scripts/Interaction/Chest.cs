using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    Canvas canvas;
    Animator animator;

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>(true);
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        animator.Play("Open");
    }

    private void ShowCards()
    {
        Manager.Game.ShowCards();
        Destroy(gameObject);
    }

    public void ShowUI()
    {
        Debug.Log("Show UI");
        canvas.gameObject.SetActive(true);
    }

    public void HideUI()
    {
        Debug.Log("Hide UI");
        canvas.gameObject.SetActive(false);
    }
}
