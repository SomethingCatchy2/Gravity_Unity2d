using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fast : MonoBehaviour
{
    // Indicates whether the object is on the ground.
    private bool isGrounded;

    // Reference to the Rigidbody2D component for applying physics-based forces.
    private Rigidbody2D rb;

    // Optional: Reference to a Transform if you want to determine a target direction.
    private Transform whereGo;

    // Vector used for computing changes in direction or movement.
    private Vector3 change;

    // State variable to determine movement direction.
    public int shew = 0; 

    // Maximum velocity in the X axis (not currently used).
    private float maxVelX = 10;

    // Movement speed for the player.
    public float PlayerSpeed = 5;

    // X-axis speed (not currently used).
    public float xSpeed;

    // Start is called before the first frame update.
    void Start(){
        // Apply an initial force to move the object leftwards.
        GetComponent<Rigidbody2D>().AddForce(Vector2.left * 500f);
        // Set initial state.
        shew = 1;
    }

    // Update is called once per frame.
    void Update(){
        // Check if the object is grounded before applying further forces.
        if (isGrounded == true){ 
            // When in state 1, apply force to move right and toggle state.
            if(shew == 1){
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * 5000f);
                isGrounded = false; // Reset grounded flag.
                shew = 0;          // Toggle state.
            }
            // When in state 0, apply force to move left and toggle state.
            if(shew == 0){
                GetComponent<Rigidbody2D>().AddForce(Vector2.left * 5000f);
                isGrounded = false; // Reset grounded flag.
                shew = 1;         // Toggle state.
            }
        }
    }

    // Called when the collider enters a collision.
    void OnCollisionEnter2D(Collision2D col){
        // If the colliding object has the tag "Ground", set isGrounded to true.
        if(col.gameObject.tag == "Ground"){
            isGrounded = true;
        }
    }
}
