using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject weaponStatus;
    [SerializeField] private Inventory inventory;

    private HealthBar healthBar;
    private StaminaBar staminaBar;
    private GameObject consumables;
    private Text ethanol;
    private Text epinephrine;

    void Start()
    {
        // Finding a few components
        healthBar = GameObject.Find("Player").transform.GetChild(0).GetComponent<HealthBar>();
        staminaBar = GameObject.Find("Player").transform.GetChild(1).GetComponent<StaminaBar>();
        consumables = GameObject.Find("Consumables");

        ethanol = consumables.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        epinephrine = consumables.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();

        weaponStatus.SetActive(false);
        inventory.itemAdded += InventoryScriptItemAdded;
    }

    void Update()
    {
        // Updating the amount of ethanol injections and epinephrine the player has.
        ethanol.text = ThirdPersonMovement.ethanolInjections.ToString();
        epinephrine.text = ThirdPersonMovement.epinephrine.ToString();

        // Updating the amount of health and stamina the UI displays.
        healthBar.SetHealth(ThirdPersonMovement.currentHealth);       
        staminaBar.SetStamina(ThirdPersonMovement.stamina);
    }

    // Creating a method, which updates the inventory slots when items are added.
    private void InventoryScriptItemAdded(object sender, InventoryEventArgs e)
    {
        Transform inventoryPanel = transform.Find("Inventory");

        // Looping through the inventory and searching for a free slot.
        foreach(Transform slot in inventoryPanel)
        {
            // Getting the image and reference information to the item.
            Transform imageTransform = slot.GetChild(0).GetChild(0);
            Image image = imageTransform.GetComponent<Image>();
            ItemHandler handler = imageTransform.GetComponent<ItemHandler>();          

            // If the slot is free,
            if (!image.enabled)
            {
                // assign the item in it and store a reference to the item.
                image.enabled = true;
                image.sprite = e.Item.Image;
                handler.item = e.Item;
                print("Stored reference to the item.");
                break;
            }
        }
    }
}
