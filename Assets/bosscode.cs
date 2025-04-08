using UnityEngine;

// This class makes an object follow a target on both X and Y axes
public class bosscode : MonoBehaviour
{
    // Reference to the GameObject that this object should follow
    public GameObject target; 

    // Movement speed in units per second
    public float speed = 5f;  

    // Update is called once per frame
    void Update()
    {
        // Only proceed if we have a valid target
        if (target != null)
        {
            // Store the current position of this object
            Vector2 currentPosition = transform.position;

            // Create target position with target's X and Y coordinates
            // This ensures movement happens on both X and Y axes
            Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);

            // Smoothly move towards the target position
            // MoveTowards ensures consistent movement speed regardless of distance
            // Time.deltaTime makes movement frame-rate independent
            transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
        }
    }
}
