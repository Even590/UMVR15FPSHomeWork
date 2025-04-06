using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerHealth : MonoBehaviour
{
    [Header("最大血量")]
    [SerializeField] float MaxHealth = 10f;
    [Header("當前血量")]
    [SerializeField] float CurrentHealth;

    //受到攻擊時要觸發的委派事件
    public event Action PlayerOnDamage;
    //受傷時的音效播放
    public AudioClip GetHit;
    private AudioSource audioSource;
    //人物死亡時要觸發的委派事件
    public event Action PlayerOnDead;

    private bool Isdead = false;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    public float GetCurrentHealth()
    {
        return CurrentHealth;
    }

    public float GetMaxHealth()
    {
        return MaxHealth;
    }

    public float GetHealthRatio()
    {
        return CurrentHealth / MaxHealth;
    }

    public bool IsDead()
    {
        return Isdead;
    }

    public void TakeDamage(float damage)
    {
        if (Isdead) return;

        if (gameObject.tag == "Player")
        {
            print("Player的血量剩餘：" + CurrentHealth);
        }

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Max(CurrentHealth, 0);
        audioSource.PlayOneShot(GetHit);

        if (CurrentHealth > 0)
        {
            PlayerOnDamage?.Invoke();
        }

        if (CurrentHealth <= 0)
        {
            HeadleDeath();
        }
    }

    private void HeadleDeath()
    {
        if (Isdead) return;

        if (CurrentHealth <= 0)
        {
            Isdead = true;
            PlayerOnDead?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        CurrentHealth += amount;
        CurrentHealth = Mathf.Min(CurrentHealth, MaxHealth);


    }
}
