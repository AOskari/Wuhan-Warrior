using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* This is a separate class for the off-hand minigun, which is mainly used as a visual aspect.
 * The main minigun does all the work and damage. */
public class OffhandMinigun : MonoBehaviour
{
   

    private ThirdPersonMovement controller;
    private GameObject player;
    public GameObject leftHand;

    public ParticleSystem muzzleFlash;
    public GameObject raycastObject;
    public int ammo;

    // Noting down the position and rotation of the minigun while being a child object of the left hand.
    private Vector3 Position = new Vector3(0.00409f, 0.00742f, -0.00109f);
    private Vector3 Rotation = new Vector3(-71.51f, 61.961f, 92.79101f);

    public static bool isActive;

    void Start()
    {
        isActive = false;
        player = GameObject.Find("ThirdPersonPlayer");
        controller = player.GetComponent<ThirdPersonMovement>();
    }

    void Update()
    {
        ammo = Minigun.currentAmmo;

        // If the player presses the left mouse button and the minigun is active with an appropriate amount of ammo,
        if (Input.GetKey(KeyCode.Mouse0) && isActive && ammo > 0)
        {
            // play the muzzleFlash particle-effect. The shooting animation is called from the main minigun class.
            muzzleFlash.Play();
        }
        // If the weapon is active but the player gives no input,
        else if (isActive)
        {
            // stop the muzzleFlash-effect.
            muzzleFlash.Stop();
        }
    }

    // Creating a OnUse method which places the off-hand minigun to the player's left hand.
    public void OnUse()
    { 
            // Changing the parent object from the main minigun to the left hand, and positioning and rotating it accordingly.
            transform.parent = leftHand.transform;
            transform.localPosition = Position;
            transform.localEulerAngles = Rotation;
        
            // Setting the weapon active.
            isActive = true;
            gameObject.SetActive(true);
            print("activated object");
    }      
    
    // This function is called from the main minigun class. It is used for unequipping the off-hand minigun.
    public void UnEquip()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
    
    // This function is called from the main minigun class. This is used to toggle the weapon active and inactive.
    public void SetActive()
    {
        if (isActive)
        {
            isActive = false;
        } else
        {
            isActive = true;
        }
    }
}



// POS:0.00409f, 0.00742f, -0.00109f
// ROT: -71.51f, 61.961f, 92.79101f
// SCALE: 0.01000002, 0.01000002, 0.01000002