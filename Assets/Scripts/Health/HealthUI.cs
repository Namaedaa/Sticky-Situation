using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image healthBarForegroundImage;

    public void UpdateHealthBar(HealthScript healthController)
    {
        healthBarForegroundImage.fillAmount = healthController.RemainingHealthPercentage;
    }
}
