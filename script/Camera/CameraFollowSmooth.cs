using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSmooth : MonoBehaviour
{
    [Tooltip("跟隨的Player目標")]
    [SerializeField] Transform Player;
    [Tooltip("跟目標的最大距離")]
    [SerializeField] float DistanceToTarget;
    [Tooltip("起始高度")]
    [SerializeField] float StartHight;
    [Tooltip("平滑的移動時間")]
    [SerializeField] float SmoothTime;
    [Tooltip("滾輪軸靈敏度")]
    [SerializeField] float SensitivityOffset_z;
    [Tooltip("最小垂直軸Y的Offset")]
    [SerializeField] float MinOffset_Y;
    [Tooltip("最大垂直軸Y的Offset")]
    [SerializeField] float MaxOffset_Y;
    [Tooltip("瞄準模式下攝影機的偏移量")]
    [SerializeField] Vector3 AimCamera;
    [Tooltip("瞄準模式下攝影機的拉近距離")]
    [SerializeField] float AimDistance;

    float Offset_Y;
    bool IsAim;

    InputControl input;
    Vector3 SmoothPosition = Vector3.zero;
    Vector3 CurrentVelocity = Vector3.zero;

    private void Awake()
    {
        // input = GameManagerSingleton.Instance.inputControl;
        transform.position = Player.position + Vector3.up * StartHight;
        Offset_Y = StartHight;

        Player.GetComponent<Playercontrol>().OnAim += AimHandle;
    }

    private void LateUpdate()
    {
        if (input.GetMouseScrollAxis() != 0)
        {
            Offset_Y -= input.GetMouseScrollAxis() * SensitivityOffset_z;
            Offset_Y = Mathf.Clamp(Offset_Y, MinOffset_Y, MaxOffset_Y);
            Vector3 OffsetTarget = Player.position + Player.up * Offset_Y;
            transform.position = Vector3.Lerp(transform.position, OffsetTarget, SmoothTime);
        }
        if (CheckDistance())
        {
            SmoothPosition = Vector3.SmoothDamp(transform.position, Player.position + Vector3.up * Offset_Y, ref CurrentVelocity, SmoothTime);
            transform.position = SmoothPosition;
        }
    }

    

    //檢查與目標的距離
    private bool CheckDistance()
    {
        return Vector3.Distance(transform.position, Player.position) > DistanceToTarget;
    }

    private void AimHandle(bool aim)
    {
        IsAim = aim;
    }
}
