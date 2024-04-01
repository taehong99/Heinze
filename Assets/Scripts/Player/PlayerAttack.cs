using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Weapon weapon;

    public Animator animator;
    private bool isAttacking = false; // 플레이어가 공격중인지 확인
    public GameObject weaponObject;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            ActivateSkillAnimation("Attack");        
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))   
        {
            ActivateSkillAnimation("Skil1");
           
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateSkillAnimation("Skil2");

        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            ActivateSkillAnimation("Skil3");

        }
    }
  
    public void ActivateSkillAnimation(string skillName)
    {
        if (animator != null && !isAttacking)
        {
            animator.SetTrigger(skillName);
            isAttacking = true;
            StartCoroutine(WaitForAnimation());
        }
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(1f); // 애니메이션 구현 중 (1)초 간은 다른 애니메이션 발생 x 
        isAttacking = false; // 재생 종료
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


