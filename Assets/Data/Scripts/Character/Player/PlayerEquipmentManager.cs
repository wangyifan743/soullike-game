using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager playerManager;

    [SerializeField] WeaponManager rightWeaponManager;
    [SerializeField] WeaponManager leftWeaponManager;

    public WeaponModelInstantiationSlot rightHandSlot;
    public WeaponModelInstantiationSlot leftHandSlot;

    public GameObject rightHandWeaponModel;
    public GameObject leftHandWeaponModel;

    protected override void Awake()
    {
        base.Awake();
        playerManager = GetComponent<PlayerManager>();
        IntializeWeaponSlots();

    }

    protected override void Start()
    {
        base.Start();
        LoadWeaponOnBothHands();
    }

    private void IntializeWeaponSlots(){
        WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

        foreach (var weaponSlot in weaponSlots)
        {
            if(weaponSlot.weaponModelSlot == WeaponModelSlot.RightHand){
                rightHandSlot = weaponSlot;
            }
            else if(weaponSlot.weaponModelSlot == WeaponModelSlot.LeftHand){
                leftHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponOnBothHands(){
        LoadWeaponOnLeftHand();
        LoadWeaponOnRightHand();
    }

    // left weapon

    public void LoadWeaponOnLeftHand(){
        if(playerManager.playerInventoryManager.currentLeftHandWeapon != null){
            leftHandSlot.UnloadWeapon();

            leftHandWeaponModel = Instantiate(playerManager.playerInventoryManager.currentLeftHandWeapon.weaponModel);
            leftHandSlot.LoadWeapon(leftHandWeaponModel);
            leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            leftWeaponManager.SetWeaponDamage(playerManager ,playerManager.playerInventoryManager.currentLeftHandWeapon);
        }
    }

    public void SwitchLeftWeapon(){
        if(!playerManager.IsOwner)
            return;

        // Swap_Left_Weapon_01
        playerManager.playerAnimatorManager.PlayTargetActionAnimation("Swap_Left_Weapon_01",false,false,true,true);
        

        WeaponItem selectedWeapon = null;

        playerManager.playerInventoryManager.leftHandWeaponIndex += 1;

        if(playerManager.playerInventoryManager.leftHandWeaponIndex < 0 || playerManager.playerInventoryManager.leftHandWeaponIndex > 2){
            playerManager.playerInventoryManager.leftHandWeaponIndex = 0;
            
            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < playerManager.playerInventoryManager.weaponsInLeftHandSlots.Length; i++)
            {
                if(playerManager.playerInventoryManager.weaponsInLeftHandSlots[i].ItemID != WorldItemDatabase.instance.unarmedWeapon.ItemID){
                    weaponCount +=1;

                    if(firstWeapon == null){
                        firstWeapon = playerManager.playerInventoryManager.weaponsInLeftHandSlots[i];
                        firstWeaponPosition = i;
                    }
                }
            }

            if(weaponCount <= 1){
                playerManager.playerInventoryManager.leftHandWeaponIndex = -1;
                selectedWeapon = Instantiate(WorldItemDatabase.instance.unarmedWeapon);
                playerManager.playerNetworkManager.currentLeftHandWeaponID.Value = selectedWeapon.ItemID;
            }
            else{
                playerManager.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                playerManager.playerNetworkManager.currentLeftHandWeaponID.Value = firstWeapon.ItemID;
            }

            return;

        }

        

        foreach (WeaponItem weapon in playerManager.playerInventoryManager.weaponsInLeftHandSlots)
        {
            if(playerManager.playerInventoryManager.weaponsInLeftHandSlots[playerManager.playerInventoryManager.leftHandWeaponIndex].ItemID != WorldItemDatabase.instance.unarmedWeapon.ItemID){
                selectedWeapon = playerManager.playerInventoryManager.weaponsInLeftHandSlots[playerManager.playerInventoryManager.leftHandWeaponIndex];

                playerManager.playerNetworkManager.currentLeftHandWeaponID.Value = playerManager.playerInventoryManager.weaponsInLeftHandSlots[playerManager.playerInventoryManager.leftHandWeaponIndex].ItemID;

       

                return;

            }
        }

        if(selectedWeapon == null && playerManager.playerInventoryManager.leftHandWeaponIndex <= 2){
            SwitchLeftWeapon();
          
        }
        


    }



    // right weapon
    public void LoadWeaponOnRightHand(){
        if(playerManager.playerInventoryManager.currentRightHandWeapon != null){
            rightHandSlot.UnloadWeapon();

            rightHandWeaponModel = Instantiate(playerManager.playerInventoryManager.currentRightHandWeapon.weaponModel);
            rightHandSlot.LoadWeapon(rightHandWeaponModel);
            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            rightWeaponManager.SetWeaponDamage(playerManager, playerManager.playerInventoryManager.currentRightHandWeapon);
        }
    }

    public void SwitchRightWeapon(){
        if(!playerManager.IsOwner)
            return;

        // Swap_Right_Weapon_01
        playerManager.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01",false,false,true,true);
        

        WeaponItem selectedWeapon = null;

        playerManager.playerInventoryManager.rightHandWeaponIndex += 1;

        if(playerManager.playerInventoryManager.rightHandWeaponIndex < 0 || playerManager.playerInventoryManager.rightHandWeaponIndex > 2){
            playerManager.playerInventoryManager.rightHandWeaponIndex = 0;
            
            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < playerManager.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
            {
                if(playerManager.playerInventoryManager.weaponsInRightHandSlots[i].ItemID != WorldItemDatabase.instance.unarmedWeapon.ItemID){
                    weaponCount +=1;

                    if(firstWeapon == null){
                        firstWeapon = playerManager.playerInventoryManager.weaponsInRightHandSlots[i];
                        firstWeaponPosition = i;
                    }
                }
            }

            if(weaponCount <= 1){
                playerManager.playerInventoryManager.rightHandWeaponIndex = -1;
                selectedWeapon = Instantiate(WorldItemDatabase.instance.unarmedWeapon);
                playerManager.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.ItemID;
            }
            else{
                playerManager.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                playerManager.playerNetworkManager.currentRightHandWeaponID.Value = firstWeapon.ItemID;
            }

            return;

        }

        

        foreach (WeaponItem weapon in playerManager.playerInventoryManager.weaponsInRightHandSlots)
        {
            if(playerManager.playerInventoryManager.weaponsInRightHandSlots[playerManager.playerInventoryManager.rightHandWeaponIndex].ItemID != WorldItemDatabase.instance.unarmedWeapon.ItemID){
                selectedWeapon = playerManager.playerInventoryManager.weaponsInRightHandSlots[playerManager.playerInventoryManager.rightHandWeaponIndex];

                playerManager.playerNetworkManager.currentRightHandWeaponID.Value = playerManager.playerInventoryManager.weaponsInRightHandSlots[playerManager.playerInventoryManager.rightHandWeaponIndex].ItemID;

       

                return;

            }
        }

        if(selectedWeapon == null && playerManager.playerInventoryManager.rightHandWeaponIndex <= 2){
            SwitchRightWeapon();
          
        }
    }


    public void OpenDamageCollider(){
        if(playerManager.playerNetworkManager.isUsingRightHand.Value){
            rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
        }
        if(playerManager.playerNetworkManager.isUsingLeftHand.Value){
            leftWeaponManager.meleeDamageCollider.EnableDamageCollider();
        }
    }

    public void CloseDamageCollider(){
        if(playerManager.playerNetworkManager.isUsingRightHand.Value){
            rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
        }
        if(playerManager.playerNetworkManager.isUsingLeftHand.Value){
            leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
        }
    }
}
