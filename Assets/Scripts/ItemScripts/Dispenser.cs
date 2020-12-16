using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dispenser : InventoryItemBase
{

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioClip clip2;
    [SerializeField] private AudioClip upgrade;
    [SerializeField] private ParticleSystem muzzleFlash;

    private float audioCooldown = 0.4f;
    private float audioElapsedTime;
    private float elapsedTime = 0f;
    private float upgradeAmount = 0.2f;

    private int price = 200;
    private int maxUpgrade = 10;
    private int timesUpgraded = 0;
    private int upgradePrice = 50;
    private int paperCount;
    private int ammoPrice = 20;
    private int currentAmmo;
    private int ammoPurchaseAmount = 200;

    private bool isActive;
    private new string name = "The Sprinkler";
    private ThirdPersonMovement controller;
    private GameObject pickupText;

    // The following 2 float variables are used to create a fixed firerate for the weapon.


    void Start()
    {
        currentAmmo = 1000;
        audioElapsedTime = audioCooldown;
        // Finding the wanted gameobjects and components.
        pickupText = GameObject.Find("PickupText");
        player = GameObject.Find("ThirdPersonPlayer");
        controller = player.GetComponent<ThirdPersonMovement>();
    }

    public override int UpgradePrice
    {
        get
        {
            return upgradePrice;
        }
    }

    public override int AmmoPrice
    {
        get
        {
            return ammoPrice;
        }
    }

    // Overriding the InventoryItem-class Name method, which returns the item's name.
    public override string Name
    {
        get
        {
            return name;
        }
    }

    public override int GetPrice
    {
        get
        {
            return price;
        }
    }

    public override string GetDamage
    {
        get
        {
            return damage.ToString();
        }
    }

    public override int AmmoPurchaseAmount
    {
        get
        {
            return ammoPurchaseAmount;
        }
    }

    public override string AmmoAmount
    {
        get
        {
            return currentAmmo.ToString();
        }
    }

    public override float UpgradeAmount
    {
        get
        {
            return upgradeAmount;
        }
    }


    void Update()
    {
        // Checking if the game is not paused.
        if (ThirdPersonMovement.allowInput)
        {
            paperCount = PaperCount.count;
            // Increasing the elapsedTime value in every update call.
            elapsedTime += Time.deltaTime;
            audioElapsedTime += Time.deltaTime;

            if (isActive)
            {
                // Updating the weapon info in the pause menu and the weapon status above the player's inventory.
                ShowWeaponInfo();
                GetWeaponStatus();

                // If the player is pressing the left mouse button, the weapon is equipped, the player has ammo and the elapsedTime value is the same or higher than the firerate value,
                if (Input.GetKey(KeyCode.Mouse0) && currentAmmo > 0 && elapsedTime >= shootCooldown)
                {

                    // play the muzzleFlash particle-effect, call the shoot function and reset the elapsedTime to 0.
                    if (audioElapsedTime >= audioCooldown)
                    {
                        source.PlayOneShot(clip, 0.5f);
                        audioElapsedTime = 0f;
                    }
                    currentAmmo--;
                    muzzleFlash.Play();
                    Shoot();
                    raycastObject.GetComponent<Animator>().Play("DispenserShoot");
                    elapsedTime = 0f;

                }
                else if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    // return the weapon to its idle-state.
                    audioElapsedTime = audioCooldown;
                    source.Stop();
                    muzzleFlash.Stop();
                }

                // If the boolean value ammoBuy is true, give the player a possibility to buy ammo.
                if (ThirdPersonMovement.ammoBuy)
                {
                    UpdateAmmoText();

                    if (Input.GetKeyDown(KeyCode.B))
                    {
                        source.PlayOneShot(clip2, 0.5f);
                        buyAmmo();
                    }
                }
                // If the boolean value upgradeWeapon is true, give the player a possibility to upgrade the equipped weapon.
                if (ThirdPersonMovement.upgradeWeapon)
                {
                    // If the weapon is fully upgraded, display the cant upgrade text.
                    if (timesUpgraded >= maxUpgrade)
                    {
                        CantUpgradeText();
                    }
                    else
                    {
                        // Displaying the upgrade tex.
                        UpdateUpgradeText();

                        if (Input.GetKeyDown(KeyCode.B))
                        {
                            // If the B key is pressed, upgrade the weapon.
                            Upgrade();
                        }
                    }
                }
            }
        }
    }


    // Defining the weapons own equip-method
    public override void OnUse()
    {

        if (isActive)
        {
            /* If the weapon is active and the OnUse method is used,
             * disable the dispenser idle-animation ,set the weapon inactive and reset the weapon status text. 
             This function also controls the off-hand minigun. */
            isActive = false;
            gameObject.SetActive(false);
            weaponStatus.SetActive(false);
            weaponName.text = "";
            ammoDisplay.text = "";
            damageDisplay.text = "";

        }
        else
        {
            // If the weapon is not active, set it active.
            isActive = true;
            gameObject.SetActive(true);
            weaponStatus.SetActive(true);
        }
    }

    // The following function gives the player the ability to buy ammo, when the player is within an BuyAmmo object.
    private void buyAmmo()
    {

        // If the player presses the B key and has an appropriate amount of toilet paper,
        if (paperCount >= ammoPrice)
        {
            print("Buying ammo");
            // add to the ammo amount of the weapon and reduce the amount of toilet paper the player has.
            currentAmmo += ammoPurchaseAmount;
            PaperCount.count -= ammoPrice;
        }
    }

    // Creating a method which is called from the inventory, which essentially unequips weapons before switching to other weapons
    public override void UnEquip()
    {
        // Disabling the weapons animation and the object itself. 
        CharacterAnimator.animator.SetBool("DispenserShoot", false);
        isActive = false;
        gameObject.SetActive(false);

        // Resetting the weapon status text.
        weaponStatus.SetActive(false);
        weaponName.text = "";
        ammoDisplay.text = "";
        damageDisplay.text = "";
    }

    // This method is used for upgrading this weapon.
    public void Upgrade()
    {
        // Checking the weapon is not fully upgraded and the if the player has sufficient amount of toilet paper.
        if (timesUpgraded < maxUpgrade && paperCount >= upgradePrice)
        {
            // Playing the upgrade sound effect and adding to the timesUpgraded counter.
            source.PlayOneShot(upgrade, 0.5f);
            timesUpgraded++;
            
            // Increasing the weapons damage, reducing the amount of paper and increasing the upgrade price.
            damage += upgradeAmount;
            PaperCount.count -= upgradePrice;
            upgradePrice += upgradePrice / 3;
        }
    }

}

// Noting down the position and rotation while being a child of the hand bone
// POS: -0.00064, 0.00444, 0.00124
// ROT: -73.19701, -74.898, -99.271