using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum MonsterType { Normal, Ranged, Boss1, Boss, ExplodeMonster}
public class MonsterSight : MonoBehaviour
{
    [SerializeField] MonsterType type;
    [SerializeField] Monster monster;
    [SerializeField] RangedMonster RangedMonster;
    [SerializeField] BossMonster1 BossMonster1;
    [SerializeField] BossMonster BossMonster;
    [SerializeField] ExplodeMonster ExplodeMonster;
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(type == MonsterType.Normal)
            {
                monster.Detect(other.transform);
            }
            else if(type ==  MonsterType.Ranged)
            {
                RangedMonster.Detect(other.transform);
            }
            else if(type == MonsterType.Boss1)
            {
               BossMonster1.Detect(other.transform);
            }        
            else if(type == MonsterType.Boss)
            {
                //BossMonster.Detect(other.transform);
            }
            else if(type == MonsterType.ExplodeMonster)
            {
                ExplodeMonster.Detect(other.transform);
            }
        }
    }
}
