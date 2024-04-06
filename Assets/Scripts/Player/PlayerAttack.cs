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
        if (MouseManager.Instance.Left)
        {
            ActivateSkillAnimation("Attack");        
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))   
        {
            Attack();
        }
        ExitAttack();
    }

    #region Basic Attacks
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
    #endregion

    #region Skills

    private void CrescentSlash() // Skill1
    {
        Manager.Pool.GetPool(Manager.Resource.Load<PooledObject>("Effects/WarriorSkill1"), transform.position + Vector3.up * 0.8f, transform.rotation);
    }

    private void GoldenSword() // Skill2
    {
        Manager.Pool.GetPool(Manager.Resource.Load<PooledObject>("Effects/WarriorSkill2"), transform.position + transform.forward * 7f, transform.rotation);
    }

    #endregion

    public void EnableWeapon()
    {
        weapon.EnableWeapon();
    }

    public void DisableWeapon()
    {
        weapon.DisableWeapon();
    }

    private void OnSkill1()
    {
        CrescentSlash();
    }

    private void OnSkill2()
    {
        GoldenSword();
    }
}


