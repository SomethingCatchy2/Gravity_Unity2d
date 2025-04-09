using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // State flags for player conditions
    public bool isGrounded;
    public bool isHearted;
    public bool isHeartted;
    public bool isJump;
    public bool isJumpp;
    public bool isFast;
    public bool isSlow;
    public bool isIce;
    public Rigidbody2D rb;

    // Movement and physics parameters
    public float xSpeed = 7.5f;
    public float killcount = 0f;
    public float bounciness = 0.0f;
    public float jumpStrength = 5f;
    private static float portalcount = 0;

    // ✅ New: Checkpoint position
    private static Vector3 lastCheckpointPosition = new Vector3(-146.1f, 201.8f, 0);
    private static bool respawningFromCheckpoint = false;

    void Start()
    {
        isGrounded = false;
        isHearted = false;
        isHeartted = false;
        isJump = false;
        isJumpp = false;
        isFast = false;
        isSlow = false;
        isIce = false;

        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing from this GameObject!");
        }

        // ✅ New: If we're respawning, teleport to last checkpoint
        if (respawningFromCheckpoint)
        {
            transform.position = lastCheckpointPosition;
            respawningFromCheckpoint = false;
        }
    }

    void Update()
    {
        if (isGrounded)
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
            xSpeed = 3f;
        }
        else
        {
            xSpeed = 7.5f;
        }

        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * xSpeed, rb.linearVelocity.y);
        rb.linearVelocity *= (1 - bounciness);

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.gravityScale *= -1;
            isGrounded = false;
        }

        if (isJump && Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(DelayAction(0.75f));
        }

        if (isJumpp && Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(DelayAction(0.75f));
        }
    }

    IEnumerator DelayAction(float delayTime)
    {
        rb.gravityScale *= -1;
        yield return new WaitForSeconds(delayTime);
        rb.gravityScale *= -1;
        isJumpp = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Ground":
                isGrounded = true;
                isJump = false;
                break;

            case "Enemy":
                if (isHeartted)
                {
                    isHeartted = false;
                }
                else if (isHearted)
                {
                    isHearted = false;
                }
                else
                {
                    // ✅ New: Set flag and reload scene to respawn at checkpoint
                    respawningFromCheckpoint = true;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                break;

            case "Heart":
                isHearted = true;
                break;

            case "Heartt":
                isHeartted = true;
                break;

            case "ice":
                isIce = true;
                break;

            case "jump":
                isJump = true;
                break;

            case "jumpp":
                isJumpp = true;
                break;

            case "bossportal":
                transform.position = new Vector3(1252.1f, 18.38f, 0);
                break;

            case "bosskill":
                if (killcount == 3)
                {
                    transform.position = new Vector3(1137.22f, -85.44f, 0);
                }
                else
                {
                    killcount += 1;
                    Destroy(col.gameObject);
                }
                break;

            case "home":
                transform.position = new Vector3(-146.86f, 203.9f, 0);
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
                TeleportPlayer();
                portalcount += 1;
                ResetPlayerState();
                break;
           

            case "toSelect":
                TeleportPlayerToSelect();
                break;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("check"))
        {
            Debug.Log("Trigger entered!");
            lastCheckpointPosition = transform.position;
        }
        if (other.CompareTag("Enemy"))
                if (isHeartted)
                {
                    isHeartted = false;
                }
                else if (isHearted)
                {
                    isHearted = false;
                }
                else
                {
                    // ✅ New: Set flag and reload scene to respawn at checkpoint
                    respawningFromCheckpoint = true;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
    }

    void TeleportPlayer()
    {
        if (portalcount == 1)
        {
            transform.position = new Vector3(-469.329987f,-31.02f,0);
        }
        else if (portalcount == 2)
        {
            transform.position = new Vector3(-0.629999995f,-0.150000006f,0);
        }
        else if (portalcount == 3)
        {
            transform.position = new Vector3(309.6f,-21.4f,0);
        }
        else if (portalcount == 4)
        {
            transform.position = new Vector3(536.13f,-46.86f,0);
        }
        else if (portalcount == 5)
        {
            transform.position = new Vector3(-751.01f,-23.34f,0);
        }
        else if (portalcount == 6)
        {
            transform.position = new Vector3(-351.399994f,-168.899994f,0);
        }
        else if (portalcount == 7)
        {
            transform.position = new Vector3(-793.799988f,-135.300003f,0);
        }
        else if (portalcount == 8)
        {
            transform.position = new Vector3(655.0f,-177.899994f,0);
        }
        else if (portalcount == 9)
        {
            transform.position = new Vector3(294.0f,-172.880005f,0);
        }
        else if (portalcount == 10)
        {
            transform.position = new Vector3(-31.8999996f,-170.300003f,0);
        }
        else if (portalcount == 11)
        {
            transform.position = new Vector3(-677.200012f,-314.100006f,0);
        }
        else if (portalcount == 12)
        {
            transform.position = new Vector3(-377.0f,-295.0f,0);
        }
        else if (portalcount == 13)
        {
            transform.position = new Vector3(-790.5f,-444.100006f,0);
        }
        else if (portalcount == 14)
        {
            transform.position = new Vector3(-376.880005f,-441.880005f,0);
        }
        else if (portalcount == 15)
        {
            transform.position = new Vector3(-61.2000008f,-502.399994f,0);
        }
        else if (portalcount == 16)
        {
            transform.position = new Vector3(278.98999f,-438.890015f,0);
        }
        else if (portalcount == 17)
        {
            transform.position = new Vector3(614.849976f,-464.649994f,0);
        }
        else if (portalcount == 18) //boss leavle
        {
            transform.position = new Vector3(939.47998f,19.2399998f,0);
        }
    }

    void TeleportPlayerToSelect()
    {
        ResetPlayerState();
        Debug.Log("Teleporting to Select Level");
        transform.position = new Vector3(100, 70, 0);
    }

    void ResetPlayerState()
    {
        isJump = false;
        isFast = false;
        isSlow = false;
        isIce = false;
        isJumpp = false;
        isHearted = false;
        isHeartted = false;

        rb.gravityScale = 1;
    }
}
