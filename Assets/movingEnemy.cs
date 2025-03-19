using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class movingEnemy : MonoBehaviour
{
    private bool isGrounded;
    private Rigidbody2D rb;
    // OPTIONAL: include if you want to limit x velocity
    private Transform whereGo;
    private Vector3 change;
// public static float Range(float, float);

    private float maxVelX = 10;

    public float xSpeed;
    public float jumpStrength;
    public float rand1;
    
    
        void Start()
    {
        isGrounded = false;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {

     
        if (isGrounded==true) {
            rb.gravityScale = rb.gravityScale *-1f;
        isGrounded = false;
       
        }

       
    }


    void OnCollisionEnter2D(Collision2D col)
    {
                if(col.gameObject.tag == "Ground"){
        isGrounded = true;
        }

                        if(col.gameObject.tag == "Player"){
        isGrounded = true;
        }







    }

   }
    
//        void Start()
//     {
//         isGrounded = false;
//         rb = GetComponent<Rigidbody2D>();
//     }
    
    
//         void FixedUpdate(){
//         if (isGrounded==true) {
//             rb.gravityScale = rb.gravityScale *-1;
//         isGrounded = false;
// }
// }
//     void OnCollisionEnter2D(Collision2D col)

//            if(col.gameObject.tag == "Ground"){
//         isGrounded = true;
//         }
    

