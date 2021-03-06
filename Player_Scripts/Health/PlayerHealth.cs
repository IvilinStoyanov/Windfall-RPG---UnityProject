﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public int totalHealth = 250;
    private static int regenHealthPerSec = 5;

    public float currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    playerLevelSystem playerLevelSystem;
    Animator anim;
    AudioSource playerAudio;
    PlayerMotor playerMotor;

    bool isDead;
    bool damaged;

    void Awake()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerLevelSystem = GetComponent<playerLevelSystem>();
        playerMotor = GetComponent<PlayerMotor>();

        healthSlider.value = totalHealth;
        // Set the initial health of the player.
        currentHealth = totalHealth;
        BarScript.totalHealthBar = totalHealth;
        BarScript.currentPlayerHealth = totalHealth;


    }

    private void Start()
    {

        //TODO: IF DEAD REMOVE THIS FUNCTION
        InvokeRepeating("RegenerationHealth", 0f, 8f);
    }

    void Update()
    {
        // If the player has just been damaged...
        if (damaged)
        {
            // ... set the colour of the damageImage to the flash colour.
            damageImage.color = flashColour;
        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        // Reset the damaged flag.
        damaged = false;
    }

    public void TakeCurrentHealthAfterLevelUp(int totalHp)
    {
        totalHealth = totalHp;
    }

    public void TakeDamage(float amount)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Reduce the current health by the damage amount.
        currentHealth -= amount;
        //Set image filler to current health

        // Set the health bar's value to the current health.
        healthSlider.value = currentHealth;

        // Play the hurt sound effect.
        /* playerAudio.Play()*/
        ;

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead)
        {
            // ... it should die.
            Death();
        }
        
    }


    public void RegenerationHealth()
    {
        if (currentHealth < totalHealth)
        {
            currentHealth += regenHealthPerSec;
            healthSlider.value += regenHealthPerSec;
            BarScript.currentPlayerHealth = healthSlider.value;
        }

    }

    void Death()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;

        // Turn off any remaining shooting effects.
        //playerShooting.DisableEffects();

        // Tell the animator that the player is dead.
        anim.SetTrigger("PlayerDead");

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        playerAudio.clip = deathClip;
        playerAudio.Play();

        // Turn off the movement and shooting scripts.
        playerMotor.enabled = false;
        //playerShooting.enabled = false;
    }
}
