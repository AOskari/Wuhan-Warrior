using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Disinfectant : InventoryItemBase
{

    [SerializeField] private AudioSource source;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private AudioClip upgrade;

    private float elapsedTime = 0f;
    private float upgradeAmount = 1f;

    private int paperCount;
    private int timesUpgraded = 0;
    private int maxUpgrade = 10;
    private int upgradePrice = 20;


    private string currentAmmo = "Infinite";
    private bool isActive;
    private ThirdPersonMovement controller;
    public new string name = "Bottle of Inifinite Disinfectant";

    void Start()
    {
        player = GameObject.Find("ThirdPersonPlayer");
        controller = player.GetComponent<ThirdPersonMovement>();
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

    public override int UpgradePrice
    {
        get
        {
            return upgradePrice;
        }
    }

    public override string AmmoAmount
    {
        get
        {
            return currentAmmo;
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
            // Adding to the elapsedTime timer.
            elapsedTime += Time.deltaTime;
            paperCount = PaperCount.count;

            // IF the weapon is equipped
            if (isActive)
            {
                // Display the weapon information.
                ShowWeaponInfo();
                GetWeaponStatus();

                // If the player is clicking the left mouse button,
                if (Input.GetKeyDown(KeyCode.Mouse0) && isActive && elapsedTime >= shootCooldown)
                {
                    // Play the sound effect, muzzleflash and call the shoot method.
                    source.Play();
                    muzzleFlash.Play();
                    Shoot();
                    raycastObject.GetComponent<Animator>().Play("OneHandShoot");
                    elapsedTime = 0f;
                }

                if (ThirdPersonMovement.ammoBuy)
                {
                    UpdateAmmoText();
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
                        // Displaying the upgrade text.
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

    public override void UpdateAmmoText()
    {
        weaponDesc.transform.GetChild(0).GetComponent<Text>().text = "";
        weaponDesc.transform.GetChild(1).GetComponent<Text>().text = "";
        weaponDesc.transform.GetChild(2).GetComponent<Text>().text = "Cannot purchase ammo";
        weaponDesc.transform.GetChild(3).GetComponent<Text>().text = "";
        weaponDesc.transform.GetChild(4).GetComponent<Text>().text = "";
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
            damage += 1f;
            PaperCount.count -= upgradePrice;
            upgradePrice += upgradePrice / 3;
        }
    }
}

// POS: 0.00069, 0.00092, -0.00023
// ROT: -185.569, 188.001, 7.450989