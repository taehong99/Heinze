using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    private bool isAttacking = false; // 플레이어가 공격중인지 확인

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            ActiveAttackAnimation();
          
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

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(1f); // 애니메이션 구현 중 (1)초 간은 다른 애니메이션 발생 x 
        isAttacking = false; // 재생 종료
    }
}


