using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Sword, Bow, Staff }
public class WeaponPickup : MonoBehaviour, IInteractable
{
    [SerializeField] WeaponType weaponType;
    Canvas canvas;

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>(true);
    }

    public void Interact()
    {
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
