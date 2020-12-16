using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject hotKeys;
    [SerializeField] private GameObject restart;
    [SerializeField] private GameObject instructions;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip1;
    [SerializeField] private AudioClip clip2;
    [SerializeField] private Respawn respawn;
    [SerializeField] private Slider volume;
    private bool isPaused;

    public static bool paused;
    public static bool hitmarkerSound;

    void Start()
    {
        hitmarkerSound = false;
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Setting the volume value according to the value of the volume slider.
        AudioListener.volume = volume.value;

        paused = isPaused;

        // Activating or deactivating the pause menu when escape is pressed.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                ActivateMenu();
            }
            else
            {
                DeactiveMenu();
            }
        }  

    }

    // this method displays the pause menu panel, and pauses the game.
    void ActivateMenu()
    {
        // Pausing the game by setting the timescale to 0.
        Time.timeScale = 0;

        ThirdPersonMovement.allowInput = false;

        // Pausing all audiosources.
        AudioListener.pause = true;

        // Setting the pause menu panel active.
        pauseMenu.SetActive(true);

        // And finally unlocking the cursor.
        Cursor.lockState = CursorLockMode.None;
    }

    public void DeactiveMenu()
    {
        // Playing a soundeffect and deactivating all other child objects.
        source.PlayOneShot(clip2, 0.5f);
        HideHotKeys();

        // Unpausing the game by setting the timescale back to 1.
        Time.timeScale = 1;
        // Unpausing the audio.
        AudioListener.pause = false;

        // Deactivating the pause menu.
        pauseMenu.SetActive(false);
        isPaused = false;

        // Locking the cursor back in place, making it disappear.
        Cursor.lockState = CursorLockMode.Locked;

        // Finally set the allowInput boolean true, allowing the player give inputs.
        ThirdPersonMovement.allowInput = true;
    }

    // These methods are used for displaying and hiding child objects of the pause menu.
    public void ShowHotKeys()
    {
        source.PlayOneShot(clip1, 0.5f);
        hotKeys.SetActive(true);
    }

    public void HideHotKeys()
    {
        source.PlayOneShot(clip2, 0.5f);
        hotKeys.SetActive(false);
    }

    public void Restart()
    {
        respawn.ResetDay();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        DeactiveMenu();
    }

    public void ShowRestart()
    {
        source.PlayOneShot(clip1, 0.5f);
        restart.SetActive(true);
    }

    public void HideRestart()
    {
        source.PlayOneShot(clip2, 0.5f);
        restart.SetActive(false);
    }

    public void ToggleHitmarkerSound()
    {
        hitmarkerSound = !hitmarkerSound;
    }

    public void ShowInstr()
    {
        source.PlayOneShot(clip1, 0.5f);
        instructions.SetActive(true);
    }


    public void HideInstr()
    {
        source.PlayOneShot(clip2, 0.5f);
        instructions.SetActive(false);
    }

}
