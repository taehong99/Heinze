using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill5 : MonoBehaviour
{
    const float Skill5Multiplier = 90;

    [SerializeField] float damageInterval;
    [SerializeField] LayerMask monsterMask;
    Collider[] colliders = new Collider[10];

    private void OnEnable()
    {
        InvokeRepeating("DealDamage", 0, damageInterval);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    void DealDamage()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, 10, colliders, monsterMask);
        for (int i = 0; i < count; i++)
        {
            IDamagable damagable = colliders[i].GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable?.TakeDamage(Manager.Player.CalculateDamage(Skill5Multiplier));
            }
        }
    }
}
