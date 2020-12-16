using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using Cinemachine;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine.UI;

// This class controls the player's movement and status.
public class ThirdPersonMovement : MonoBehaviour
{

    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cam;
    [SerializeField] private CinemachineFreeLook vcam;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioClip clip2;

    [SerializeField] private AudioClip pickup;
    [SerializeField] private AudioClip grabItem;
    [SerializeField] private AudioClip hitmarker;
    [SerializeField] private AudioClip upgradeMask;
    [SerializeField] private AudioClip useInjection;

    [SerializeField] private StaminaBar staminaBar;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Inventory inventory;
   
    [SerializeField] private Transform crosshair;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject weaponDesc;
    [SerializeField] private Text weaponName;
    
    [SerializeField] private Text weaponDamage;
    [SerializeField] private Text rateOfFire;
    [SerializeField] private Text range;
    [SerializeField] private Text price;
    [SerializeField] private Text buy;
  
    private float audioCooldown = 0.4f;
    private float audioElapsedTime;
    private float gravity = -9.81f;
    private float jumpHeight = 1.5f;
    private float maxStamina = 100f;

    private float minFOV = 25f;
    private float maxFOV = 40f;
    private float rotateCooldown = 3f;
    private float rotateElapsedTime = 0f;
    private float turnSmoothVelocity;
    private int maskUpgradePrice = 30;

    private Vector3 lastPosition;
    private Vector3 velocity;

    private bool isGrounded;
    private bool isShift;
    private bool isMoving;
    private bool isCtrl;
    private bool isClick;
    private bool recentlyClicked;
    private List<AudioClip> clips;

    public float speed = 3f;
    public float groundDistance = 0.4f;
    public float turnSmoothTime = 0.1f;

    public static int ethanolInjections;
    public static int epinephrine;
    public static float stamina; 
    public static float maxHealth = 100f;
    public static float currentHealth;
    public static float staminaNeededToJump = 10f;
    public static float maskProtectionValue = 0f;
    public static bool ammoBuy;
    public static bool upgradeWeapon;
    public static bool allowInput;

    void Start()
    {
        allowInput = true;
        clips = new List<AudioClip>();
        clips.Add(clip);
        clips.Add(clip2);

        ethanolInjections = 5;
        epinephrine = 5;
        recentlyClicked = false;
        audioElapsedTime = audioCooldown;
        ammoBuy = false;
        upgradeWeapon = false;

        // Locking the cursor to the middle of the screen.
        Cursor.lockState = CursorLockMode.Locked;
        vcam.m_CommonLens = true;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        stamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);

        inventory.itemUsed += InventoryItemUsed;
    }


    // ================================================================================================== //
    // ====================================      UPDATE BEGINS       ==================================== //
    // ================================================================================================== //

    void Update()
    {
        // Reading the player's input if the player is alive and the game is not paused.
        if (currentHealth >= 0f && allowInput)
        {
            audioElapsedTime += Time.deltaTime;

            /* Adding to the rotation elapsed time when the player has clicked the mouse.
             * this is used to block the player constantly turning around while clicking rapidly. */
            if (rotateElapsedTime < rotateCooldown)
            {
                rotateElapsedTime += Time.deltaTime;
            }
            else
            {
                recentlyClicked = false;
            }

            // Creating the possibility to use the consumables using the Q or E key.
            if (ethanolInjections > 0 && Input.GetKeyDown(KeyCode.Q) && currentHealth != maxHealth)
            {
                ethanolInjections--;
                source.PlayOneShot(useInjection, 0.5f);
                if (currentHealth + 30f >= maxHealth)
                {
                    currentHealth = maxHealth;
                }
                else
                {
                    currentHealth += 30f;
                }
            }
            // If E is pressed, the player has sufficient amount of epinephrine and stamina is lower than 100,
            if (epinephrine > 0 && Input.GetKeyDown(KeyCode.E) && stamina != maxStamina)
            {
                // Use an epinephrine.
                epinephrine--;
                source.PlayOneShot(useInjection, 0.5f);
                if (stamina + 50f >= maxStamina)
                {
                    stamina = maxStamina;
                }
                else
                {
                    stamina += 50f;
                }
            }

            // Creating a possibility to jump if the player is grounded, the camera is not zoomed in and the player has sufficient stamina.
            if (Input.GetButtonDown("Jump") && isGrounded && !Input.GetMouseButton(1) && stamina >= staminaNeededToJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity / 2);
                stamina -= staminaNeededToJump;
            }

            // Getting the movement input from the WASD keys,
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // and creating a new direction from the given inputs.
            Vector3 direction = new Vector3(horizontal, 0f, vertical);

            // Creating a new rotation which rotates the player towards the crosshair.
            Vector3 towardsCrosshair = (crosshair.transform.position - transform.position);
            towardsCrosshair.y = 0f;
            Quaternion lookRotation = Quaternion.LookRotation(towardsCrosshair);

            // Creating variables which will be used for moving and rotating the player to a wanted direction.
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            // Creating a smooth transition towards the wanted direction.
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 currentDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                transform.rotation = lookRotation;
                isClick = true;
            }
            else
            {
                isClick = false;
            }

            // If the player is moving, move the player towards the given direction.
            if (direction.magnitude >= 0.1f && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
            {
                isMoving = true;
                if (isGrounded)
                {
                    // Move the player to the pointed direction,
                    controller.Move(moveDir * speed * Time.deltaTime);

                    // Adding a walking sound effect which speeds up or slows down depending on the player's speed.
                    if (audioElapsedTime >= audioCooldown)
                    {
                        if (speed >= 3f && speed < 6f)
                        {
                            audioCooldown = 0.45f;
                        }
                        else if (speed >= 6f)
                        {
                            audioCooldown = 0.35f;
                        }
                        source.PlayOneShot(clips[Random.Range(0, 2)], 0.05f);
                        audioElapsedTime = 0f;
                    }

                    // If the player is clicking with the mouse,
                    if (isClick)
                    {
                        // reset the rotation cooldown and set the recentlyClicked boolean true.
                        rotateElapsedTime = 0f;
                        if (!recentlyClicked)
                        {
                            recentlyClicked = true;
                        }

                        // If the player has not clicked recently,
                    }
                    else if (!recentlyClicked)
                    {
                        // rotate the player mesh according to the movement input.
                        transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    }

                    // If recentlyClicked,
                    if (recentlyClicked)
                    {
                        // Rotate the player towards the crosshair.
                        transform.rotation = lookRotation;
                    }
                }
                else if (!isGrounded)
                {
                    // If the player is not grounded, restrict the movement to only forward, so the player can't float around while airborne.
                    controller.Move(currentDir * speed * Time.deltaTime);
                }
            }
            else
            {
                isMoving = false;
            }
        }    
    }

    // ================================================================================================== //
    // ====================================       UPDATE ENDS        ==================================== //
    // ================================================================================================== //





    // ================================================================================================== //
    // ====================================   FIXED UPDATE BEGINS    ==================================== //
    // ================================================================================================== //

    // This section is mainly used to determine the player's physics and other player inputs which need a fixed update timer.
    void FixedUpdate()
    {

       
        // Checking if the player is grounded using Checksphere. If the checksphere touches the ground, it returns true.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Checking if the player has the left shift button down. 
        isShift = Input.GetKey(KeyCode.LeftShift);

        // Checking if the player has the left ctrl button down.
        isCtrl = Input.GetKey(KeyCode.LeftControl);


        /* If the player is grounded, and the the velocity in the y-axis is smaller than 0, 
         * set the player's speed to 3 and y-velocity to -2f. In this case, y-velocity means the gravity.
         */

        if (isGrounded && velocity.y < 0)
        {
            speed = 3f;
            velocity.y = -2f;
        }

        /* Creating an if-statement, which gradually reduces the player's speed to 0 if the player is not grounded,
         * creating the realistic effect that the player's forward velocity slows down if he's above ground.
         */

        if (!isGrounded)
        {
            if (speed <= 0f)
            {
                speed = 0f;
            }
            else
            {
                speed -= 0.03f;
            }
        }


        /* Updating the vertical velocity in-case the player is above ground, (in this case 9.81f) and 
         * multiplying it with Time.deltaTime so the speed doesn't change depending on the computer's framerate. */

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


        // Creating a sprint input 

        if (isShift && isGrounded && stamina > 0.2f && isMoving)
        {
            speed = 6f;
            stamina -= 0.2f;
            staminaBar.SetStamina(stamina - 0.2f);
        }

        // If the player is moving at the same time, regenerate stamina slower.
        if (!isShift && stamina < 100f && isMoving)
        {
            stamina += 0.05f;
            staminaBar.SetStamina(stamina + 0.05f);
        }

        // If the player is not running, recharge the stamina over time.
        else if (!isShift && stamina < 100f && !isMoving)
        {
            stamina += 0.1f;
            staminaBar.SetStamina(stamina + 0.1f);
        }       

        // Creating a crouch input
        if (isCtrl && isGrounded)
        {
            speed /= 2;
        }     

        // Creating the possibility to aim using the right mouse button
        if (Input.GetMouseButton(1) && isGrounded)
        {
            if (vcam.m_Lens.FieldOfView > minFOV)
            {
                vcam.m_Lens.FieldOfView -= 1f;            
            }
            speed /= 2;
        }
        else
        {
            vcam.m_Lens.FieldOfView = maxFOV;
        }

    }
    // ================================================================================================== //
    // ====================================    FIXED UPDATE ENDS     ==================================== //
    // ================================================================================================== //





    // ================================================================================================== //
    // ==================================     OTHER FUNCTIONS BELOW     ================================= //
    // ================================================================================================== //

    // A Method which plays the hitmarker sound effect.
    public void PlayHitMarker()
    {
        source.PlayOneShot(hitmarker, 0.1f);
    }

    // A Method which returns the player's current speed.
    public float GetSpeed()
    {
        return this.speed;  
    }


    // Creating a possibility to reduce the player's health when taking damage. 
    public void TakeDamage(float damage)
    {
        currentHealth -= (damage - maskProtectionValue);
    }

    // A simple function which upgrades the mask's protection value if it is under 5.
    private void UpgradeMask()
    {
        // reduce the amount of paper the player has and upgrade the mask.
        print("Upgrading mask.");
        source.PlayOneShot(upgradeMask, 0.3f);
        PaperCount.count -= maskUpgradePrice;
        maskProtectionValue += 0.5f;
        maskUpgradePrice += maskUpgradePrice / 3;              
    }

    // A method for activating and setting the text within the weaponDesc gameobject.
    private void SetWeaponDesc(string name, string damage, string rof, string ranged, string weaponPrice, string buyText)
    {
        weaponDesc.SetActive(true);
        weaponName.text = name;
        weaponDamage.text = damage;
        rateOfFire.text = rof;
        range.text = ranged;
        price.text = weaponPrice;
        buy.text = buyText;
    }

    // Creating a method which equips the item to the player's hand when clicked. This method is added to a eventhandler.
    private void InventoryItemUsed(object sender, InventoryEventArgs e)
    {
        // Getting the current item which is being handled
        InventoryItem item = e.Item;

        // Getting the item's gameObject.
        GameObject gameItem = (item as MonoBehaviour).gameObject;

        // Setting it active, displaying it
        gameItem.SetActive(true);

        // Placing the item to the player's right hand and positioning it correctly.
        gameItem.transform.parent = hand.transform;
        gameItem.transform.localPosition = (item as InventoryItemBase).Position;
        gameItem.transform.localEulerAngles = (item as InventoryItemBase).Rotation;
    }

    // ================================================================================================== //
    // ==================================     OTHER FUNCTIONS ENDS      ================================= //
    // ================================================================================================== //






    // ================================================================================================== //
    // ==================================== COLLIDER FUNCTIONS BEGIN ==================================== //
    // ================================================================================================== //

    // Creating a collider function, which determines what happens when the player's collider collides with an InventoryItem.
    void OnTriggerStay(Collider hit)
    {
        //TODO: Possible purchase sound effects.
        // If collided with an object with an InventoryItem class, add it to the inventory.
        InventoryItem item = hit.GetComponent<Collider>().GetComponent<InventoryItem>();

        // If the collided object is an item,
        if (item != null)
        {
            // set the pick up indicator active and set the wanted text.
            SetWeaponDesc(item.Name, "Damage: " + item.GetDamage, "Rate of fire: " + item.RateOfFire, "Range: " + item.GetRange, "Price: " + item.GetPrice, "B Purchase");
            // If the pickup button is pressed, add the item to the inventory and disable the pickup indicator.
            if (Input.GetKey(KeyCode.B) && PaperCount.count >= item.GetPrice)
            {
                source.PlayOneShot(grabItem, 0.5f);
                PaperCount.count -= item.GetPrice;
                inventory.AddItem(item);
                weaponDesc.SetActive(false);
            }
        }

        // If picking up toiletpaper, play a sound effect.
        if (hit.gameObject.tag == "Toiletpaper")
        {
            source.PlayOneShot(pickup, 0.15f);
        }

        // If the player is inside the collider of an ammo pack set AmmoBuy true.
        if (hit.gameObject.tag == "BuyAmmo")
        {
            ammoBuy = true;
            weaponDesc.SetActive(true);
        }

        // If the player is within a collider of an mask upgrade station,
        if (hit.gameObject.tag == "UpgradeMask")
        {
            // and if the maskProtection value is not 5,
            if (maskProtectionValue < 5f)
            {
                // Indicate the player by displaying a text,
                SetWeaponDesc("Inquisitor's Mask", "Upgrade", "+0.5 Damage reduction", "", "Price: " + maskUpgradePrice, "B Upgrade");
            
                // and if the B key is pressed and the papercount is high enough,
                if (Input.GetKeyDown(KeyCode.B) && PaperCount.count >= maskUpgradePrice)
                {
                    // upgrade the mask.
                    UpgradeMask();
                }
            } else
            {
                // If the mask is fully upgraded, display a text.
                SetWeaponDesc("", "The mask is fully upgraded.", "", "", "", "");
            }       
        }

        // If the player is standing nex to the workbench,
        if (hit.gameObject.tag == "UpgradeWeapon")
        {
            // Set the wepaondescription active and the upgradeWeapon boolean true.
            weaponDesc.SetActive(true);
            upgradeWeapon = true;
        }

        // if the player is standing next to the firstAid box,
        if (hit.gameObject.tag == "FirstAid")
        {
            // Display the following text,
            SetWeaponDesc("Ethanol Injection", "Heals for 30 hitpoints", "", "", "Price: 10", "B Purchase");

            // If B is pressed and the papercount is high enough, buy an item.
            if (Input.GetKeyDown(KeyCode.B) && PaperCount.count >= 10)
            {
                source.PlayOneShot(grabItem, 0.5f);
                PaperCount.count -= 10;
                ethanolInjections++;
            }
        }

        // if the player is standing next to the Adrenaline box,
        if (hit.gameObject.tag == "AdrenalineKit")
        {
            // Display the following text,
            SetWeaponDesc("Epinephrine", "Regenerates 50 stamina", "", "", "Price: 10", "B Purchase");

            // If B is pressed and the papercount is high enough, buy an item.
            if (Input.GetKeyDown(KeyCode.B) && PaperCount.count >= 10)
            {
                source.PlayOneShot(grabItem, 0.5f);
                PaperCount.count -= 10;
                epinephrine++;
            }
        }
    }

    // When exiting an collider, disable the pickup indicator.
    void OnTriggerExit(Collider hit)
    {
        weaponName.text = "";
        weaponDamage.text = "";
        rateOfFire.text = "";
        range.text = "";
        price.text = "";
        buy.text = "";
        weaponDesc.SetActive(false);
        ammoBuy = false;
        upgradeWeapon = false;
    }
    // ================================================================================================== //
    // ====================================  COLLIDER FUNCTIONS END  ==================================== //
    // ================================================================================================== //
}
