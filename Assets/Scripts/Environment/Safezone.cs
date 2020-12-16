using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creating a short script, which indicates the player if he can't enter the safezone.
public class Safezone : MonoBehaviour
{
    [SerializeField] private GameObject waveIndicator;

    private float elapsedTime = 0f;
    private float waitAmount = 3f;

    void Update()
    {
        // Creating a slight delay on the indicator, so the player wont get indicated immediately when leaving the safezone.
        if (elapsedTime <= waitAmount)
        {
            elapsedTime += Time.deltaTime;
        }     
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
    }


    // Checking if the player collided with the safezone's collider.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && elapsedTime >= waitAmount)
        {
            // Indicate the player with a text.
            waveIndicator.SetActive(true);
        }
    }

    // Remove the indicator when the player leaves the collider.
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            waveIndicator.SetActive(false);
        }
    }
}
