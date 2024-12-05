using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemies : MonoBehaviour
{
    [SerializeField]
    private float damageAmount;
    private HealthScript playerHealthController;
    public void Start()
    {
        playerHealthController = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthScript>();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<SlimeMovement>())
        {
            var healthController = collision.gameObject.GetComponent<HealthScript>();
            healthController.TakeDamage(damageAmount);
        }
    }
    public void Attack()
    {
        playerHealthController.TakeDamage(damageAmount);
    }
}
