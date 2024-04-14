using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour, IInteractable
{
    Canvas canvas;
    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>(true);
    }

    public void Interact()
    {
        Manager.Game.ObtainPotions(1);
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
