using UnityEngine;

// This class makes an object follow a target on the Y-axis only
public class Ypos : MonoBehaviour
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

            // Create target position with same X but target's Y coordinate
            Vector2 targetPosition = new Vector2(currentPosition.x, target.transform.position.y);
            
            // Smoothly move towards the target position on Y-axis only
            transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
        }
    }
}