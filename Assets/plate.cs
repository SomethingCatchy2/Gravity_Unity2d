using UnityEngine;

public class plate : MonoBehaviour
{
    // Public variable for the Rigidbody2D
    public Rigidbody2D targetRigidbody;

    // Variable to store the gravity scale
    public float storedGravityScale;

    public bool isColPlayer;

    void Start()
    {
        // If no Rigidbody2D is assigned, try to find one attached to this object
        if (targetRigidbody == null)
        {
            targetRigidbody = GetComponent<Rigidbody2D>();
        }

        // Store the current gravity scale of the Rigidbody2D
        storedGravityScale = targetRigidbody.gravityScale;

        isColPlayer = false;
    }

    void Update()
    {
    if(isColPlayer){
        targetRigidbody.gravityScale *= -1;
        isColPlayer = false;
        }
        
    }


        void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
        isColPlayer = true;
        }
    }
}
