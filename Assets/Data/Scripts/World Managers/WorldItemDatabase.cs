using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldItemDatabase : MonoBehaviour
{
    public static WorldItemDatabase instance;

    public WeaponItem unarmedWeapon;

    [Header("Weapons")]
    [SerializeField] List<WeaponItem> weaponItems = new List<WeaponItem>();

    [Header("Items")]
    [SerializeField] List<Item> items = new List<Item>();

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }

        foreach (WeaponItem weapon in weaponItems)
        {
            items.Add(weapon);
        }

        for (int i = 0; i < items.Count; i++)
        {
            items[i].ItemID = i;
        }
        
    }

    public WeaponItem GetWeaponByID(int ID){
        return weaponItems.FirstOrDefault(weapon => weapon.ItemID == ID);
    }
}
