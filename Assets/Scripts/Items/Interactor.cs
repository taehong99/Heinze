using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] LayerMask interactableMask;
    private IInteractable curInteractable;

    private void Start()
    {
        curInteractable = null;

        // subscribe to player interact input event
        transform.GetComponentInParent<PlayerController>().InteractPressed += OnInteract;
    }

    private void OnInteract()
    {
        if (curInteractable == null)
            return;

        Debug.Log("Interact");
        curInteractable.HideUI();
        curInteractable.Interact();
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if(interactable != null)
        {
            curInteractable = interactable;
            curInteractable.ShowUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            curInteractable.HideUI();
            curInteractable = null;
        }
    }
}
