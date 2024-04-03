using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterSight : MonoBehaviour
{
    [SerializeField] Monster monster;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monster.Detect(other.transform);
        }
    }
}
