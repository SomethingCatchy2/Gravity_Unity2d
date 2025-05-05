using UnityEngine;

public class skin : MonoBehaviour
{
    public GameObject target; // the enemy's target
    public float moveSpeed = 5; // move speed
    public float rotationSpeed = 5; // speed of turning

    private Rigidbody rb;
    private bool isFollowing = true;  // —— NEW flag

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isFollowing || target == null)
            return;

        // Rotate to look at the player
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            Quaternion.LookRotation(target.transform.position - transform.position), 
            rotationSpeed * Time.deltaTime);

        // Move towards the player
        transform.position += transform.forward * Time.deltaTime * moveSpeed;
    }

    // —— NEW: Called by PlayerController to lock/unlock following
    public void SetFollowStatus(bool canFollow)
    {
        isFollowing = canFollow;
    }

    // Called by PlayerController to teleport follower
    public void TeleportTo(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}
