using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private GameObject rootCanvas;
    [SerializeField] private Image foreGround;
    [Range(0,1)]
    [SerializeField] float ChangeHealthRatio = 0.05f;

    void Update()
    {
        if(Mathf.Approximately(health.GetHealthRatio(),0) || Mathf.Approximately(health.GetHealthRatio(), 1))
        {
            rootCanvas.SetActive(false);
            return;
        }

        rootCanvas.SetActive(true);
        rootCanvas.transform.LookAt(Camera.main.transform.position);
        foreGround.fillAmount = Mathf.Lerp(foreGround.fillAmount,health.GetHealthRatio(),ChangeHealthRatio);

    }
}
