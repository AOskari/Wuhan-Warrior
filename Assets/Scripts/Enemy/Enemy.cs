using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class mainly controls the enemy's audioclips, animations and taking damage. It is separate from the enemy controller, because 
public class Enemy : MonoBehaviour
{
    [SerializeField] private Spawner spawn;
    [SerializeField] private GameObject toiletPaper;
    [SerializeField] private Animator animator;

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip1;
    [SerializeField] private AudioClip clip2;
    [SerializeField] private AudioClip clip3;
    [SerializeField] private AudioClip deathSound;

    private float deathElapsedTime = 0f;
    private float deathAnimation = 0.933f;

    private bool dead;
    private bool playedDeathAnimation;

    private float health;
    private List<AudioClip> clips;

    void Start()
    {
        // Adding the sound effects to a list
        clips = new List<AudioClip>();
        clips.Add(clip1);
        clips.Add(clip2);
        clips.Add(clip3);

        // Giving a few variables a value or reference
        playedDeathAnimation = false;
        dead = false;
        spawn = GameObject.Find("Spawner").GetComponent<Spawner>();
        health = spawn.GetHealth();

        // Playing a sound effect  to indicate that an enemy has spawned,.
        source.PlayOneShot(clips[Random.Range(0, 2)], 0.5f);
    }

    void Update()
    {
        // Checking if the enemy is dead.
        if (dead)
        {
            // Creating a slight delay during death, so the animation wont get interrupted.
            deathElapsedTime += Time.deltaTime;

            // Playing the death animation and sound effect once.
            if (!playedDeathAnimation)
            {
                animator.Play("Die");
                source.PlayOneShot(deathSound, 0.5f);
                playedDeathAnimation = true;
            }

            if (deathElapsedTime >= deathAnimation)
            {
                Die();
            }
        } 
    }

    // Creating a method which enables the enemy to take damage.
    public void TakeDamage(float amount)
    {
        // If the health is above 0, Playing the TakeDamage animation and reduce the health by the wanted amount.
        if (health > 0f)
        {
            animator.Play("TakeDamage");
        }
        
        health -= amount;
        print("Taking damage");

        // If the health is 0 or less, call the Die-function.
        if (health <= 0f)
        {
            dead = true;
        }
    }

    // Simple method to get the amount of health the enemy has.
    public float GetEnemyHealth()
    {
        return health;
    }

    // Creating a method which adds to the Spawner's enemy killed amount and operates it accordingly, and also kills the enemy.
    public void Die()
    {
        // Adding 1 to the following values.
        Spawner.enemiesKilledInTotal++;
        spawn.enemiesKilled++;

        int amountOfPaper = Random.Range(5, 8);
        // Spawning a random amount of toilet paper around the enemy.
        for (int i = 0; i < amountOfPaper; i++)
        {
            float posX = Random.Range(0f, 2f);
            float posZ = Random.Range(0f, 2f);
            Vector3 newPos = new Vector3(posX, 2f, posZ);
            Instantiate(toiletPaper, transform.position + newPos, transform.rotation);
        }

        // Finally, play the death sound effect and destroy the object.   
        Destroy(transform.parent.gameObject);
    }

}
