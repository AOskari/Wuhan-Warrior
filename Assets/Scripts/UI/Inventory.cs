using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    // Identifying all 9 inventory slots.
    [SerializeField] private Image slot1;
    [SerializeField] private Image slot2;
    [SerializeField] private Image slot3;
    [SerializeField] private Image slot4;
    [SerializeField] private Image slot5;
    [SerializeField] private Image slot6;
    [SerializeField] private Image slot7;
    [SerializeField] private Image slot8;
    [SerializeField] private Image slot9;

    [SerializeField] private InventoryItemBase startWeapon;

    private int Slots = 9;

    // Creating a list of items which will store the information of the inventory items.
    private List<InventoryItem> mItems = new List<InventoryItem>();

    // Creating two events, which are used for adding and equipping items to the player's hand. These 
    public event EventHandler<InventoryEventArgs> itemAdded;
    public event EventHandler<InventoryEventArgs> itemUsed;

    void Start()
    {
        // Giving the player a start weapon.
        AddItem(startWeapon);
        DeactivateSlot();
        slot1.enabled = true;
        GameObject.Find("Border1").GetComponent<Button>().onClick.Invoke();
    }

    // Creating a bunch of else if statements, which enables the player to select an inventory slot item using 1-9 keys, if the slot has an item in it.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DeactivateSlot();
            slot1.enabled = true;
            if (GameObject.Find("ItemImage1").GetComponent<ItemHandler>().item != null)
            {
                GameObject.Find("Border1").GetComponent<Button>().onClick.Invoke();
            }        
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DeactivateSlot();
            slot2.enabled = true;
            if (GameObject.Find("ItemImage2").GetComponent<ItemHandler>().item != null)
            {
                GameObject.Find("Border2").GetComponent<Button>().onClick.Invoke();
            }   
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            DeactivateSlot();
            slot3.enabled = true;
            if (GameObject.Find("ItemImage3").GetComponent<ItemHandler>().item != null)
            {
                GameObject.Find("Border3").GetComponent<Button>().onClick.Invoke();
            }         
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            DeactivateSlot();
            slot4.enabled = true;
            if (GameObject.Find("ItemImage4").GetComponent<ItemHandler>().item != null)
            {
                GameObject.Find("Border4").GetComponent<Button>().onClick.Invoke();
            }
          
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            DeactivateSlot();
            slot5.enabled = true;
            if (GameObject.Find("ItemImage5").GetComponent<ItemHandler>().item != null)
            {
                GameObject.Find("Border5").GetComponent<Button>().onClick.Invoke();
            }
 
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            DeactivateSlot();
            slot6.enabled = true;
            if (GameObject.Find("ItemImage6").GetComponent<ItemHandler>().item != null)
            {
                GameObject.Find("Border6").GetComponent<Button>().onClick.Invoke();
            }     
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            DeactivateSlot();
            slot7.enabled = true;
            if (GameObject.Find("ItemImage7").GetComponent<ItemHandler>().item != null)
            {
                GameObject.Find("Border7").GetComponent<Button>().onClick.Invoke();
            }           
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            DeactivateSlot();
            slot8.enabled = true;
            if (GameObject.Find("ItemImage8").GetComponent<ItemHandler>().item != null)
            {
                GameObject.Find("Border8").GetComponent<Button>().onClick.Invoke();
            }    
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            DeactivateSlot();
            slot9.enabled = true;
            if (GameObject.Find("ItemImage9").GetComponent<ItemHandler>().item != null)
            {
                GameObject.Find("Border9").GetComponent<Button>().onClick.Invoke();
            }          
        }
    }

    // Creating a method which adds inventory items to the player's inventory.
    public void AddItem(InventoryItem item)
    {
            // Checking if the inventory has space.
            if (mItems.Count < Slots)
            {
                // Getting the items collider,
                Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
                if (collider.enabled)
                {
                    // and disabling it.
                    collider.enabled = false;

                    // Adding the item to the inventory.
                    mItems.Add(item);
                    print("Item added.");

                    // Calling the item's onPickup method, which essentially makes the item inactive, making it disappear from the game.
                    item.onPickup();

                    if (itemAdded != null)
                    {
                        // Finally add the item to the event system.
                        itemAdded(this, new InventoryEventArgs(item));
                    }

                }
            }      
    }

    // Method which disables the blue border around the inventory slots and unequips the current weapon.
    public void DeactivateSlot()
    {
        if (GameObject.Find("ItemImage1").GetComponent<ItemHandler>().item != null)
        {
            GameObject.Find("ItemImage1").GetComponent<ItemHandler>().item.UnEquip();
        }
        
        if (GameObject.Find("ItemImage2").GetComponent<ItemHandler>().item != null)
        {
            GameObject.Find("ItemImage2").GetComponent<ItemHandler>().item.UnEquip();
        }

        if (GameObject.Find("ItemImage3").GetComponent<ItemHandler>().item != null)
        {
            GameObject.Find("ItemImage3").GetComponent<ItemHandler>().item.UnEquip();
        }

        if (GameObject.Find("ItemImage4").GetComponent<ItemHandler>().item != null)
        {
            GameObject.Find("ItemImage4").GetComponent<ItemHandler>().item.UnEquip();
        }

        if (GameObject.Find("ItemImage5").GetComponent<ItemHandler>().item != null)
        {
            GameObject.Find("ItemImage5").GetComponent<ItemHandler>().item.UnEquip();
        }

        if (GameObject.Find("ItemImage6").GetComponent<ItemHandler>().item != null)
        {
            GameObject.Find("ItemImage6").GetComponent<ItemHandler>().item.UnEquip();
        }

        if (GameObject.Find("ItemImage7").GetComponent<ItemHandler>().item != null)
        {
            GameObject.Find("ItemImage7").GetComponent<ItemHandler>().item.UnEquip();
        }

        if (GameObject.Find("ItemImage8").GetComponent<ItemHandler>().item != null)
        {
            GameObject.Find("ItemImage8").GetComponent<ItemHandler>().item.UnEquip();
        }

        if (GameObject.Find("ItemImage9").GetComponent<ItemHandler>().item != null)
        {
            GameObject.Find("ItemImage9").GetComponent<ItemHandler>().item.UnEquip();
        }

        slot1.enabled = false;
        slot2.enabled = false;
        slot3.enabled = false;
        slot4.enabled = false;
        slot5.enabled = false;
        slot6.enabled = false;
        slot7.enabled = false;
        slot8.enabled = false;
        slot9.enabled = false;
    }

    // This method creates a new inventory event, which equips the item.
    public void UseItem(InventoryItem item)
    {
        if (itemUsed != null)
        {
            itemUsed(this, new InventoryEventArgs(item));
        }
    }
}


// Some of the code blocks where used from a YouTube inventory tutorial:
// Video by Jayanam: https://www.youtube.com/watch?v=Hj7AZkyojdo&t=193s&ab_channel=Jayanam