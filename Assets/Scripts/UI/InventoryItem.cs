using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creating an interface, so the following values and methods must be implemented.
public interface InventoryItem
{

    string Name { get;  }
    int GetPrice { get;  }
    string GetDamage { get;  }
    float RateOfFire { get;  }
    float GetRange { get;  }
    int AmmoPrice { get;  }
    int UpgradePrice { get; }
    int AmmoPurchaseAmount { get; }
    Sprite Image { get; }

    void onPickup();

    void OnUse();

    void UnEquip();
}

// Creating an event system for adding and using inventoryitems. Other inventory items will get notified when an item is used. 
public class InventoryEventArgs : EventArgs
{
    public InventoryEventArgs(InventoryItem item)
    {
        Item = item;
    }

    public InventoryItem Item;
}
