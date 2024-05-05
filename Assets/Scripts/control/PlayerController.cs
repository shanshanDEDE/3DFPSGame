using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移動參數")]
    [Tooltip("移動速度")]
    [SerializeField] float moveSpeed = 8;
    [Tooltip("shift加速的倍度")]
    [Range(0, 3)]
    [SerializeField] float sprintSpeedModifier = 2;
    [Tooltip("蹲下時的減速倍數")]
    [Range(0, 1)]
    [SerializeField] float crouchedSpeedModifier = 0.5f;
    [Tooltip("旋轉速度")]
    [SerializeField] float rotateSpeed = 5f;
    [Tooltip("加速度百分比")]
    [SerializeField] float addSpeedRatio = 0.1f;

    [Space(20)]
    [Header("跳躍參數")]
    [Tooltip("跳躍時向上施加的力量")]
    [SerializeField] float jumpForce = 15;
    [Tooltip("跳躍時向下施加的力量")]
    [SerializeField] float gravityDownForce = 50;
    [Tooltip("檢查地面的距離")]
    [SerializeField] float distanceToGround = 0.1f;

    public event Action<bool> onAim;
    public event Action onSprint;

    InputController input;
    CharacterController controller;
    Animator animator;
    Health health;

    Vector3 targetMovement;
    Vector3 jumpDirection;

    float lastFramSpeed;
    //是否在瞄準
    bool isAim;


    void Awake()
    {
        input = GameManagerSingleton.Instance.InputController;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();

        health.onDie += OnDie;
    }

    void Update()
    {
        //瞄準的行為
        AimBehaviour();
        //移動行為
        MoveBehaviour();
        //跳躍行為
        JumpBehaviour();
    }

    //瞄準行為
    private void AimBehaviour()
    {
        bool lastTimeAim = isAim;
        if (input.GetFireInputDown())
        {
            isAim = true;
        }

        if (input.GetAimInputDown())
        {
            isAim = !isAim;
        }

        if (lastTimeAim != isAim)
        {
            onAim?.Invoke(isAim);
        }

        animator.SetBool("IsAim", isAim);

    }

    private void MoveBehaviour()
    {
        targetMovement = Vector3.zero;
        targetMovement += input.GetMoveInput().z * GetCurrentCamaraForward();
        targetMovement += input.GetMoveInput().x * GetCurrentCamaraRight();

        //避免對角線超過1
        targetMovement = Vector3.ClampMagnitude(targetMovement, 1);

        float nextFrameSpeed = 0;

        //是否按下加速
        if (targetMovement == Vector3.zero)
        {
            nextFrameSpeed = 0;
        }
        else if (input.GetSprintInput() && !isAim)
        {

            nextFrameSpeed = 1f;

            targetMovement *= sprintSpeedModifier;
            SmoothRotation(targetMovement);
            onSprint?.Invoke();
        }
        else if (!isAim)
        {
            nextFrameSpeed = 0.5f;

            SmoothRotation(targetMovement);
        }

        if (isAim)
        {
            SmoothRotation(GetCurrentCamaraForward());
        }

        if (lastFramSpeed != nextFrameSpeed)
        {
            lastFramSpeed = Mathf.Lerp(lastFramSpeed, nextFrameSpeed, addSpeedRatio);
        }
        animator.SetFloat("WalkSpeed", lastFramSpeed);
        animator.SetFloat("Vertical", input.GetMoveInput().z);
        animator.SetFloat("Horizontal", input.GetMoveInput().x);

        controller.Move(targetMovement * moveSpeed * Time.deltaTime);
    }

    private void JumpBehaviour()
    {
        if (input.GetJumpInputDown() && IsGround())
        {
            animator.SetTrigger("IsJump");
            jumpDirection = Vector3.zero;
            jumpDirection += Vector3.up * jumpForce;
        }
        jumpDirection.y -= gravityDownForce * Time.deltaTime;
        jumpDirection.y = Mathf.Max(jumpDirection.y, -gravityDownForce);

        controller.Move(jumpDirection * Time.deltaTime);
    }

    private bool IsGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, distanceToGround);
    }


    private void SmoothRotation(Vector3 targetMovement)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetMovement, Vector3.up), rotateSpeed * Time.deltaTime);
    }

    //取得相機右方方向
    private Vector3 GetCurrentCamaraRight()
    {
        Vector3 camaraRight = Camera.main.transform.right;
        camaraRight.y = 0;
        camaraRight.Normalize();
        return camaraRight;
    }
    //取得相機正方方向
    private Vector3 GetCurrentCamaraForward()
    {
        Vector3 camaraForward = Camera.main.transform.forward;
        camaraForward.y = 0;
        camaraForward.Normalize();
        return camaraForward;
    }

    private void OnDie()
    {
        animator.SetTrigger("IsDead");
    }

}
