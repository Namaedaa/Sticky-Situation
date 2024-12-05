using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInvincibilityScript : MonoBehaviour
{
    [SerializeField]
    private float invincibilityDuration;

    private InvincibilityScript invincibilityController;

    private void Awake()
    {
        invincibilityController = GetComponent<InvincibilityScript>();
    }

    public void StartInvincibility()
    {
        invincibilityController.StartInvincibility(invincibilityDuration);
    }
}
