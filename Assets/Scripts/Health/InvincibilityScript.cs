using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityScript : MonoBehaviour
{
    private HealthScript healthController;

    private void Awake()
    {
        healthController = GetComponent<HealthScript>();
    }

    public void StartInvincibility(float invincibilityDuration)
    {
        StartCoroutine(InvincibilityCoroutine(invincibilityDuration));
    }

    private IEnumerator InvincibilityCoroutine(float invincibilityDuration)
    {
        healthController.IsInvincible = true;
        
        yield return new WaitForSeconds(invincibilityDuration);
       
        healthController.IsInvincible = false;
    }
}
