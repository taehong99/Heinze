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
    string skillName = "Skil1"; // Ȱ��ȭ�� ��ų �̸�



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
            ActiveSkilAnimation();
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

        if(animator != null)
        {

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


