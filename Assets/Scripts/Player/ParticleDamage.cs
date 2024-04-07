using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    Collider[] colliders = new Collider[10];
    [SerializeField] float radius;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

    private void OnParticleTrigger()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();

        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        Debug.Log("Triggered");
        int count = Physics.OverlapBoxNonAlloc(enter[0].position, new Vector3(0.5f, 0.5f, 2.5f), colliders, Quaternion.identity,  LayerMask.GetMask("Monster"));
        for(int i = 0; i < count; i++)
        {
            IDamagable damagable = colliders[i].GetComponent<IDamagable>();
            if(damagable != null)
            {
                damagable.TakeDamage(1);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
