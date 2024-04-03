using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Weapon weapon;
    [SerializeField] float attackInterval;
    [SerializeField] float comboCooldown;
    [SerializeField] List<AttackSO> combo;

    private float lastClickedTime;
    private float lastComboEnd;
    int comboCounter;

    PlayerController controller;
    Animator animator;

    public GameObject weaponObject;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        ExitAttack();
    }

    void Attack()
    {
        if (Time.time - lastComboEnd > comboCooldown && comboCounter < combo.Count)
        {
            CancelInvoke("EndCombo");

            if (Time.time - lastClickedTime >= attackInterval)
            {
                controller.isAttacking = true;
                animator.runtimeAnimatorController = combo[comboCounter].animatorOC;
                animator.Play("Attack", 0, 0);
                comboCounter++;
                lastClickedTime = Time.time;

                if (comboCounter > combo.Count)
                {
                    comboCounter = 0;
                }
            }
        }
    }

    void ExitAttack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f &&
            animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            controller.isAttacking = false;
            Invoke("EndCombo", 0.5f);
        }
    }

    public void ForceExitAttack()
    {
        controller.isAttacking = false;
        Invoke("EndCombo", 0);
    }

    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
    }

    public void EnableWeapon()
    {
        weapon.EnableWeapon();
    }

    public void DisableWeapon()
    {
        weapon.DisableWeapon();
    }
}


