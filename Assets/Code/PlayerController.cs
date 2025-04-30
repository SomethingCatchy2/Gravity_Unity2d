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
    public bool isDead;
    public Rigidbody2D rb;

    // Partical defines
    public ParticleSystem JumpPar;
    public ParticleSystem CheckpointPar;
    public ParticleSystem HasGravPar;
    public ParticleSystem HasHeartPar;
    public ParticleSystem HasHearttPar;
    public ParticleSystem HasJumpPar;
    public ParticleSystem FireWork1;
    public ParticleSystem DiededPar; 
    // define Sprite
    public SpriteRenderer JeffSprite;

    // Movement and physics parameters
    public float xSpeed = 7.5f;
    public float killcount = 0f;
    public float bounciness = 0.0f;
    public float jumpStrength = 5f;
    public static float portalcount = 0;

    // ✅ New: Checkpoint position
    private static Vector3 lastCheckpointPosition = new Vector3(-146.1f, 201.8f, 0);
    private static bool respawningFromCheckpoint = false;

    // Add these private variables to track previous states
    private bool wasGrounded = false;
    private bool wasHearted = false;
    private bool wasHeartted = false;
    private bool wasJumping = false;

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
        // Handle ground state changes
        if(isGrounded != wasGrounded) {
            Debug.Log("Ground state changed to: " + isGrounded);
            if(isGrounded) {
                if(HasGravPar != null) {
                    HasGravPar.Clear();  // Clear existing particles
                    HasGravPar.Play();   // Start fresh
                    Debug.Log("Ground particle playing");
                }
            } else {
                if(HasGravPar != null) HasGravPar.Stop();
            }
            wasGrounded = isGrounded;
        }

        // Handle heart state changes
        if(isHearted != wasHearted) {
            Debug.Log("Heart state changed to: " + isHearted);
            if(isHearted) {
                if(HasHeartPar != null) {
                    HasHeartPar.Clear();
                    HasHeartPar.Play();
                    Debug.Log("Heart particle playing");
                }
            } else {
                if(HasHeartPar != null) HasHeartPar.Stop();
            }
            wasHearted = isHearted;
        }

        // Handle heartt state changes
        if(isHeartted != wasHeartted) {
            Debug.Log("Heartt state changed to: " + isHeartted);
            if(isHeartted) {
                if(HasHearttPar != null) {
                    HasHearttPar.Clear();
                    HasHearttPar.Play();
                    Debug.Log("Heartt particle playing");
                }
            } else {
                if(HasHearttPar != null) HasHearttPar.Stop();
            }
            wasHeartted = isHeartted;
        }

        // Handle jump state changes
        bool isJumping = isJump || isJumpp;
        if(isJumping != wasJumping) {
            Debug.Log("Jump state changed to: " + isJumping);
            if(isJumping) {
                if(HasJumpPar != null) {
                    HasJumpPar.Clear();
                    HasJumpPar.Play();
                    Debug.Log("Jump particle playing");
                }
            } else {
                if(HasJumpPar != null) HasJumpPar.Stop();
            }
            wasJumping = isJumping;
        }

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

        if (Input.GetKeyDown(KeyCode.W) && isGrounded && !isDead || Input.GetKeyDown(KeyCode.UpArrow) && isGrounded && !isDead)
        {
            
            JumpPar.Play();
            rb.gravityScale *= -1;
            isGrounded = false;
           
            
        }

        if (isJump && Input.GetKeyDown(KeyCode.S) && !isDead || isJump && Input.GetKeyDown(KeyCode.DownArrow) && !isDead)
        {
            StartCoroutine(DelayAction(0.75f));
            HasJumpPar.Stop();
        }

        if (isJumpp && Input.GetKeyDown(KeyCode.S) && !isDead || isJumpp && Input.GetKeyDown(KeyCode.DownArrow) && !isDead)
        {
            StartCoroutine(DelayAction(0.75f));
            HasJumpPar.Stop();
        }
    }

    IEnumerator DelayAction(float delayTime)
    {
        isJumpp = false;
        isJump = false;
        rb.gravityScale *= -1;
        yield return new WaitForSeconds(delayTime);
        rb.gravityScale *= -1;
    }
      IEnumerator Die(float delayTime)
    {
                    
                    respawningFromCheckpoint = true;
                    JeffSprite.enabled = false;
                    DiededPar.Play();
                    yield return new WaitForSeconds(5);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Ground":
                isGrounded = true;
                isJump = false;
                HasGravPar.Play();
                
                break;

            case "Enemy":
                if (isHeartted)
                {
                    isHeartted = false;
                    HasHeartPar.Stop();
                }
                else if (isHearted)
                {
                    isHearted = false;
                    HasHeartPar.Stop();
                }
                else
                {

                      StartCoroutine(Die(5f));
                    
                }
                break;

          
            case "ice":
                isIce = true;
                break;

            case "jump":
               HasJumpPar.Play();
                isJump = true;
                isGrounded = true;
                break;

            case "jumpp":
            HasJumpPar.Play();
                isJumpp = true;
                isGrounded = true;
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

           
           

            case "toSelect":
                TeleportPlayerToSelect();
                break;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("check")) //Checkpoint col check + update.
        {
            Debug.Log("Trigger entered!");
            CheckpointPar.Play();
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
                    StartCoroutine(Die(5f));
                    // ✅ New: Set flag and reload scene to respawn at checkpoint
       
          
                }
        if (other.CompareTag("Heart") || other.CompareTag("Heartt"))
        {
            if (other.CompareTag("Heart"))
            
            {
            HasHeartPar.Play();
                isHearted = true;
            }
            if (other.CompareTag("Heartt"))
            {
                HasHeartPar.Play();
                isHeartted = true;
            }
            
        }
        if (other.CompareTag("Portal"))
        {
            portalcount += 1;
            TeleportPlayer();
            ResetPlayerState();

        }
if (other.CompareTag("Finish"))
        {
           FireWork1.Play();

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
