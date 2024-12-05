using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthScript : MonoBehaviour
{

    [SerializeField]
    internal float currentHealth;

    [SerializeField]
    internal float maximumHealth;

    [SerializeField]
    private float lowHealthLimit;

    public float RemainingHealthPercentage
    {
        get
        {
            return currentHealth / maximumHealth;
        }
    }

    public bool IsInvincible { get; set; }
    public bool IsLowHealth { get; set; }

    public UnityEvent OnDied;

    public UnityEvent OnDamaged;

    public UnityEvent OnHealthChanged;

    public UnityEvent OnHealthBelowPercentage;

    public void Update()
    {
       
    }

    public void CheckIfLowPercentage()
    {
        if (RemainingHealthPercentage <= lowHealthLimit)
        {
            OnHealthBelowPercentage.Invoke();
        }
       
    }
    public void TakeDamage(float damageAmount)
    {
        if (currentHealth == 0)
        {
            return;
        }

        if (IsInvincible)
        {
            return;
        }

        currentHealth -= damageAmount;
        OnHealthChanged.Invoke();

        CheckIfLowPercentage();
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
       
        if (currentHealth == 0)
        {
            OnDied.Invoke();
        }
        else
        {
            OnDamaged.Invoke();
        }
    }

    public void AddHealth(float amountToAdd)
    {
        if (currentHealth == maximumHealth)
        {
            return;
        }

        currentHealth += amountToAdd;
        OnHealthChanged.Invoke();
        CheckIfLowPercentage();
        if (currentHealth > maximumHealth)
        {
            currentHealth = maximumHealth;
        }
    }
}
