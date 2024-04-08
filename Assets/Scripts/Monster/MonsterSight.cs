using DungeonArchitect.Editors.SnapFlow;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum MonsterType { Normal, Ranged }
public class MonsterSight : MonoBehaviour
{
    [SerializeField] MonsterType type;
    [SerializeField] Monster monster;
    [SerializeField] RangedMonster RangedMonster;
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(type == MonsterType.Normal)
            {
                monster.Detect(other.transform);
            }
            else
            {
                RangedMonster.Detect(other.transform);
            }
            
            
        }
    }
}
