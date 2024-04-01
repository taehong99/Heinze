using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Weapon weapon;

    public Animator animator;
    private bool isAttacking = false; // �÷��̾ ���������� Ȯ��
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


