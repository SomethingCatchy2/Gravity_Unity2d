using UnityEngine;

// This class makes an object follow a target on the x-axis only
public class Follow : MonoBehaviour
{
    // Reference to the GameObject that this object should follow
    public GameObject target; 

    // Movement speed in units per second
    public float speed;  

    // Update is called once per frame
    void Update()
    {
        // Only proceed if we have a valid target
        if (target != null)
        {
            // Store the current position of this object
            Vector2 currentPosition = transform.position;

            // Create target position with same Y but target's X coordinate
            // This ensures movement only happens on X axis
            Vector2 targetPosition = new Vector2(target.transform.position.x, currentPosition.y);
            
            // Smoothly move towards the target position
            // MoveTowards ensures consistent movement speed regardless of distance
            // Time.deltaTime makes movement frame-rate independent
            transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
        }
    }
}