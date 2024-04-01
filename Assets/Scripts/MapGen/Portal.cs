using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] Portal destination;
    [SerializeField] Vector3 spawnPos;

    private void OnTriggerEnter(Collider other)
    {
        // teleport player to receiver portal
        if (!other.CompareTag("Player"))
            return;

        CharacterController cc = other.GetComponent<CharacterController>();
        cc.enabled = false;
        other.transform.position = destination.spawnPos;
        cc.enabled = true;
    }
}
