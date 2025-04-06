using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    [Header("最大血量")]
    [SerializeField] float MaxHealth = 10f;
    [Header("當前血量")]
    [SerializeField] float CurrentHealth;

    //受到攻擊時要觸發的委派事件
    public event Action OnDamage;
    //人物死亡時要觸發的委派事件
    public event Action OnDead;

    private bool Isdead = false;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
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
        if(Isdead) return;

        if(gameObject.tag == "Player")
        {
            print("Player的血量剩餘：" + CurrentHealth);
        }

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Max(CurrentHealth,0);

        if(CurrentHealth > 0)
        {
            OnDamage?.Invoke();
        }

        if(CurrentHealth <= 0)
        {
            HeadleDeath();
        }
    }

    private void HeadleDeath()
    {
        if(Isdead) return;
        
        if(CurrentHealth <= 0)
        {
            Isdead = true;
            OnDead?.Invoke();
        }
    }
}
