using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minigun : InventoryItemBase
{

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioClip clip2;
    [SerializeField] private AudioClip reload;
    [SerializeField] private AudioClip upgrade;
    [SerializeField] private GameObject OffhandMinigun;
    [SerializeField] private ParticleSystem muzzleFlash;

    private float audioCooldown = 1.42f;
    private float audioElapsedTime;
    private float upgradeAmount = 0.5f;

    private int maxUpgrade = 10;
    private int timesUpgraded = 0;
    private int paperCount;
    private int ammoPrice = 40;
    private int upgradePrice = 80;
    private int ammoPurchaseAmount = 200;
    private int price = 400;
    private new string name = "Wuhan Annihilator 7000";

    private ThirdPersonMovement controller;
    private GameObject pickupText;
    public static int currentAmmo;



    // The following 2 float variables are used to create a fixed firerate for the weapon.
    private float elapsedTime = 0f;
    private bool isActive;

    void Start()
    {
        currentAmmo = 2000;
        audioElapsedTime = audioCooldown;
        // Finding the wanted gameobjects and components.
        pickupText = GameObject.Find("PickupText");
        player = GameObject.Find("ThirdPersonPlayer");
        controller = player.GetComponent<ThirdPersonMovement>();

    }

    // Overriding the InventoryItem-class Name method, which returns the item's name.
    public override string Name
    {
        get
        {
            return name;
        }
    }

    // Overriding the InventoryItem-class GetPrice method, which returns the item's price.
    public override int GetPrice
    {
        get
        {
            return price;
        }
    }
    // Overriding some of the InventoryItemBase-functions.
    public override string GetDamage
    {
        get
        {
            return damage.ToString();
        }
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
                    raycastObject.GetComponent<Animator>().Play("MinigunFire");
                    elapsedTime = 0f;

                    // If the player gives no input but the weapon is active, 
                }
                else if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    // return the weapon to its idle-state.
                    audioElapsedTime = audioCooldown;
                    source.Stop();
                    muzzleFlash.Stop();
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

                // If the boolean value ammoBuy is true, give the player a possibility to buy ammo.
                if (ThirdPersonMovement.ammoBuy)
                {
                    UpdateAmmoText();

                    if (Input.GetKeyDown(KeyCode.B))
                    {
                        buyAmmo();
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
             * disable the minigun idle-animation ,set the weapon inactive and reset the weapon status text. 
             This function also controls the off-hand minigun. */
            CharacterAnimator.animator.SetBool("minigun", false);
            isActive = false;
            gameObject.SetActive(false);
            weaponStatus.SetActive(false);

            // Setting the offhand minigun inactive.
            OffhandMinigun.GetComponent<OffhandMinigun>().SetActive();
            OffhandMinigun.GetComponent<OffhandMinigun>().UnEquip();

        } else
        {
            // Setting the offhand minigun active.
            OffhandMinigun.GetComponent<OffhandMinigun>().SetActive();
            OffhandMinigun.GetComponent<OffhandMinigun>().OnUse();

            // If the weapon is not active, set it active.
            CharacterAnimator.animator.SetBool("minigun", true);
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
            source.PlayOneShot(reload, 0.5f);
            currentAmmo += ammoPurchaseAmount;
            PaperCount.count -= ammoPrice;
        }
    }

    // Creating a method which is called from the inventory, which essentially unequips weapons before switching to other weapons
    public override void UnEquip()
    {
        // Disabling the weapons animation and the object itself. 
        CharacterAnimator.animator.SetBool("minigun", false);
        isActive = false;
        gameObject.SetActive(false);

        // Resetting the weapon status text.
        weaponStatus.SetActive(false);
        weaponName.text = "";
        ammoDisplay.text = "";
        damageDisplay.text = "";

        // Disabling the off hand minigun by calling its own functions.
        OffhandMinigun.GetComponent<OffhandMinigun>().SetActive();
        OffhandMinigun.GetComponent<OffhandMinigun>().UnEquip();
    }

    // This method is used for upgrading this weapon.
    public void Upgrade()
    {
        // Checking if the item is not fully upgraded and the player has sufficient amount of paper.
        if (timesUpgraded < maxUpgrade && paperCount >= upgradePrice)
        {
            // play upgrade soundeffect.
            source.PlayOneShot(upgrade, 0.5f);

            // Add 1 to the timesUpgraded, reduce the amount of paper the player has and increase the upgradePrice.
            timesUpgraded++;
            damage += UpgradeAmount;
            PaperCount.count -= upgradePrice;
            upgradePrice += upgradePrice / 3;
        }
    }
}



// Noting down the position, rotation and scale of the weapon while being a child of the hand bone.

// RIGHT HAND:
// POSITION: -0.00211, 0.00807, -0.00258
// ROT: -113.263, -265.643, 96.08299
// SCALE: 0.01000001, 0.01, 0.01