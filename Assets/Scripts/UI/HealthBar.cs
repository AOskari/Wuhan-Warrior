using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    private Animation heart;

    void Start()
    {
        heart = GameObject.Find("Heart").GetComponent<Animation>();
    }

    void Update()
    {
        // Getting the current and max health of the player,
        float currentHealth = ThirdPersonMovement.currentHealth;
        float maxHealth = ThirdPersonMovement.maxHealth;

        // and animating the heart according to the amount of health the player has.
        if (currentHealth / maxHealth >= 0.6f)
        {
            heart.Play("heartBeat1");
        }
        else if (currentHealth / maxHealth >= 0.3f && currentHealth / maxHealth < 0.6f)
        {
            heart.Play("heartbeat2");
        }
        else if (currentHealth / maxHealth > 0f && currentHealth / maxHealth < 0.3f)
        {
            heart.Play("heartbeat3");
        }


    }

    // A method which sets the healthbar's max value.
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    // A method for controlling the displayed health on the healthbar.
    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }


}
