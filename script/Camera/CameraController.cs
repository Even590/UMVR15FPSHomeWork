using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform CameraRotation;

    private float Mouse_X;
    private float Mouse_Y;
    public float MouseSensitivity = 500;
    public float xRotation;
    private bool IsPause = false;

    private bool isJumping = false;
    private bool isDeadcamera = false;
    [SerializeField] float jumpOffset = 0.5f;
    [SerializeField] float defaultHeight = 1.7f;
    [Header("PlayerTarget")]
    [SerializeField] GameObject player;
    [Header("受傷時的特效")]
    [SerializeField] ParticleSystem GetHitParticle;
    [Header("Pause")]
    [SerializeField] GameObject PauseUI;


    private Vector3 OriginalCameraPosition;
    private float shakeAmount = 0f;

    private FPScontrol fPScontrol;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if(PauseUI != null)
        {
            PauseUI.SetActive(false);
        }

        player.GetComponent<PlayerHealth>().PlayerOnDamage += OnDamage;
        fPScontrol = GetComponentInParent<FPScontrol>();
        if(fPScontrol != null)
        {
            fPScontrol.JumpEvent += OnJumpEvent;
            fPScontrol.Deadcamera += OnDeadCamera;
        }

        OriginalCameraPosition = transform.localPosition;
    }

    private void OnJumpEvent(bool isJumping)
    {
        this.isJumping = isJumping;
    }

    private void OnDeadCamera(bool isDeadcamera)
    {
        this.isDeadcamera = isDeadcamera;
        Transform weapon = transform.Find("Weapon");
        if (weapon != null)
        {
            Destroy(weapon.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

        Mouse_X = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        Mouse_Y = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;
        xRotation -= Mouse_Y;
        xRotation = Mathf.Clamp(xRotation,-90f,90f);
        CameraRotation.Rotate(Vector3.up * Mouse_X);
        this.transform.localRotation = Quaternion.Euler(xRotation,0,0);

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!IsPause)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

        if(isJumping)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, defaultHeight + jumpOffset, transform.localPosition.z);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, defaultHeight, transform.localPosition.z);
        }

        if(isDeadcamera)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 0.2f, transform.localPosition.z);
        }
        else
        {
            bool isMoving = Mathf.Abs(fPScontrol.currentSpeed) >= 0.1f;
            if(isMoving)
            {
                float speedFactor = Mathf.Clamp(fPScontrol.currentSpeed / 6f, 0f, 1f);
                shakeAmount = Mathf.Lerp(shakeAmount, Mathf.PerlinNoise(Time.time * 2f, 0f) * 0.1f, Time.deltaTime * 5f);
                transform.localPosition = OriginalCameraPosition + new Vector3(shakeAmount * speedFactor, 0f, 0f);
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, OriginalCameraPosition, Time.deltaTime * 2f);
            }
        }
    }

    private void Resume()
    {
        PauseUI.SetActive(false);
        Time.timeScale = 1;
        IsPause = false;
    }

    private void Pause()
    {
        PauseUI.SetActive(true);
        Time.timeScale = 0;
        IsPause = true;
    }

    private void OnDamage()
    {
        if(GetHitParticle != null)
        {
            GetHitParticle.Play();
        }
    }
}
