using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("攝影機跟隨的目標")]
    [SerializeField] Transform target;
    [Header("水平靈敏度")]
    [SerializeField] float sensitivity_x = 2;
    [Header("垂直靈敏度")]
    [SerializeField] float sensitivity_y = 2;
    [Header("滾輪靈敏度")]
    [SerializeField] float sensitivity_z = 5;
    [Header("offset")]
    [SerializeField] Vector3 offset;

    float MinVerticalAngle = -10;
    float MaxVerticalAngle = 85;
    float CameraToTargetDistance = 2;
    float MinDistance = 2;
    float MaxDistance = 5;
    float Mouse_x = 0;
    float Mouse_y = 30;

    InputControl m_input;

    private void Awake()
    {
        // m_input = GameManagerSingleton.Instance.inputControl;
    }

    private void LateUpdate()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Mouse_x += m_input.GetMouseXAxis() * sensitivity_x;
            Mouse_y -= m_input.GetMouseYAxis() * sensitivity_y;

            Mouse_y = Math.Clamp(Mouse_y, MinVerticalAngle, MaxVerticalAngle);

            transform.rotation = Quaternion.Euler(Mouse_y, Mouse_x, 0);
            transform.position = Quaternion.Euler(Mouse_y, Mouse_x, 0) * new Vector3(0, 0, -CameraToTargetDistance) + target.position + Vector3.up * offset.y;
            CameraToTargetDistance -= m_input.GetMouseScrollAxis() * sensitivity_z;
            CameraToTargetDistance = Mathf.Clamp(CameraToTargetDistance, MinDistance, MaxDistance);
        }
    }
}


