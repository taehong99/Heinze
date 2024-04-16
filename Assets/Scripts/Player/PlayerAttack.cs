using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    const float Skill1Multiplier = 180;
    const float Skill2Multiplier = 300;

    [Header("Basic Attack")]
    [SerializeField] Weapon weapon;
    [SerializeField] float attackInterval;
    [SerializeField] float comboCooldown;
    [SerializeField] List<AttackSO> combo;
    [SerializeField] LayerMask monsterMask;
    private float lastClickedTime;
    private float lastComboEnd;
    int comboCounter;

    [Header("Skills")]
    [SerializeField] WarriorCardDeckSO warriorDeck; //TODO: Refactor
    public float[] cooldownTimers;

    [Header("Skill1")]
    [SerializeField] float skill1Radius;

    [Header("Skill2")]
    [SerializeField] Transform skill2DamagePoint;

    [Header("Skill4")]
    [SerializeField] float skill4Duration;
    [SerializeField] float skill4SpawnInterval;

    [Header("Skill6")]
    [SerializeField] float skill6BuffDuration;

    [Header("Misc")]
    PlayerController controller;
    PlayerEffects effects;
    Animator animator;
    Collider[] colliders = new Collider[20];
    Plane plane = new Plane(Vector3.up, 0);

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        effects = GetComponent<PlayerEffects>();
        animator = GetComponent<Animator>();
        cooldownTimers = new float[warriorDeck.skills.Count];
        Manager.Game.AssignPlayer(this);
    }

    private void Update()
    {
        //if (MouseManager.Instance.Left){
        //    Attack();   
        //}
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
                LookAtMouse();
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

    private void Freeze()
    {
        controller.isAttacking = true;
    }
    private void UnFreeze()
    {
        controller.isAttacking = false;
    }

    private void PlayAttackSound(int index)
    {
        switch (index)
        {
            case 1:
                Manager.Sound.PlaySFX(Manager.Sound.AudioClips.attack1SFX);
                break;
            case 2:
                Manager.Sound.PlaySFX(Manager.Sound.AudioClips.attack2SFX);
                break;
            case 3:
                Manager.Sound.PlaySFX(Manager.Sound.AudioClips.attack3SFX);
                break;
            default:
                break;
        }
            
    }

    #endregion

    #region Skills

    protected virtual void UseSkill(int skillId)
    {
        ForceExitAttack();
        LookAtMouse();
        switch (skillId)
        {
            case 1:
                animator.Play("Skill1");
                break;
            case 2:
                animator.Play("Skill2");
                Skill2();
                break;
            case 3:
                animator.Play("Skill3");
                break;
            case 4:
                animator.Play("Skill4");
                Skill4();
                break;
            case 5:
                animator.Play("Skill5");
                Skill5();
                break;
            case 6:
                animator.Play("Skill6");
                Skill6();
                break;
        }
    }

    // Skill1
    private void Skill1()
    {
        Freeze();
        Invoke("UnFreeze", 0.4f);
        effects.PlayEffect("Skill1");
        int count = Physics.OverlapSphereNonAlloc(transform.position, skill1Radius, colliders, monsterMask);
        for(int i = 0; i < count; i++)
        {
            IDamagable damagable = colliders[i].GetComponent<IDamagable>();
            damagable?.TakeDamage(Manager.Player.GetAttack(Skill1Multiplier));
        }
    }

    // Skill2
    private void Skill2()
    {
        Freeze();
        Invoke("UnFreeze", 0.7f);
        //Manager.Pool.GetPool(Manager.Resource.Load<PooledObject>("Effects/WarriorSkill2"), transform.position + transform.forward * 7f, transform.rotation);
        effects.PlayEffect("Skill2");
    }

    private void Skill2Damage()
    {
        int count = Physics.OverlapBoxNonAlloc(skill2DamagePoint.position, new Vector3(2.5f, 1f, 5f), colliders, skill2DamagePoint.rotation, monsterMask);
        for (int i = 0; i < count; i++)
        {
            IDamagable damagable = colliders[i].GetComponent<IDamagable>();
            damagable?.TakeDamage(Manager.Player.GetAttack(Skill2Multiplier));
        }
    }

    // Skill3
    private void Skill3()
    {
        StartCoroutine(SummonWaves());
    }
    
    private IEnumerator SummonWaves()
    {
        Freeze();
        for(int i = 0; i < 2; i++)
        {
            effects.PlayEffect("Skill3");
            yield return new WaitForSeconds(0.5f);
        }
        effects.PlayEffect("Skill3");
        UnFreeze();
    }

    // Skill4
    private void Skill4()
    {
        Freeze();
        Invoke("UnFreeze", 1f);
        //effects.PlayEffect("Skill4");
        StartCoroutine(Skill4Routine());
    }

    private IEnumerator Skill4Routine()
    {
        float t1 = 0;
        float t2 = 0;
        while (t1 < skill4Duration)
        {
            t1 += Time.deltaTime;
            t2 += Time.deltaTime;
            if (t2 > skill4SpawnInterval)
            {
                effects.PlayEffect("Skill4");
                t2 = 0;
            }
            yield return null;
        }
    }

    // Skill5
    private void Skill5()
    {
        Freeze();
        Invoke("UnFreeze", 1f);
        effects.PlayEffect("Skill5");
    }

    // Skill6
    private void Skill6()
    {
        Freeze();
        Invoke("UnFreeze", 1f);
        effects.PlayEffect("Skill6");
        Manager.Player.TakeDamage(10);
        Manager.Player.UpdateStat(Stat.Attack, IncreaseRate.Percent, 0.5f);
        Manager.Player.UpdateStat(Stat.Defense, IncreaseRate.Percent, 0.5f);
        Invoke("RemoveBuff", skill6BuffDuration);
    }

    private void RemoveBuff()
    {
        Manager.Player.UpdateStat(Stat.Attack, IncreaseRate.Percent, -0.5f);
        Manager.Player.UpdateStat(Stat.Defense, IncreaseRate.Percent, -0.5f);
    }

    #endregion

    Vector3 worldPosition;
    void LookAtMouse()
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
        }
        transform.LookAt(worldPosition);
    }

    public float GetCooldownRatio(int id)
    {
        return cooldownTimers[id] / warriorDeck.skills[id].cooldown;
    }

    private IEnumerator CooldownTimerRoutine(int id)
    {
        if (id == 0)
            yield break;

        cooldownTimers[id] = warriorDeck.skills[id].cooldown;
        while (cooldownTimers[id] > 0)
        {
            cooldownTimers[id] -= Time.deltaTime;
            yield return null;
        }
        cooldownTimers[id] = 0;
    }

    public void EnableWeapon()
    {
        weapon.EnableWeapon();
    }

    public void DisableWeapon()
    {
        weapon.DisableWeapon();
    }

    private void OnAttack()
    {
        Attack();
    }

    PlayerSkillDataSO skill;
    private void OnSkill1()
    {
        skill = Manager.Game.GetSkillInSlot(0);
        if (skill == null || cooldownTimers[skill.id] > 0)
            return;

        UseSkill(skill.id);
        StartCoroutine(CooldownTimerRoutine(skill.id));
    }

    private void OnSkill2()
    {
        skill = Manager.Game.GetSkillInSlot(1);
        if (skill == null || cooldownTimers[skill.id] > 0)
            return;
        UseSkill(skill.id);
        StartCoroutine(CooldownTimerRoutine(skill.id));
    }

    private void OnSkill3()
    {
        skill = Manager.Game.GetSkillInSlot(2);
        if (skill == null || cooldownTimers[skill.id] > 0)
            return;
        UseSkill(skill.id);
        StartCoroutine(CooldownTimerRoutine(skill.id));
    }

    private void OnSkill4()
    {
        skill = Manager.Game.GetSkillInSlot(3);
        if (skill == null || cooldownTimers[skill.id] > 0)
            return;
        UseSkill(skill.id);
        StartCoroutine(CooldownTimerRoutine(skill.id));
    }
}


