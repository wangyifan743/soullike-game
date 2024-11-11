using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [Header("Item Information")]
    public int ItemID;
    public string itemName;
    public Sprite itemIcon;
    [TextArea] public string itemDescription;
}
