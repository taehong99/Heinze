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

    [Header("Skills")]
    [SerializeField] float skill1Radius;
    [SerializeField] Transform skill2DamagePoint;
    [SerializeField] LayerMask monsterMask;

    private float lastClickedTime;
    private float lastComboEnd;
    int comboCounter;

    PlayerController controller;
    Animator animator;
    Collider[] colliders = new Collider[10];

    public GameObject weaponObject;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (MouseManager.Instance.Left)        {
            //ActivateSkillAnimation("Attack");
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

    protected virtual void UseSkill(int skillId)
    {
        ForceExitAttack();
        switch (skillId)
        {
            case 0:
                animator.Play("CrescentSlash");
                break;
            case 1:
                animator.Play("SummonSword");
                SummonSword();
                break;
        }
    }

    private void CrescentSlash() // Skill1
    {
        Manager.Pool.GetPool(Manager.Resource.Load<PooledObject>("Effects/WarriorSkill1"), transform.position + Vector3.up * 0.8f, transform.rotation);
        int count = Physics.OverlapSphereNonAlloc(transform.position, skill1Radius, colliders, monsterMask);
        for(int i = 0; i < count; i++)
        {
            IDamagable damagable = colliders[i].GetComponent<IDamagable>();
            damagable?.TakeDamage(1);
        }
    }

    private void SummonSword() // Skill2
    {
        Manager.Pool.GetPool(Manager.Resource.Load<PooledObject>("Effects/WarriorSkill2"), transform.position + transform.forward * 7f, transform.rotation);
    }

    private void Skill2Damage()
    {
        int count = Physics.OverlapBoxNonAlloc(skill2DamagePoint.position, new Vector3(0.5f, 1f, 5f), colliders, skill2DamagePoint.rotation, monsterMask);
        for (int i = 0; i < count; i++)
        {
            IDamagable damagable = colliders[i].GetComponent<IDamagable>();
            damagable?.TakeDamage(1);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, skill1Radius);
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
        UseSkill(0);
    }

    private void OnSkill2()
    {
        UseSkill(1);
    }
}


