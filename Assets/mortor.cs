using UnityEngine;

public class mortor : MonoBehaviour
{
    [Tooltip("Units per second. Negative for left.")]
    public float speed = 5f;

    void Update()
    {
        // Moves on the X axis only, frame-rate independent
        transform.Translate(speed * Time.deltaTime, 0f, 0f);
    }
}