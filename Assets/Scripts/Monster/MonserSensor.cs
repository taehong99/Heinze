using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonserSensor : MonoBehaviour
{
    [field : SerializeField]public Transform target { get; set; }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;
        }
    }
}
