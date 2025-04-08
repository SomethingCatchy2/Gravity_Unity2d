using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class movingEnemy : MonoBehaviour
{
    // Flag to check if the enemy is touching the ground
    private bool isGrounded;
    // Reference to the Rigidbody2D component
    private Rigidbody2D rb;
    // Transform reference (unused currently)
    private Transform whereGo;
    // Vector3 for movement changes (unused currently)
    private Vector3 change;

    // Maximum velocity in X direction (unused currently)
    private float maxVelX = 10;

    // Movement speed in X direction
    public float xSpeed;
    // Jump force strength
    public float jumpStrength;
    // Random value variable
    public float rand1;
    public float rand2;

    void Start()
    {
        // Initialize variables
        isGrounded = false;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // If enemy touches ground, reverse gravity and reset grounded state
        if (isGrounded == true)
        {
            rb.gravityScale = rb.gravityScale * -1f;
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Set isGrounded to true when colliding with objects tagged as "Ground"
        if (col.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
}
