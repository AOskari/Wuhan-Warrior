using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A simple script for controlling the particle system on the mask upgrade pedestal.
public class Sparkles : MonoBehaviour
{
    private ParticleSystem system;
    void Start()
    {
        // Getting the particle system component.
        system = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // Updating the emission rate of the particle system according to the mask's protection value.
        var emission = system.emission;
        emission.rateOverTime = (ThirdPersonMovement.maskProtectionValue + 1) * 25;
    }

}
