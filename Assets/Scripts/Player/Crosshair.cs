using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

// A simple script, which moves the crosshair object according to the player's UI reticle. 
public class Crosshair : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float reticleStartPoint;
    [SerializeField] private Transform Reticle;

    void Update()
    {
        // Creating a new vector3, which gets the mouse's x and y coordinates and the camera's z-coordinates.
        Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(Reticle.transform.position.x, Reticle.transform.position.y, Mathf.Abs(Camera.main.transform.position.z * 1.5f)));

        // Giving the crosshair the same position as the reticle.
        transform.position = newPos;
    }

}
