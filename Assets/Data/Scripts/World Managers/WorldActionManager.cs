using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldActionManager : MonoBehaviour
{
    [Header("Weapon Item Actions")]
    public WeaponItemAction[] weaponItemActions;
    public static WorldActionManager instance;

    private void Awake() {
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        for (int i = 0; i < weaponItemActions.Length; i++)
        {
            weaponItemActions[i].actionID = i;
        }
    }

    public WeaponItemAction GetWeaponItemActionByID(int ID){
        return weaponItemActions.FirstOrDefault(weaponItemAction => weaponItemAction.actionID == ID);
    }
}
