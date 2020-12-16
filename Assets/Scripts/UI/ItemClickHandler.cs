using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// This class is used for equipping items to the player's hand.
public class ItemClickHandler : MonoBehaviour
{
    public Inventory inventory;
    [SerializeField] private new string name;

    // Creating a method which will equip the weapon to the player's hand. This will be invoked from the inventory.
    public void OnItemClicked()
    {
        // Finding the ItemHandler component of the object.
        ItemHandler handler = gameObject.transform.Find(name).GetComponent<ItemHandler>();

        InventoryItem item = handler.item;

        // Calling the inventory's UseItem-method, which activates a new event.
        inventory.UseItem(item);

        // Finally, call the item's OnUse method.
        item.OnUse();
    }
}
