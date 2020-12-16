using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Creating a base class for all items that are stored in the inventory.
// This class uses the InventoryItem-class, which stores the needed information for storing the item to the player's inventory.
// Methods which are universal between the weapons are implemented in this class. Other required methods are implemented in their own scripts, as they differ from one another.
public class InventoryItemBase : MonoBehaviour, InventoryItem
{

    public GameObject weaponStatus;
    public Text ammoDisplay;
    public Text weaponName;
    public Text damageDisplay; 
    
    public GameObject crosshair;
    public GameObject raycastObject;
    public GameObject weaponDesc;
    public GameObject player;

    public GameObject weaponInfo;
    public Vector3 Position;
    public Vector3 Rotation;

    public int range;
    public float shootCooldown;
    public float damage;
    public Sprite image;

    public Sprite Image
    {
        get { return image; }
    }


    // Creating virtual functions, which can be overwritten when needed. These methods are used for getting wanted information.
    public virtual string Name
    {
        get
        {
            return "A Weapon";
        }
    }

    public virtual int GetPrice
    {
        get
        {
            return 0;
        }
    }

    public virtual float GetRange
    {
        get
        {
            return range;
        }
    }

    public virtual float RateOfFire
    {
        get
        {
            return 1f / shootCooldown;
        }
    }

    public virtual string GetDamage
    {
        get
        {
            return "damage";
        }
    }

    public virtual int AmmoPurchaseAmount
    {
        get
        {
            return 0;
        }
    }

    public virtual int UpgradePrice
    {
        get
        {
            return 0;
        }
    }

    public virtual int AmmoPrice
    {
        get
        {
            return 0;
        }
    }

    public virtual string AmmoAmount
    {
        get
        {
            return "";
        }
    }

    public virtual float UpgradeAmount
    {
        get
        {
            return 0f;
        }
    }

    // This method will be overwritten as needed. This is used for unequipping the weapon when changing weapons. 
    public virtual void UnEquip()
    {
    }

    // Deactivation method when the item is picked up.
    public virtual void onPickup()
    {
        gameObject.SetActive(false);
    }

    // This method will be overwritten as needed. This is used for equipping the weapon to the player's right hand.
    public virtual void OnUse()
    {
    }

    // A method which updates the Weapon description when standing near a weapon that can be bought.
    public virtual void UpdateAmmoText()
    {
        weaponDesc.transform.GetChild(0).GetComponent<Text>().text = Name;
        weaponDesc.transform.GetChild(1).GetComponent<Text>().text = "";
        weaponDesc.transform.GetChild(2).GetComponent<Text>().text = "+" + AmmoPurchaseAmount + " ammo";
        weaponDesc.transform.GetChild(3).GetComponent<Text>().text = "Price: " + AmmoPrice;
        weaponDesc.transform.GetChild(4).GetComponent<Text>().text = "";
        weaponDesc.transform.GetChild(5).GetComponent<Text>().text = "B Buy";
    }

    // A method which updates the weapon description when standing on near the workbench.
    public void UpdateUpgradeText()
    {
        weaponDesc.transform.GetChild(0).GetComponent<Text>().text = Name;
        weaponDesc.transform.GetChild(1).GetComponent<Text>().text = "Upgrade";
        weaponDesc.transform.GetChild(2).GetComponent<Text>().text = "+" + UpgradeAmount + " damage";
        weaponDesc.transform.GetChild(3).GetComponent<Text>().text = "Price: " + UpgradePrice;
        weaponDesc.transform.GetChild(4).GetComponent<Text>().text = "";
        weaponDesc.transform.GetChild(5).GetComponent<Text>().text = "B Upgrade";
    }

    // A method which updates the weapon description when the currently equipped weapon is fully upgraded.
    public void CantUpgradeText()
    {
        weaponDesc.transform.GetChild(0).GetComponent<Text>().text = Name;
        weaponDesc.transform.GetChild(1).GetComponent<Text>().text = "";
        weaponDesc.transform.GetChild(2).GetComponent<Text>().text = "This weapon is fully upgraded.";
        weaponDesc.transform.GetChild(3).GetComponent<Text>().text = "";
        weaponDesc.transform.GetChild(4).GetComponent<Text>().text = "";
        weaponDesc.transform.GetChild(5).GetComponent<Text>().text = "";
    }

    // This method is used on the pause menu. It displays more detailed information of the currently equipped weapon.
    public void ShowWeaponInfo()
    {
        weaponInfo.transform.GetChild(1).GetComponent<Text>().text = Name + 
            "\n \n Damage: " + GetDamage + "\n \n Range: " + GetRange + "\n \n Fire rate: " + RateOfFire + "\n \n Ammo: " + AmmoAmount;
    }

    // A Method which displays the weapon status of the currently equipped weapon.
    public void GetWeaponStatus()
    {
        // Finding the weaponstatus object from the hierarcy, and finding the text component of the children.
        weaponStatus = GameObject.Find("WeaponStatus");
        ammoDisplay = weaponStatus.transform.GetChild(0).GetComponent<Text>();
        weaponName = weaponStatus.transform.GetChild(1).GetComponent<Text>();
        damageDisplay = weaponStatus.transform.GetChild(4).GetComponent<Text>();

        // Updating the text components with the following values.
        ammoDisplay.text = AmmoAmount.ToString();
        weaponName.text = Name.ToString();
        damageDisplay.text = GetDamage;

        // Finally, displaying the weaponstatus object.
        weaponStatus.SetActive(true);
    }

    // A Method which disables the weapon status and resets the text.
    public void DisableWeaponStatus()
    {
        weaponStatus = GameObject.Find("WeaponStatus");
        weaponStatus.SetActive(false);
    }


    // Creating a function which enables the player shoot with the currently equipped weapon.
    public void Shoot()
    {
        // Creating 2 vectors which will be used to get the crosshair's exact position.
        Vector3 fromPosition = raycastObject.transform.position;
        Vector3 toPosition = crosshair.transform.position;

        // This vector3 gives the exact point of the crosshair.
        Vector3 direction = toPosition - fromPosition;

        RaycastHit hit;

        // Shooting a ray from the player towards the crosshair,
        if (Physics.Raycast(player.transform.position, direction, out hit, range))
        {
            // displaying a green line to demonstrate the ray,
            Debug.DrawRay(player.transform.position, direction, Color.green);
            print(hit.transform.name);

            if (hit.transform.gameObject.tag == "Enemy")
            {
                print("Hit an enemy");
                Enemy target = hit.transform.GetChild(0).GetComponent<Enemy>();
                // if the ray hits an enemy,
                if (target != null)
                {
                    // deal the weapon's damage to it and play the hitmarker sound.
                    target.TakeDamage(damage);

                    // If the hitmarker toggle is activated, play the the hitmarker sound and animation.
                    if (PauseMenu.hitmarkerSound)
                    {
                        GameObject.Find("ThirdPersonPlayer").GetComponent<ThirdPersonMovement>().PlayHitMarker();
                        GameObject.Find("Canvas").transform.GetChild(3).GetComponent<Animation>().Play();
                    }                 
                }
            }           
        }
    }
}


