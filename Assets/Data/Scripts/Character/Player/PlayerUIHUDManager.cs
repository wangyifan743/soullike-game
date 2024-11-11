using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHUDManager : MonoBehaviour
{
    [SerializeField]UI_StatBar staminaBar;
    [SerializeField]UI_StatBar healthBar;

    public void RefreshHUD(){
        healthBar.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(true);

        staminaBar.gameObject.SetActive(false);
        staminaBar.gameObject.SetActive(true);
    }
    public void SetCurrentStaminaValue(float currentStamina){
        staminaBar.SetSta(currentStamina);
    }
    public void SetCurrentHealthValue(float currentHealth){
        healthBar.SetSta(currentHealth);
    }

    public void SetNewStaminaValue(float oldValue, float newValue){
        staminaBar.SetSta(newValue);
    }

    public void SetMaxStaminaValue(float maxValue){
        staminaBar.SetMaxStat(maxValue);
    }

    public void SetNewHealthValue(float oldValue, float newValue){
        healthBar.SetSta(newValue);
    }

    public void SetMaxHealthValue(float maxValue){
        healthBar.SetMaxStat(maxValue);
    }


}
