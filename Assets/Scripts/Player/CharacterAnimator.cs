using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private ThirdPersonMovement movement;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;

    private float groundDistance = 0.4f;
    private float animationSmoothTime = .1f;
    private float speedPercent;
    private float currentStamina;

    private bool isMoving;
    private bool isGrounded;
    private bool isShift;
    private bool deathAnim;

    public static Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Getting input if the player is alive.
        if (ThirdPersonMovement.currentHealth > 0)
        {      
            currentStamina = ThirdPersonMovement.stamina;
            speedPercent = gameObject.GetComponent<ThirdPersonMovement>().GetSpeed() / 8f;
            float staminaNeededToJump = ThirdPersonMovement.staminaNeededToJump;

            isShift = Input.GetKey(KeyCode.LeftShift);

            // Checking if the player is moving
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

            // IF the player is moving,
            if (isMoving)
            {
                // Change the animator's speedpercent value according to the player's speed. This changes between a walk and run animation.           
                animator.SetFloat("speedPercent", speedPercent, animationSmoothTime, Time.deltaTime);
            }
            else
            {
                // If not moving, set the speedpercent value to 0, activating the idle animation.
                animator.SetFloat("speedPercent", 0f, animationSmoothTime, Time.deltaTime);
            }

            // If the player is pressing the spacebar, is grounded and has sufficient amount of stamina,
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && currentStamina >= staminaNeededToJump)
            {
                // Activate the jump animation.
                animator.SetBool("jump", true);
            }
            else
            {
                animator.SetBool("jump", false);
            }
        } else
        {
            // If the player has 0 health, play the death animation.
            animator.Play("Death");
        }
    }
}


    






