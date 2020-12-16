using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ToiletPaper : MonoBehaviour
{

    // Creating a collision event, which destroys the toilet paper and adds to the paper counter.
    void OnTriggerStay(Collider other)
    {
        // If the collided object is the player,
        if (other.gameObject.tag == "Player")
        {
            // destroy the object and add to the papercounter.

            print("Collected toilet paper.");          
            Destroy(this.gameObject);
            PaperCount.count++;
            PaperCount.collectedInTotal++;
        }
    }

}
