using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class move : MonoBehaviour
{
    private bool isGrounded;
    private Rigidbody2D rb;
    // OPTIONAL: include if you want to limit x velocity
    private Transform whereGo;
    private Vector3 change;
// public static float Range(float, float);
public int shew = 0; 
    private float maxVelX = 10;
 public float PlayerSpeed = 5;
    public float xSpeed;
		void Start(){
		GetComponent<Rigidbody2D>().AddForce(Vector2.left * 500f);
        shew = 1;
		}


		void Update(){
            
if (isGrounded == true){ 
if(shew==1){
    GetComponent<Rigidbody2D>().AddForce(Vector2.left * 500f);
		isGrounded = false;
        shew = 0;
        }
if(shew==0){
		GetComponent<Rigidbody2D>().AddForce(Vector2.right * 500f);
		isGrounded = false;
        shew = 1;
		}
}
	}






    void OnCollisionEnter2D(Collision2D col){

    

           if(col.gameObject.tag == "Ground"){
        isGrounded = true;
        }
    }
}