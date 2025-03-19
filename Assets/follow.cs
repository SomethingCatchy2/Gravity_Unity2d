
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject target; // Assign the object to follow in the inspector
    public float speed = 5f;  // Speed at which the enemy moves

    void Update()
    {
        if (target != null)
        {
            // Get current and target positions
            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = new Vector2(target.transform.position.x, currentPosition.y);
            
            // Move towards the target position at the given speed
            transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
        }
    }
}