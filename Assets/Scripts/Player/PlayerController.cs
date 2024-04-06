using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float sprintSpeedDelta;
    private float moveSpeed;

    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCooldown;
    private float dashCooldownTimer;
    private bool isDashing;
    public bool isAttacking;

    //스킬 컨트롤러
    //private String[] skilName = new string[3] { "Skil1", "Skil2", "Skil3" };

    // 선택된 슬롯 스킬 업데이트
    //private InventoryManager inventoryManager;

    CharacterController controller;
    Animator animator;
    PlayerAttack attacker;
    Vector3 moveDir;
    Vector3 forwardDir;
    Vector3 rightDir;
    public event Action InteractPressed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        attacker = GetComponent<PlayerAttack>();
    }

    private void Start()
    {
        moveSpeed = Manager.Player.MoveSpeed;
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
        HandleGravity();
    }

    float rotationSpeed = 10f;
    private void Move()
    {
        if (isDashing || isAttacking)
            return;

        forwardDir = Camera.main.transform.forward;
        forwardDir = new Vector3(forwardDir.x, 0, forwardDir.z).normalized;

        rightDir = Camera.main.transform.right;
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
    private void HandleGravity()
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

    private void Dash()
    {
        if (dashCooldownTimer > 0)
            return;

        animator.Play("Dash");
        StartCoroutine(DashRoutine());
        StartCoroutine(DashCooldownRoutine());
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;
        attacker.ForceExitAttack();
        float time = 0; // need to remember this to know how long to dash

        Vector3 dashDir;
        if (moveDir.magnitude == 0)
        {
            dashDir = transform.forward;
        }
        else
        {
            dashDir = forwardDir * moveDir.z + rightDir * moveDir.x;
        }
        
        while (time < dashDuration)
        {
            controller.Move(dashDir * dashSpeed * Time.deltaTime);
            time += Time.deltaTime;
            yield return null; // this will make Unity stop here and continue next frame
        }
        isDashing = false;
    }

    private IEnumerator DashCooldownRoutine()
    {
        dashCooldownTimer = dashCooldown;
        while(dashCooldownTimer >= 0)
        {
            dashCooldownTimer -= Time.deltaTime;
            yield return null;
        }
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

    private void OnDash()
    {
        Dash();
    }

    private void OnInteract()
    {
        InteractPressed?.Invoke();
    }
}
