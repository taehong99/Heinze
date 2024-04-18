using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Collider attackCollider;

    public void EnableWeapon()
    {
        attackCollider.enabled = true;
    }
    
    public void DisableWeapon()
    {
        attackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            return;

        IDamagable damagable = other.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage(Manager.Player.GetAttack(100));
        }
    }
}
