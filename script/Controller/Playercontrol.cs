using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    None,
    HandGun,
    Rifle
}

public class Playercontrol : MonoBehaviour
{
    [Header("移動參數")]
    [Tooltip("移動速度")]
    [SerializeField] float MoveSpeed = 4;
    [Tooltip("奔跑加速")]
    [Range(1,3)]
    [SerializeField] float SprintSpeedModifier = 2;
    // [Tooltip("蹲下時的減速與倍速")]
    // [Range(0,1)]
    // [SerializeField] float CrouchedSpeedModifier = 0.5f;
    [Tooltip("角色旋轉速度")]
    [SerializeField] float RotateSpeed = 5f;
    [Tooltip("加速度百分比")]
    [SerializeField] float addSpeedRatio = 0.05f;

    [Space(20)]
    [Header("跳躍參數")]
    [Tooltip("跳躍時向上施加的力量")]
    [SerializeField] float JumpForce = 10;
    [Tooltip("跳躍時向下施加的力量")]
    [SerializeField] float GravityDownForce = 50;
    [Tooltip("檢查與地面之間的距離")]
    [SerializeField] float DistanceToGround = 0.1f;
    [Header("武器參數")]
    public WeaponType currentWeapon = WeaponType.None;

    public GameObject handGun;
    public GameObject rilfe;

    public event Action<bool> OnAim;

    InputControl input;
    CharacterController controller;
    Animator animator;
    Health health;

    //每一幀要移動到的目標位置
    Vector3 TargetMovement;
    //每一幀要跳躍到的目標位置
    Vector3 JumpDirection;
    //上一幀的移動速度
    float lastFrameSpeed;
    //是否為瞄準狀態
    bool isAim;

    private void Awake()
    {
        // input = GameManagerSingleton.Instance.inputControl;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();

        health.OnDead += onDead;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(WeaponType.HandGun);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(WeaponType.Rifle);
        }
        //瞄準行為
        AimBehaviour();
        //移動行為
        MoveBehaviour();
        //跳躍行為
        JumpBehaviour();
        //根據武器類型更新動畫參數
        UpdateWeaponAnimation();

    }

    private void UpdateWeaponAnimation()
    {
        animator.SetBool("IsHandGun", currentWeapon == WeaponType.HandGun);
        animator.SetBool("IsRilfe", currentWeapon == WeaponType.Rifle);
    }

    public void SwitchWeapon(WeaponType newWeapon)
    {
        currentWeapon = newWeapon;

        handGun.SetActive(currentWeapon == WeaponType.HandGun);
        rilfe.SetActive(currentWeapon == WeaponType.Rifle);

        UpdateWeaponAnimation();
    }

    private void AimBehaviour()
    {
        if(input.GetFireInputDown())
        {
            isAim = true;
        }
        if(input.GetAimInputDown())
        {
            isAim = !isAim;
        }

        animator.SetBool("IsAim", isAim);
        if(isAim)
        {
            OnAim?.Invoke(isAim);
            animator.SetFloat("WalkSpeed",0);
        }
    }

    //處理移動行為
    private void MoveBehaviour()
    {
        TargetMovement = Vector3.zero;
        TargetMovement += input.GetMoveInput().z * GetCurrentCameraForward();
        TargetMovement += input.GetMoveInput().x * GetCurrentCameraRight();

        //避免對角線超過1
        TargetMovement = Vector3.ClampMagnitude(TargetMovement,1);

        // 下一幀的移動速度
        float nextFrameSpeed = 0;

        //如果按下奔跑鍵
        if(TargetMovement == Vector3.zero)
        {
            nextFrameSpeed = 0f;
        }
        else if(input.GetSprintInput()  && !isAim)
        {
            nextFrameSpeed = 1f;
            TargetMovement *= SprintSpeedModifier;

            SmoothRotation(TargetMovement);
        }
        else if(!isAim)
        {
            nextFrameSpeed = 0.5f;

            SmoothRotation(TargetMovement);
        }

        if(isAim)
        {
            SmoothRotation(GetCurrentCameraForward());
        }

        if(lastFrameSpeed != nextFrameSpeed)
            lastFrameSpeed = Mathf.Lerp(lastFrameSpeed,nextFrameSpeed,addSpeedRatio);

        animator.SetFloat("WalkSpeed",lastFrameSpeed);
        animator.SetFloat("Vertical",input.GetMoveInput().z);
        animator.SetFloat("Horizontal",input.GetMoveInput().x);

        controller.Move(TargetMovement * Time.deltaTime * MoveSpeed);
    }

    //處理跳躍行為
    private void JumpBehaviour()
    {
        if(input.GetJumpInputDown() && IsGrounded())
        {
            animator.SetTrigger("IsJump");
            JumpDirection = Vector3.zero;
            JumpDirection += JumpForce * Vector3.up;
        }

        JumpDirection.y -= GravityDownForce * Time.deltaTime;
        JumpDirection.y = Mathf.Max(JumpDirection.y,-GravityDownForce);

        controller.Move(JumpDirection * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, DistanceToGround);
    }

    // //平滑旋轉角度
    private void SmoothRotation(Vector3 TargetMovement)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(TargetMovement,Vector3.up),RotateSpeed * Time.deltaTime);
    }

    //取得相機正方方向
    private Vector3 GetCurrentCameraForward()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();
        return cameraForward;
    }

    //取得相機右方方向
    private Vector3 GetCurrentCameraRight()
    {
        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();
        return cameraRight;
    }

    private void onDead()
    {
        animator.SetTrigger("IsDead");
    }
}
