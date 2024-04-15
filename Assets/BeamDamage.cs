using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamDamage : MonoBehaviour
{
    public int damagePerSecond = 10;
    private bool isDamaging = false;

    enum State
    {
        DealDamage
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamagable damagable = other.GetComponent<IDamagable>();
        if(damagable != null)
        {
            isDamaging = true;
            StartCoroutine(DealDamage(damagable));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isDamaging = false;
    }

    private IEnumerator DealDamage(IDamagable damagable)
    {
        while (isDamaging)
        {
            damagable.TakeDamage(damagePerSecond);
            yield return new WaitForSeconds(1f);
        }
    }
}
