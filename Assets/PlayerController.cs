using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Player states
    public bool isGrounded;
    public bool isHearted;
    public bool isJump;
    public bool isFast;
    public bool isSlow;

    public Rigidbody2D rb;

    // Player movement variables
    public float xSpeed = 7.5f;
    public float bounciness = 0.0f;
    public float jumpStrength = 5f;
    private int rand1, rand2;

    void Start()
    {
        // Initialize player states
        isGrounded = false;
        isHearted = false;
        isJump = false;
        isFast = false;
        isSlow = false;
        rb = GetComponent<Rigidbody2D>();

        // Check if Rigidbody2D is attached
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing from this GameObject!");
        }
    }

    void Update()
    {
        // Adjust speed based on player state
        if (isFast)
            xSpeed = 30f;
        else if (isSlow)
            xSpeed = 2f;
        else
            xSpeed = 7.5f;

        // Adjust bounciness if on ice
        // Handle movement (left/right)
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * xSpeed, rb.linearVelocity.y);
        rb.linearVelocity *= (1 - bounciness);  // Apply bounciness to movement
        Debug.Log("Player moving with velocity: " + rb.linearVelocity);

        // Flip gravity when pressing 'W' if grounded
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.gravityScale *= -1;
            isGrounded = false;
            Debug.Log("Gravity flipped. New gravity scale: " + rb.gravityScale);
        }

        // Temporary gravity flip when pressing 'S' if can jump
        if (isJump && Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(DelayAction(0.75f));
        }
    }

    // Coroutine to temporarily flip gravity
    IEnumerator DelayAction(float delayTime)
    {
        rb.gravityScale *= -1;
        Debug.Log("Temporary gravity flip activated");
        yield return new WaitForSeconds(delayTime);
        rb.gravityScale *= -1;
        isJump = false;
        Debug.Log("Gravity restored");
    }

    // Handle collisions
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Collided with: " + col.gameObject.tag);

        switch (col.gameObject.tag)
        {
            case "Ground":
                isGrounded = true;
                Debug.Log("Player landed on the ground");
                break;

            case "jump":
                isJump = true;
                Debug.Log("Jump ability reset");
                break;

            case "Heart":
                isHearted = true;
                Debug.Log("Heart collected. Extra life granted.");
                break;

            case "Enemy":
                if (isHearted)
                {
                    isHearted = false;
                    Debug.Log("Hit by enemy but survived due to heart");
                }
                else
                {
                    Debug.Log("Hit by enemy. Reloading scene...");
                    StartCoroutine(ReloadScene());
                }
                break;

            case "Fast":
                isFast = true;
                isSlow = false;
                break;

            case "Slow":
                isSlow = true;
                isFast = false;
                break;

            case "Portal":
                Debug.Log("Entered Portal");
                TeleportPlayer();
                break;

            case "Portal1":
                Debug.Log("Entered Portal1");
                TeleportPlayer1();
                break;
        }
    }

    // Teleport to random location when entering Portal
    void TeleportPlayer()
    {
        rand1 = Random.Range(1, 6);
        ResetPlayerState();
        Debug.Log("Teleporting to random location: " + rand1);

        Vector3[] positions = {
            new Vector3(7.6f, -0.7f, 0),
            new Vector3(312.4f, -21.3f, 0),
            new Vector3(543.9f, -48f, 0),
            new Vector3(-461.5f,-31.4f,0),
            new Vector3(-745.4f, -22.7f, 0)
        };
        transform.position = positions[rand1 - 1];
    }

    // Teleport to random location when entering Portal1
    void TeleportPlayer1()
    {
        rand2 = Random.Range(1, 6);
        ResetPlayerState();
        Debug.Log("Teleporting via Portal1 to location: " + rand2);

        Vector3[] positions = {
            new Vector3(-789.7f,-128.7f,0),
            new Vector3(-361.1f,-163.1f,0),
            new Vector3(-36f,-169.7f,0),
            new Vector3(294f,-174f,0),
            new Vector3(654.5f,-174.2f,0),
        };
        transform.position = positions[rand2 - 1];
    }

    // Reset player state after teleportation
    void ResetPlayerState()
    {
        isJump = false;
        isFast = false;
        isSlow = false;
        rb.gravityScale = 1;

    }

    // Coroutine to reload the scene after a delay
    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Scene reloading...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
