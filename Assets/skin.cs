using UnityEngine;

public class skin : MonoBehaviour {

    public GameObject target; // the enemy's target
    public float moveSpeed = 5; // move speed
    public float rotationSpeed = 5; // speed of turning
    private Rigidbody rb;
    private Vector3 mytransform; // Declare without initialization

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mytransform = target.transform.position; // Initialize it in Start
    }

    void Update()
    {
        if (target != null) // Ensure target exists before accessing its position
        {
            // Rotate to look at the player
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(target.transform.position - transform.position), 
                rotationSpeed * Time.deltaTime);

            // Move towards the player
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
    }
}
