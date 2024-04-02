using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Weapon weapon;
    [SerializeField] CharacterController controller;

    public Animator animator;

    [SerializeField] private bool isAttacking = false; // �÷��̾ ���������� Ȯ��p
    public GameObject weaponObject;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        //if (Input.GetKeyDown(KeyCode.Alpha1))   
        //{
        //    ActivateSkillAnimation("Skil1");
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    ActivateSkillAnimation("Skil2");
        //}
        //if (Input.GetKey(KeyCode.Alpha3))
        //{
        //    ActivateSkillAnimation("Skil3");
        //}
    }

    private int count = 0;
    private void Attack()
    {
        controller.enabled = false;
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slam")
        {
            count = 0;
            return;
        }

        if(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Pierce")
        {
            if(count == 0)
            {
                Debug.Log("Attack2");
                animator.SetTrigger("Attack2");
                count++;
            }
            else
            {
                Debug.Log("Attack3");
                animator.SetTrigger("Attack3");
            }
        }
        else if(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Slash")
        {
            Debug.Log("Attack4");
            animator.SetTrigger("Attack4");
        }
        else
        {
            Debug.Log("Attack1");
            animator.SetTrigger("Attack1");
        }
    }

    private void EndAttack()
    {
        count = 0;
        controller.enabled = true;
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
        yield return new WaitForSeconds(1f); // �ִϸ��̼� ���� �� (1)�� ���� �ٸ� �ִϸ��̼� �߻� x
        isAttacking = false; // ��� ����
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


