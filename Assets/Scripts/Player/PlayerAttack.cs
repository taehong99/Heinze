using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    private bool isAttacking = false; // �÷��̾ ���������� Ȯ��

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
        yield return new WaitForSeconds(1f); // �ִϸ��̼� ���� �� (1)�� ���� �ٸ� �ִϸ��̼� �߻� x 
        isAttacking = false; // ��� ����
    }
}


