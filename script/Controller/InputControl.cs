using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InputControl : MonoBehaviour
{
    bool canInput = true;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        CheckCursorstate();
    }

    public Vector3 GetMoveInput()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"),0f,Input.GetAxis("Vertical"));
        move = Vector3.ClampMagnitude(move,1);
        return move;
    }

    public bool GetSprintInput()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    public bool GetJumpInputDown() => Input.GetKeyDown(KeyCode.Space);

    public float GetMouseXAxis()
    {
        if(CanProcessInput())
        {
            return Input.GetAxis("Mouse X");
        }
        return 0;
    }

    public float GetMouseYAxis()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse Y");
        }
        return 0;
    }

    public float GetMouseScrollAxis()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse ScrollWheel");
        }
        return 0;
    }

    //取得是否按下滑鼠左鍵
    public bool GetFireInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButtonDown(0);
        }
        return false;
    }

    //取得是否持續按下滑鼠左鍵
    public bool GetFireInputHold()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButton(0);
        }
        return false;
    }

    //取得是否放開滑鼠左鍵
    public bool GetfireInputUp()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButtonUp(0);
        }
        return false;
    }

    //取得是否按下滑鼠右鍵
    public bool GetAimInputDown()
    {
        if(CanProcessInput())
        {
            return Input.GetMouseButtonDown(1);
        }
        return false;
    }

    //取得是否按下換彈鍵
    public bool GetReloadInputDown()
    {
        if(CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.R);
        }
        return false;
    }

    //取得是否按下切換武器
    public int GetSwitchWeaponInput()
    {
        if(CanProcessInput())
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                return -1;
            }
            else if(Input.GetKeyDown(KeyCode.E))
            {
                return 1;
            }
        }
        return 0;
    }

    private void CheckCursorstate()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
    public bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked && canInput;
    }
}
