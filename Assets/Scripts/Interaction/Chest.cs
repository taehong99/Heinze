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
        Manager.Player.Freeze();
        Manager.Sound.PlaySFX(Manager.Sound.AudioClips.chestOpenSFX);
        animator.Play("Open");
    }

    private void ShowCards()
    {
        Manager.Game.ShowCards();
        Manager.Sound.PlaySFX(Manager.Sound.AudioClips.cardFlipSFX);
        Destroy(gameObject);
    }

    public void ShowUI()
    {
        canvas.gameObject.SetActive(true);
    }

    public void HideUI()
    {
        canvas.gameObject.SetActive(false);
    }
}
