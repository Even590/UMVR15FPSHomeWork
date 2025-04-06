using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{

    [SerializeField] Image HealthImage;
    PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        HealthImage.fillAmount = Mathf.Lerp(HealthImage.fillAmount, playerHealth.GetHealthRatio(), 0.1f);
    }
}
