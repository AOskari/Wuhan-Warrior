using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script controls the wave environment.
public class Spawner : MonoBehaviour
{

    [SerializeField] private Animation animate;
    [SerializeField] private new AudioSource audio;
    [SerializeField] private AudioSource waveStartSource;
    [SerializeField] private AudioClip waveStart;
    [SerializeField] private AudioClip waveStart2;

    [SerializeField] private AudioClip track1;
    [SerializeField] private AudioClip track2;

    [SerializeField] private GameObject waveIndicator;
    [SerializeField] private GameObject enemy;
    [SerializeField] private Text day;
    [SerializeField] private Text EnemyCounter;
    [SerializeField] private float elapsedTime = 0f;
    [SerializeField] private float timeBetweenSpawn = 5f;
    [SerializeField] private GameObject safezone;

    private int enemyAmount = 0;
    private int enemiesSpawned = 0;
    private int spawnerAmount = 10;

    public int enemiesKilled = 0;
    public static int wave = 0;
    public static int enemiesKilledInTotal = 0;
    private float enemyHealth = 25f;

    private float increaseAmount = 0.1f;
    private float stopAudioTimer = 5f;
    private float elapsedAudioTimer = 0f;
    private bool playerExitedArea;
    private bool playedFadeOut;

    private GameObject[] spawners;
    private Enemy enemyScript;

    private List<AudioClip> tracks;

    void Start()
    {
        // Creating a new List of audioclip, and getting the Enemy script.
        tracks = new List<AudioClip>();
        enemyScript = enemy.transform.GetChild(0).GetComponent<Enemy>();

        playedFadeOut = false;
        playerExitedArea = false;

        // Adding 2 soundtracks to the list.
        tracks.Add(track1);
        tracks.Add(track2);

        spawners = new GameObject[spawnerAmount];

        // Setting up the spawners to the spawners list.
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = transform.GetChild(i).gameObject;
        }
    }
    
    void Update()
    {
        // Start the audio timer if the player has exited the area.
        if (playerExitedArea)
        {
            elapsedAudioTimer += Time.deltaTime;
            if (elapsedAudioTimer >= stopAudioTimer)
            {
                elapsedAudioTimer = 0f;
                audio.Stop();
            }
        }

        // When the wave has ended and the playedFadeOut boolean is false,
        if (enemiesKilled >= enemyAmount && !playedFadeOut)
        {
            // start the fadeout animation and set the playedFadeOut boolean true.
            animate.Play("AudioFadeOut");
            playedFadeOut = true;
        }

        // Updating the day and enemy counter text in the UI.
        if (wave > 0)
        {
            day.text = "Day " + wave;
            EnemyCounter.text = "Enemies destroyed " + enemiesKilled + "/" + enemyAmount;
        }

        // If the wave is cleared, change the enemy counter text and deactivate the spawnArea collider.
        if (enemiesKilled >= enemyAmount && wave > 0)
        {
            safezone.SetActive(false);
            EnemyCounter.text = "Area cleared.";
        }

        // If the spawn cooldown has recharged,
        if (elapsedTime > timeBetweenSpawn)
        {
            // spawn an enemy.
            SpawnEnemy();
        }

        // Increasing the spawn timer over time.
        elapsedTime += Time.deltaTime;
    }

    // ================================================================================================== //
    // ==================================        FUNCTIONS BELOW        ================================= //
    // ================================================================================================== //

    // Creating a method to spawn the enemies.
    private void SpawnEnemy()
    {
        if (enemiesSpawned < enemyAmount)
        {
            // Saving a integer which will be used to choose a random spawn point between the spawners.
            int spawnerID = Random.Range(0, spawners.Length);

            // Instantiating an enemy to a random EnemySpawner object.
            Instantiate(enemy, spawners[spawnerID].transform.position, spawners[spawnerID].transform.rotation);
            enemiesSpawned++;

            // Reset the spawn timer to 0.
            elapsedTime = 0f;
        }
    }

    // This is called everytime the last wave has completed and the player enters the spawner area.
    public void NextWave()
    {
        wave++;
        waveIndicator.GetComponent<TMPro.TextMeshProUGUI>().text = "Day " + wave;
        waveIndicator.GetComponent<Animation>().Play();
        safezone.SetActive(true);
        safezone.GetComponent<Safezone>().ResetTimer();
        // Alternating between 2 soundtracks.
        if (wave % 2 == 0)
        {
            audio.clip = track2;
            waveStartSource.PlayOneShot(waveStart2, 1f);
        } else
        {
            audio.clip = track1;
            waveStartSource.PlayOneShot(waveStart, 1f);
        }

        // Increase the amount of health enemies have by 10%.
        IncreaseHealth();
        playedFadeOut = false;     
        playerExitedArea = false;

        // Play a sound effect, increase spawned enemy amount and reset both enemiesKilled and enemeiesSpawned to 0.
        audio.Play();  
        enemyAmount += 5;
        enemiesKilled = 0;
        enemiesSpawned = 0;
        // Every 3 waves, make the spawn timer faster.   
        if (wave % 3 == 0 && timeBetweenSpawn > 1f)
        {
            timeBetweenSpawn--;
        }
    }

    // Method which is used for increasing the enemy's health after each wave.
    public void IncreaseHealth()
    {
        enemyHealth += enemyHealth * increaseAmount;
    }

    public float GetHealth()
    {
        return enemyHealth;
    }
    // ================================================================================================== //
    // ==================================         FUNCTIONS END         ================================= //
    // ================================================================================================== //






    // ================================================================================================== //
    // ==================================== COLLIDER FUNCTIONS BEGIN ==================================== //
    // ================================================================================================== //

    // This collider functions starts the next wave when the player enter's the spawner area, if the conditions are met.
    void OnTriggerEnter(Collider other)
    {
        // Starting the next wave when the player enters the area and if the previous wave has been cleared.
        if (enemiesKilled >= enemyAmount && other.gameObject.tag == "Player")
        {
            animate.Play("AudioFadeIn");
            NextWave();
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the player exits the are and the wave has completed, set the palyerExitedArea boolean true.
        if (enemiesKilled >= enemyAmount && other.gameObject.tag == "Player")
        {
            playerExitedArea = true;
           
        }
    }

    // ================================================================================================== //
    // ====================================  COLLIDER FUNCTIONS END  ==================================== //
    // ================================================================================================== //

}
