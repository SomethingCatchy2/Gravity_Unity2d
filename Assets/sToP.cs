using UnityEngine;

public class sToP : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Option 1: Stop movement by setting velocity to zero (for Rigidbody-based movement)
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }

        // Option 2: Move the object back a bit (optional)
        // other.transform.position -= other.transform.forward * 0.5f;

        // Option 3: If using a custom movement script, disable it
        // other.GetComponent<PlayerMovement>().enabled = false;
    }
}