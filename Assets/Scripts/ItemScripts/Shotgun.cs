using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shotgun : InventoryItemBase
{
    [SerializeField] private AudioClip upgrade;
    [SerializeField] private AudioClip reload;
    [SerializeField] private AudioSource source;
    [SerializeField] private ParticleSystem muzzleFlash;


    private float elapsedTime = 0f;
    private float upgradeAmount = 5f;

    private int price = 100;
    private int paperCount;
    private int timesUpgraded = 0;
    private int maxUpgrade = 10;
    private int ammoPurchaseAmount = 20;
    private int upgradePrice = 40;
    private int ammoPrice = 20;
   
    private ThirdPersonMovement controller;
    private bool isActive;
    public new string name = "Boomstick of Apocalypse";
    public static int currentAmmo;

    void Start()
    {
        currentAmmo = 50;
        player = GameObject.Find("ThirdPersonPlayer");
        controller = player.GetComponent<ThirdPersonMovement>();
    }

    void Update()
    {

        // Checking if the game is not paused.
        if (ThirdPersonMovement.allowInput)
        {

            elapsedTime += Time.deltaTime;
            paperCount = PaperCount.count;

            if (isActive)
            {
                ShowWeaponInfo();
                GetWeaponStatus();

                if (Input.GetKeyDown(KeyCode.Mouse0) && isActive && elapsedTime >= shootCooldown && currentAmmo > 0)
                {
                    currentAmmo--;
                    source.Play();
                    muzzleFlash.Play();
                    Shoot();
                    raycastObject.GetComponent<Animator>().Play("ShotgunShoot");
                    elapsedTime = 0f;
                }

                if (ThirdPersonMovement.ammoBuy)
                {
                    UpdateAmmoText();

                    if (Input.GetKeyDown(KeyCode.B))
                    {
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

    public override float UpgradeAmount
    {
        get
        {
            return upgradeAmount;
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

    public override string Name
    {
        get
        {
            return name;
        }
    }

    public override string GetDamage
    {
        get
        {
            return damage.ToString();
        }
    }

    public override int GetPrice
    {
        get
        {
            return price;
        }
    }

    public override string AmmoAmount
    {
        get
        {
            return currentAmmo.ToString();
        }
    }

    public override void OnUse()
    {

        if (isActive)
        {
            isActive = false;
            gameObject.SetActive(false);
            weaponStatus.SetActive(false);

        }
        else
        {
            isActive = true;
            gameObject.SetActive(true);
            weaponStatus.SetActive(true);
        }
    }


    public override void UnEquip()
    {
        isActive = false;
        gameObject.SetActive(false);
        weaponStatus.SetActive(false);
        weaponName.text = "";
        ammoDisplay.text = "";
        damageDisplay.text = "";
    }

    // This method is used for upgrading this weapon.
    public void Upgrade()
    {
        if (timesUpgraded < maxUpgrade && paperCount >= upgradePrice)
        {
            source.PlayOneShot(upgrade, 0.5f);
            timesUpgraded++;
            damage += upgradeAmount;
            PaperCount.count -= upgradePrice;
            upgradePrice += upgradePrice / 3;
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
}

// Noting down the rotation and position of the weapon while being a child of the hand bone.
// POS: 0.00033, 0.00079, 0.00015
// ROT: -172.501, -172.221, 9.938995

