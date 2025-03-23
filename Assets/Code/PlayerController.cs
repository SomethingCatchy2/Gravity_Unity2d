using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the player character's movement, abilities, and interactions
/// </summary>
public class PlayerController : MonoBehaviour
{
    // State flags for player conditions
    public bool isGrounded;    // Indicates if player is touching ground
    public bool isHearted;     // Indicates if player has heart protection
    public bool isJump;        // Indicates if special jump is available
    public bool isFast;        // Indicates if speed boost is active
    public bool isSlow;        // Indicates if speed reduction is active
    public bool isIce;         // Indicates if on ice surface
    public Rigidbody2D rb;     // Reference to the player's Rigidbody2D component

    // Movement and physics parameters
    public float xSpeed = 7.5f;        // Base horizontal movement speed
    public float bounciness = 0.0f;    // Bounce factor
    public float jumpStrength = 5f;    // Jump force strength
    private int rand1, rand2;          // Random values for portal teleportation

    void Start()
    {
        // Initialize all state flags to false
        isGrounded = false;
        isHearted = false;
        isJump = false;
        isFast = false;
        isSlow = false;
        isIce = false;
        
        // Get reference to the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing from this GameObject!");
        }
    }

    void Update()
    {
        // Handle speed modifications based on power-ups
        if (isIce)
        {
            isFast = false;
            isSlow = false;
            xSpeed = 7.5f;
        }
        else if (isFast)
        {
            xSpeed = 20f;
        }
        else if (isSlow)
        {
            xSpeed = 2f;
        }
        else
        {
            xSpeed = 7.5f;
        }

        // Handle horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * xSpeed, rb.linearVelocity.y);
        rb.linearVelocity *= (1 - bounciness);
        Debug.Log("Player moving with velocity: " + rb.linearVelocity);

        // Handle gravity flip with W key
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.gravityScale *= -1;
            isGrounded = false;
            Debug.Log("Gravity flipped. New gravity scale: " + rb.gravityScale);
        }

        // Handle temporary gravity flip with S key
        if (isJump && Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(DelayAction(0.75f));
        }
    }

    /// <summary>
    /// Temporarily flips gravity for a specified duration
    /// </summary>
    IEnumerator DelayAction(float delayTime)
    {
        rb.gravityScale *= -1;
        Debug.Log("Temporary gravity flip activated");
        yield return new WaitForSeconds(delayTime);
        rb.gravityScale *= -1;
        isJump = false;
        Debug.Log("Gravity restored");
    }

    /// <summary>
    /// Handles all collision events with tagged objects
    /// </summary>
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Collided with: " + col.gameObject.tag);

        switch (col.gameObject.tag)
        {
            case "Ground":
                // Reset movement states and mark as grounded when landing
                isGrounded = true;
                isSlow = false;
                isFast = false;
                isIce = false;
                isJump = false;
                Debug.Log("Player landed on the ground");
                break;

            case "jump":
                // Enable special jump ability when collecting jump powerup
                isJump = true;
                isFast = false;
                isSlow = false;
                isGrounded = true;
                isIce = false;
                Debug.Log("Jump ability reset");
                break;

            case "Heart":
                // Grant extra life protection
                isHearted = true;
                Debug.Log("Heart collected. Extra life granted.");
                break;

            case "ice":
                // Enable ice physics and disable other movement modifiers
                isIce = true;
                isFast = false;
                isSlow = false;
                Debug.Log("Player is on ice");
                isJump = false;
                isGrounded = false;
                break;

            case "Enemy":
                // Handle enemy collision with heart protection logic
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
                // Enable speed boost and disable slow effect
                isFast = true;
                isSlow = false;
                isJump = false;
                isGrounded = false;
                isIce = false;
                break;

            case "Slow":
                // Enable speed reduction and disable speed boost
                isSlow = true;
                isFast = false;
                isJump = false;
                isGrounded = false;
                isIce = false;
                
                break;

            case "Portal":
                // Trigger first portal teleportation system
                Debug.Log("Entered Portal");
                TeleportPlayer();
                break;

            case "Portal1":
                // Trigger second portal teleportation system
                Debug.Log("Entered Portal1");
                TeleportPlayer1();
                break;
            case "toSelect":
                // Trigger second portal teleportation system
                Debug.Log("Entered Portal1");
                TeleportPlayerToSelect();
                break;
        }
    }

    /// <summary>
    /// Teleports player to random location using first portal system
    /// </summary>
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

    /// <summary>
    /// Teleports player to random location using second portal system
    /// </summary>
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
            new Vector3(654.5f,-174.2f,0)
        };
        transform.position = positions[rand2 - 1];
    }
    void TeleportPlayerToSelect()
    {
        ResetPlayerState();
        Debug.Log("Teleporting to Select Level");
        transform.position = new Vector3(100, 70, 0);
    }
    /// <summary>
    /// Resets all player state flags and gravity
    /// </summary>
    void ResetPlayerState()
    {
        isJump = false;
        isFast = false;
        isSlow = false;
        isIce = false;
        isJump = false;
        
        rb.gravityScale = 1;
    }

    /// <summary>
    /// Reloads the current scene after a short delay
    /// </summary>
        IEnumerator ReloadScene()
        {
            ResetPlayerState();
            yield return new WaitForSeconds(0.5f);
            Debug.Log("Scene reloading...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
