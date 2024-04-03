using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float sprintSpeedDelta;
    [SerializeField] float jumpHeight;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance;
    [SerializeField] LayerMask groundMask;

    //스킬 컨트롤러
    //private String[] skilName = new string[3] { "Skil1", "Skil2", "Skil3" };

    // 선택된 슬롯 스킬 업데이트
    //private InventoryManager inventoryManager;

    CharacterController controller;
    Animator animator;
    Vector3 moveDir;
    public event Action InteractPressed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetBool("isSprinting", true);
            moveSpeed += sprintSpeedDelta;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("isSprinting", false);
            moveSpeed -= sprintSpeedDelta;
        }
        Move();
        HandleGravityAndJump();
    }

    float rotationSpeed = 10f;
    private void Move()
    {
        Vector3 forwardDir = Camera.main.transform.forward;
        forwardDir = new Vector3(forwardDir.x, 0, forwardDir.z).normalized;

        Vector3 rightDir = Camera.main.transform.right;
        rightDir = new Vector3(rightDir.x, 0, rightDir.z).normalized;

        controller.Move(forwardDir * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(rightDir * moveDir.x * moveSpeed * Time.deltaTime);

        Vector3 lookDir = forwardDir * moveDir.z + rightDir * moveDir.x;
        if (lookDir.sqrMagnitude > 0) // if(lookDir != Vector3.zero) <= faster alternative
        {
            Quaternion lookRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        // 벡터 투영 (오르막길 속도 구현)
        //Vector3.Project()
    }

    Vector3 velocity;
    bool isGrounded;
    private void HandleGravityAndJump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        animator.SetBool("isGrounded", isGrounded);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }
        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (!isGrounded)
            return;

        animator.SetTrigger("Jump");
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
    }

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveDir.x = input.x;
        moveDir.z = input.y;

        animator.SetFloat("moveSpeed", moveDir.magnitude);
        animator.SetFloat("xSpeed", moveDir.x);
        animator.SetFloat("zSpeed", moveDir.z);
    }

    private void OnJump()
    {
        Jump();
    }

    private void OnInteract()
    {
        InteractPressed?.Invoke();
    }
}
