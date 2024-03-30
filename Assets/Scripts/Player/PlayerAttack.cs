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
            ActiveAttackAnimation();        
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))   
        {
            ActiveSkilAnimation();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActiveSkil1Animation();
        }
    }
    public void ActiveAttackAnimation()
    {   
        if(animator != null)
        {
            if (!isAttacking) 
            {
                animator.SetTrigger("Attack");
                isAttacking = true;
                StartCoroutine(WaitForAnimation());
            }
        }


    }

    public void ActiveSkilAnimation()
    {
        if(animator != null)
        {
            if (!isAttacking)
            {
                animator.SetTrigger("Skil");
                isAttacking = true;
                StartCoroutine(WaitForAnimation());
            }
        }
    }

    public void ActiveSkil1Animation()
    {
        if (animator != null)
        {
            if (!isAttacking)
            {
                animator.SetTrigger("Skil1");
                isAttacking = true;
                StartCoroutine(WaitForAnimation());
            }
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


