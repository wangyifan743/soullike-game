using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHUDManager : MonoBehaviour
{
    [Header("STAT BARS")]
    [SerializeField]UI_StatBar staminaBar;
    [SerializeField]UI_StatBar healthBar;

    [Header("Quick Slots")]
    [SerializeField] Image rightWeaponQuickSlotIcon;
    [SerializeField] Image leftWeaponQuickSlotIcon;

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

    public void SetRightWeaponQuickSlotIcon(int weaponID){
        WeaponItem weapon = WorldItemDatabase.instance.GetWeaponByID(weaponID);
        if(weapon == null){
            Debug.Log("没有武器");
            rightWeaponQuickSlotIcon.enabled = false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;
        }

        if(weapon.itemIcon == null){
            Debug.Log("没有武器图标");
            rightWeaponQuickSlotIcon.enabled = false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;
        }

        rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        rightWeaponQuickSlotIcon.enabled = true;
        
    }

    public void SetLeftWeaponQuickSlotIcon(int weaponID){
        WeaponItem weapon = WorldItemDatabase.instance.GetWeaponByID(weaponID);
        if(weapon == null){
            Debug.Log("没有武器");
            leftWeaponQuickSlotIcon.enabled = false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;
        }

        if(weapon.itemIcon == null){
            Debug.Log("没有武器图标");
            leftWeaponQuickSlotIcon.enabled = false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;
        }

        leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        leftWeaponQuickSlotIcon.enabled = true;
        
    }

}
