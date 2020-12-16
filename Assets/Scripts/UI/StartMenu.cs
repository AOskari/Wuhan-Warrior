using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script controls the start menu, which is displayed when opening the game.
public class StartMenu : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip1;
    [SerializeField] private GameObject hotkeys;
    [SerializeField] private GameObject instructions;


    // The following methods control the buttons of the start menu.
    public void PlayGame()
    {
        source.PlayOneShot(clip1, 0.5f);
        SceneManager.LoadScene(1);
    }

    public void HideHotKeys()
    {
        source.PlayOneShot(clip1, 0.5f);
        hotkeys.SetActive(false);
    }

    public void ShowHotKeys()
    {
        source.PlayOneShot(clip1, 0.5f);
        hotkeys.SetActive(true);
    }

    public void HideInst()
    {
        source.PlayOneShot(clip1, 0.5f);
        instructions.SetActive(false);
    }

    public void ShowInst()
    {
        source.PlayOneShot(clip1, 0.5f);
        instructions.SetActive(true);
    }
}
