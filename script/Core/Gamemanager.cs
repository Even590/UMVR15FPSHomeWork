using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private InputControl m_Inputcontrol;

    public InputControl inputControl
    {
        get
        {
            if (m_Inputcontrol == null)
            {
                m_Inputcontrol = GetComponent<InputControl>();
            }
            return m_Inputcontrol;
        }
    }

    private void Awake()
    {
        // 確保 InputControl 存在
        if (GetComponent<InputControl>() == null)
        {
            gameObject.AddComponent<InputControl>();
        }
    }
}
