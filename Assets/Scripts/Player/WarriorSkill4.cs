using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill4 : MonoBehaviour
{
    const float Skill4Multiplier = 110;

    [SerializeField] LayerMask monsterMask;
    [SerializeField] float explosionRadius;
    [SerializeField] float spinSpeed;
    Collider[] colliders = new Collider[10];

    private void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        int count = Physics.OverlapSphereNonAlloc(collision.GetContact(0).point, explosionRadius, colliders, monsterMask);
        for (int i = 0; i < count; i++)
        {
            IDamagable damagable = colliders[i].GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable?.TakeDamage(Manager.Player.GetAttack(Skill4Multiplier));
            }
        }
        Manager.Pool.GetPool(Manager.Resource.Load<PooledObject>("Effects/WarriorSkill4Explosion"), collision.GetContact(0).point, Quaternion.identity);
        GetComponent<PooledObject>().Release();
    }
}
