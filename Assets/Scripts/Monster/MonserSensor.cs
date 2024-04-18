using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonserSensor : MonoBehaviour
{
    [field : SerializeField]public Transform target { get; private set; }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
        }
    }
}
