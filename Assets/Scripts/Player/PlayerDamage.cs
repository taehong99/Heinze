using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour, IDamagable
{
    [SerializeField] DamageText damageTextPrefab;
    [SerializeField] Transform spawnPoint;

    private void Start()
    {
        Manager.Player.PlayerHealed += ShowHealText;
    }

    private void OnDestroy()
    {
        Manager.Player.PlayerHealed -= ShowHealText;
    }

    private void ShowHealText(int amount)
    {
        DamageText damageText = Instantiate(damageTextPrefab, spawnPoint.position, Quaternion.identity);
        damageText.SetColor(Color.green);
        damageText.damage = amount;
    }

    public void TakeDamage(int damage)
    {
        int dmg = Manager.Player.CalculateTakenDamage(damage);
        Manager.Player.TakeDamage(dmg);
        DamageText damageText = Instantiate(damageTextPrefab, spawnPoint.position, Quaternion.identity);
        damageText.damage = dmg;
    }
}
