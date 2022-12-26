using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glidingtest : MonoBehaviour
{
    // The Rigidbody component attached to the character
    private Rigidbody rb;
    public GameObject g;

    // The Animator component attached to the character
    private Animator anim;

    // A flag to track whether the character is currently gliding
    private bool isGliding = false;

    // The force to apply to the character when gliding
    public float glideForce = 1f;

    // The duration of the gliding animation
    public float glideAnimationDuration = 2.0f;

    void Start()
    {
        // Get the Rigidbody and Animator components
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the player is pressing the button to start gliding
        if (Input.GetKeyDown("r"))
        {
            // Start gliding
            Debug.Log("I am in gliding ");  
            StartGliding();
        }
        // Check if the player has released the button to stop gliding
        else 
        {
            // Stop gliding
            isGliding = false;
        }

        // If the character is currently gliding
        if (isGliding)
        {
            // Apply a downward force to simulate gliding
            g.GetComponent<Transform>().Translate(Vector3.up);


        }
    }

    // A method to start gliding
    void StartGliding()
    {
        // Set the isGliding flag to true
        isGliding = true;

/*        // Trigger the gliding animation
        anim.SetTrigger("StartGliding");

        // Set the animation to play for a certain duration
        anim.SetFloat("GlideDuration", glideAnimationDuration);*/
    }

    // A method to stop gliding
    void StopGliding()
    {
        // Set the isGliding flag to false
        isGliding = false;

/*        // Trigger the gliding animation
        anim.SetTrigger("StopGliding");*/
    }
}
