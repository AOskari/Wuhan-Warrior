using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Creating a stamina bar script, which changes the length of 
 * the stamina bar according to the current stamina amount. */

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    // A method for setting the max value of the stamina slider.
    public void SetMaxStamina(float stamina)
    {
        slider.maxValue = stamina;
        slider.value = stamina;
    }

    // A method for updating the value of the stamina bar.
    public void SetStamina(float stamina)
    {
        slider.value = stamina;
    }

}
