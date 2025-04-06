using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FPScontrol : MonoBehaviour
{
    public float Movespeed = 3f;
    public float Runspeed = 6f;
    private float horizontal;
    private float vertical;
    private float gravitiy = 18f;
    public float JumpSpeed = 5f;
    private float smoothSpeed = 0f;
    private float smoothVelocity = 0f;
    public AudioClip SelectWeapon;
    private AudioSource audioSource;

    public Animator handGunAnimator;
    public Animator rilfeAnimator;
    private Animator activeAnimator;
    public GameObject handGun;
    public GameObject rifle;

    public float currentSpeed {get;private set;}
    CharacterController PlayerController;
    InputControl input;
    Vector3 Player_Move;
    PlayerHealth Playerhealth;

    public delegate void OnJumpEvent(bool isJumping);
    public event OnJumpEvent JumpEvent;
    public delegate void OndeadCamera(bool isDeadcamera);
    public event OndeadCamera Deadcamera;

    private bool isJumping = false;
    public bool isDeadcamera = false;
    private float jumpCooldown = 0.5f;
    private float lastJumptime = 0.5f;

    public GameObject deathScreen;

    public enum CurrentState
    {
        IDLE = 0,
        walk,
        run
    }

    public enum WeaponType
    {
        handGun,
        Rilfe
    }

    public WeaponType currentWeapon = WeaponType.handGun; //初始武器為手槍

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        PlayerController = GetComponent<CharacterController>();
        Transform cameraTransform = transform.Find("fpscam");
        audioSource.PlayOneShot(SelectWeapon);

        SwitchWeapon(currentWeapon);

        Playerhealth = GetComponent<PlayerHealth>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Playerhealth.PlayerOnDead += PlayeronDead;
    }

    void Update()
    {
        currentSpeed = 0;

        if (PlayerController.isGrounded)
        {
            Player_Move.y = 0;
            if(isJumping)
            {
                isJumping = false;
                JumpEvent?.Invoke(false);
            }
            horizontal = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f ? Input.GetAxis("Horizontal") : 0f;
            vertical = Mathf.Abs(Input.GetAxis("Vertical")) > 0.01f ? Input.GetAxis("Vertical") : 0f;
            if(horizontal != 0 || vertical != 0)
            {
                currentSpeed = Movespeed;
                if (Input.GetKey(KeyCode.LeftShift) && vertical > 0)
                {
                    currentSpeed = Runspeed;
                }
                Player_Move = (transform.forward * vertical + transform.right * horizontal) * currentSpeed;
            }

            float movementSpeed = Mathf.Clamp01(new Vector2(horizontal, vertical).magnitude);
            
            if(activeAnimator != null)
            {
                if(movementSpeed == 0)
                {
                    smoothSpeed = 0;
                }
                else
                {
                    float targetSpeed = movementSpeed * (currentSpeed == Runspeed? 1f : 0.5f);
                    smoothSpeed = Mathf.SmoothDamp(smoothSpeed, targetSpeed, ref smoothVelocity, 0.1f);
                }
                activeAnimator.SetFloat("WalkSpeed",smoothSpeed);
            }

            if (Input.GetAxis("Jump") == 1 && Time.time >= lastJumptime + jumpCooldown)
            {
                Player_Move.y += JumpSpeed;

                if(!isJumping)
                {
                    isJumping = true;
                    JumpEvent?.Invoke(true);
                    lastJumptime = Time.time;
                }
            }
        }
        else
        {
            Player_Move.y -= gravitiy * Time.deltaTime;
        }
        Player_Move.y -= gravitiy * Time.deltaTime;
        PlayerController.Move(Player_Move * Time.deltaTime);

        HandleWeapomSwitch();
    }

    private void HandleWeapomSwitch()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && currentWeapon != WeaponType.handGun)
        {
            SwitchWeapon(WeaponType.handGun);
            audioSource.PlayOneShot(SelectWeapon);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && currentWeapon != WeaponType.Rilfe)
        {
            SwitchWeapon(WeaponType.Rilfe);
            audioSource.PlayOneShot(SelectWeapon);
        }
    }


    private void SwitchWeapon(WeaponType newWeapon)
    {
        currentWeapon = newWeapon;

        //快速刷新武器模型
        StartCoroutine(RefreshWeapon());

        //更新當前武器的動畫
        activeAnimator = currentWeapon == WeaponType.handGun ? handGunAnimator : rilfeAnimator;

        if(activeAnimator != null)
        {
            activeAnimator.SetFloat("WalkSpeed", 0);
        }
    }

    private IEnumerator RefreshWeapon()
    {
        //同時啟用兩把武器，避免UI無法正常顯示
        handGun.SetActive(true);
        rifle.SetActive(true);

        //等待下一幀
        yield return null;

        if(currentWeapon == WeaponType.handGun)
        {
            //如果目前武器是手槍就隱藏步槍
            rifle.SetActive(false);
        }
        else if(currentWeapon == WeaponType.Rilfe)
        {
            //如果目前武器是步槍就隱藏手槍
            handGun.SetActive(false);
        }
    }

    private void PlayeronDead()
    {
        isDeadcamera = true;
        Deadcamera?.Invoke(true);
        
        if(deathScreen != null)
        {
            deathScreen.SetActive(true);
        }

        StartCoroutine(ExitGameAfterDelay(5f));
    }

    private IEnumerator ExitGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
