using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    [SerializeField] LayerMask monsterMask;
    HashSet<Collider> damaged = new HashSet<Collider>();
    Collider[] colliders = new Collider[10];

    private void OnDisable()
    {
        damaged.Clear();
    }

    private void Update()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, 5, colliders, monsterMask);
        Debug.Log(count);
        for (int i = 0; i < count; i++)
        {
            if (damaged.Contains(colliders[i]))
                return;

            IDamagable damagable = colliders[i].GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(1);
                damaged.Add(colliders[i]);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5);
    }
}
