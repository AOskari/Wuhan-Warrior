using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Respawn : MonoBehaviour
{

    [SerializeField] private Text text1;
    [SerializeField] private Text text2;
    [SerializeField] private Text text3;
    [SerializeField] private Text text4;
    [SerializeField] private Text text5;
    [SerializeField] private Text text6;
    [SerializeField] private Text day;
    [SerializeField] private Text enemyAmount;

    private float health;
    private float maxHealth;
    private bool playerDead;

    private GameObject player;
    private GameObject endScreen;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        endScreen = GameObject.Find("EndScreen");
        playerDead = false;
        health = ThirdPersonMovement.currentHealth;
        maxHealth = ThirdPersonMovement.maxHealth;
        player = GameObject.Find("ThirdPersonPlayer");
    }



    // ================================================================================================== //
    // ====================================      UPDATE BEGINS       ==================================== //
    // ================================================================================================== //

    void Update()
    {
        // Updating the health variable using the player's health.
        health = ThirdPersonMovement.currentHealth;

        if (health <= 0)
        {
            AudioListener.pause = true;
            // Checking if this is the first time the animations are called, so It wont get repeated while the player has 0 health.
            if (!playerDead)
            {
                // Checking if the wave number is 1.
                if (Spawner.wave == 1)
                {
                    // If it is, write the text accordingly.
                    text2.text = "You survived " + Spawner.wave + " day";
                } else
                {                
                    text2.text = "You survived " + Spawner.wave + " days";
                }             
                text3.text = "Gathered " + PaperCount.collectedInTotal + " toilet paper";

                // Checking if enemies killed is 1, 
                if (Spawner.enemiesKilledInTotal == 1)
                {
                    // if is, write the text accordingly.
                    text4.text = "And destroyed " + Spawner.enemiesKilledInTotal + " enemy.";
                } else
                {
                    text4.text = "And destroyed " + Spawner.enemiesKilledInTotal + " enemies.";
                }

                // Playing an animation which gradually displays the text on the endscreen.
                endScreen.GetComponent<Animation>().Play();
                text1.GetComponent<Animation>().Play();
                text2.GetComponent<Animation>().Play();
                text3.GetComponent<Animation>().Play();
                text4.GetComponent<Animation>().Play();
                text5.GetComponent<Animation>().Play();
                text6.GetComponent<Animation>().Play();
            }
            playerDead = true;
            
            // Checking if the player presses the B key
            if (Input.GetKeyDown(KeyCode.B))
            {
                // resetting the game.
                ResetDay();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }    
    }

    // ================================================================================================== //
    // ====================================       UPDATE ENDS        ==================================== //
    // ================================================================================================== //

    // A method which resets the day and enemyamount indicator on the UI.
    public void ResetDay()
    {
        AudioListener.pause = false;
        Spawner.wave = 0;
        day.text = "";
        enemyAmount.text = "";
    }
}
