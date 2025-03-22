using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class move : MonoBehaviour
{
    // Component references
    private Rigidbody2D rb;
    private Transform whereGo;

    // Movement variables
    private bool isGrounded;
    private Vector3 change;
    private float maxVelX = 10;
    public float PlayerSpeed = 5;
    public float xSpeed;

    // State tracking
    public int shew = 0;  // Controls direction switching (0 = right, 1 = left)

    void Start()
    {
        // Initial force to the left
        GetComponent<Rigidbody2D>().AddForce(Vector2.left * 500f);
        shew = 1;
    }

    void Update()
    {
        // Only apply forces when grounded
        if (isGrounded)
        { 
            if(shew == 1)
            {
                // Switch direction to left
                GetComponent<Rigidbody2D>().AddForce(Vector2.left * 500f);
                isGrounded = false;
                shew = 0;
            }
            if(shew == 0)
            {
                // Switch direction to right
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * 500f);
                isGrounded = false;
                shew = 1;
            }
        }
    }

    // Detect ground collision
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
}