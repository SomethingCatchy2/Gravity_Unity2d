using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


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
    public bool flying;

    // Particle defines
    public ParticleSystem JumpPar;
    public ParticleSystem CheckpointPar;
    public ParticleSystem HasGravPar;
    public ParticleSystem HasHeartPar;
    public ParticleSystem HasHearttPar;
    public ParticleSystem HasJumpPar;
    public ParticleSystem FireWork1;
    public ParticleSystem DiededPar; 

    // Sprite
    public SpriteRenderer JeffSprite;

public TMPro.TextMeshProUGUI statusText;



    // Movement and physics parameters
    public float xSpeed = 7.5f;
    public float killcount = 0f;
    public float bounciness = 0.0f;
    public float jumpStrength = 5f;
    public static float portalcount = 0;

    // —— NEW: Multiple followers and lock flag ——
    public GameObject[] followers;
    public static bool areFollowersUnlocked = false;

    // ✅ Checkpoint position
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
        flying = false;

        if (isDead)
        return;

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("Rigidbody2D component is missing!");

        // If we're respawning, teleport player + followers
        if (respawningFromCheckpoint)
        {
            transform.position = lastCheckpointPosition;

        // Only teleport followers if they are unlocked
            if (areFollowersUnlocked)
            {
                if (followers == null || followers.Length == 0)
                    followers = GameObject.FindGameObjectsWithTag("Follower");

                foreach (GameObject f in followers)
                    if (f != null)
                        f.GetComponent<skin>()?.TeleportTo(lastCheckpointPosition);
            }

            respawningFromCheckpoint = false;
        }

    // Only lock followers if they haven't been unlocked yet
        if (!areFollowersUnlocked)
        {
            if (followers == null || followers.Length == 0)
                followers = GameObject.FindGameObjectsWithTag("Follower");

        foreach (GameObject f in followers)
            if (f != null)
                f.GetComponent<skin>()?.SetFollowStatus(false);
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


        if (isFast)
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

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded && !isDead)
        {
            
            JumpPar.Play();
            rb.gravityScale *= -1;
            isGrounded = false;
        }

        if ((isJump && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))) ||
            (isJumpp && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))))
        {
            StartCoroutine(DelayAction(0.75f));
            HasJumpPar.Stop();
        }

        if (flying)
        {
                statusText.text = "flying?";
        }
        else if (portalcount == 1)
        {
                statusText.text = "pilot";
        }
        else if (portalcount == 2)
        {
                statusText.text = "the start";
        }
        else if (portalcount == 3)
        {
                statusText.text = "puzzle";
        }
        else if (portalcount == 4)
        {
                statusText.text = "yellow";
        }
        else if (portalcount == 5)
        {
                statusText.text = "cars";
        }
        else if (portalcount == 6)
        {
                statusText.text = "jump";
        }
        else if (portalcount == 7)
        {
                statusText.text = "fast slow";
        }
        else if (portalcount == 8)
        {
                statusText.text = "heart";
        }
        else if (portalcount == 9)
        {
                statusText.text = "bounce";
        }
        else if (portalcount == 10)
        {
                statusText.text = "trick";
        }
        else if (portalcount == 11)
        {
                statusText.text = "white";
        }
        else if (portalcount == 12)
        {
                statusText.text = "stop";
        }
        else if (portalcount == 13)
        {
                statusText.text = "british";
        }
        else if (portalcount == 14)
        {
                statusText.text = "up and right";
        }
        else if (portalcount == 15)
        {
                statusText.text = "mortor";
        }
        else if (portalcount == 16)
        {
                statusText.text = "can you follow instructions?";
        }
        else if (portalcount == 17)
        {
                statusText.text = "time save";
        }
        else if (portalcount == 18)
        {
                statusText.text = "not yet...";
        }
        else if (portalcount == 19)
        {
                statusText.text = "factory";
        }
        else if (portalcount == 20)
        {
                statusText.text = "yellow 2";
        }
        else if (portalcount == 21)
        {
                statusText.text = "deadlly elevators";
        }
        else if (portalcount == 22)
        {
                statusText.text = "pink bounce";
        }
        else if (portalcount == 23)
        {
                statusText.text = "time save 2";
        }
        else if (portalcount == 24)
        {
                statusText.text = "chase";
        }
        else if (portalcount == 25)
        {
                statusText.text = "fast";
        }
        else if (portalcount == 26)
        {
                statusText.text = "fast ceiling";
        }
        else if (portalcount == 27)
        {
                statusText.text = "time save 3";
        }
        else if (portalcount == 28)
        {
                statusText.text = "small";
        }
        else if (portalcount == 29)
        {
                statusText.text = "BIG";
        }
        else if (portalcount == 30)
        {
                statusText.text = "some of all";
        }
        else if (portalcount == 31)
        {
                statusText.text = "jump 2";
        }
        else if (portalcount == 32)
        {
                statusText.text = "bounce jump";
        }
        else if (portalcount == 33)
        {
                statusText.text = "wise";
        }
        else if (portalcount == 34)
        {
                statusText.text = "acrobatics";
        }
        else if (portalcount == 35)
        {
                statusText.text = "car flip";
        }
        else if (portalcount == 36)
        {
                statusText.text = "elevators";
        }
        else if (portalcount == 37)
        {
                statusText.text = "jump mortor";
        }
        else if (portalcount == 38)
        {
                statusText.text = "stop 2";
        }
        else if (portalcount == 39)
        {
                statusText.text = "pink";
        }
        else if (portalcount == 40)
        {
                statusText.text = "up down";
        }
        else if (portalcount == 41)
        {
                statusText.text = "boss";
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
        // Set death flag
        isDead = true;

        // Freeze physics completely
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;


        // Continue with existing death sequence
        respawningFromCheckpoint = true;
        JeffSprite.enabled = false;
        DiededPar.Play();
        yield return new WaitForSeconds(2);
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
                isFast = false;
                isSlow = false;
                break;

            case "Enemy":
                if (isHeartted)
                {
                    isHeartted = false;
                    HasHearttPar.Stop();  // Fixed: Stop the correct particle effect
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
                isFast = false;
                isSlow = false;
                break;

            case "jumpp":
                HasJumpPar.Play();
                isJumpp = true;
                isGrounded = true;
                isFast = false;
                isSlow = false;
                break;

            case "bossportal":
                transform.position = new Vector3(1252.1f, 18.38f, 0);
                break;

            case "bosskill":
                if (killcount == 3)
                {
                    transform.position = new Vector3(1115.5f,-32.7999992f,0);
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
        if (other.CompareTag("check")) // Checkpoint col check + update.
        {
            Debug.Log("Trigger entered!");
            CheckpointPar.Play();
            lastCheckpointPosition = transform.position;
        }

        if (other.CompareTag("Enemy"))
        {
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
            }
        }

        if (other.CompareTag("slowgrav"))
        {
            Debug.Log("Entered gravity zone!");
            rb.gravityScale = 0.5f;
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
                HasHearttPar.Play();  // Fixed: Play the correct particle effect
                isHeartted = true;
            }
        }

        if (other.CompareTag("Portal"))
        {
            portalcount += 1;
            TeleportPlayer();
            flying = false;

    // —— NEW: teleport followers too ——
            if (areFollowersUnlocked)
            {
                Vector3 newPos = transform.position;
                if (followers == null || followers.Length == 0)
                    followers = GameObject.FindGameObjectsWithTag("Follower");
                foreach (GameObject f in followers)
                    f.GetComponent<skin>()?.TeleportTo(newPos);
            }

            ResetPlayerState();
        }

        if (other.CompareTag("Ground"))
        {
            isGrounded = true;
            isJump = false;
            HasGravPar.Play();
            isFast = false;
            isSlow = false;
        }

        if (other.CompareTag("jump"))
        {
            HasJumpPar.Play();
            isJump = true;
            isGrounded = true;
            isFast = false;
            isSlow = false;
        }

        if (other.CompareTag("jumpp"))
        {
            HasJumpPar.Play();
            isJumpp = true;
            isGrounded = true;
            isFast = false;
            isSlow = false;
        }

        if (other.CompareTag("bonus"))
        {
            flying = true;
        }

        if (other.CompareTag("Fast"))
        {
            isFast = true;
            isSlow = false;
        }

        if (other.CompareTag("Finish"))
        {
            FireWork1.Play();
        }

        // —— NEW: Unlock followers on “unlock” trigger ——
        if (other.CompareTag("unlock"))
        {
            areFollowersUnlocked = true;
            Debug.Log("Followers unlocked!");
            foreach (GameObject f in followers)
                f.GetComponent<skin>()?.SetFollowStatus(true);
        }
    }

    void TeleportPlayer()
    {
        if (portalcount == 1)
        {
            transform.position = new Vector3(-469.329987f, -31.02f, 0);
        }
        else if (portalcount == 2)
        {
            transform.position = new Vector3(-0.629999995f, -0.150000006f, 0);
        }
        else if (portalcount == 3)
        {
            transform.position = new Vector3(310.720001f,-22.289999f,0);
        }
        else if (portalcount == 4)
        {
            transform.position = new Vector3(536.13f, -46.86f, 0);
        }
        else if (portalcount == 5)
        {
            transform.position = new Vector3(-751.01f, -23.34f, 0);
        }
        else if (portalcount == 6)
        {
            transform.position = new Vector3(-351.399994f, -168.899994f, 0);
        }
        else if (portalcount == 7)
        {
            transform.position = new Vector3(-793.799988f, -135.300003f, 0);
        }
        else if (portalcount == 8)
        {
            transform.position = new Vector3(714.640015f, -177.860001f, 0);
        }
        else if (portalcount == 9)
        {
            transform.position = new Vector3(294.0f, -172.880005f, 0);
        }
        else if (portalcount == 10)
        {
            transform.position = new Vector3(-31.8999996f, -170.300003f, 0);
        }
        else if (portalcount == 11)
        {
            transform.position = new Vector3(-677.200012f, -314.100006f, 0);
        }
        else if (portalcount == 12)
        {
            transform.position = new Vector3(-377.0f, -295.0f, 0);
        }
        else if (portalcount == 13)
        {
            transform.position = new Vector3(-790.5f, -444.100006f, 0);
        }
        else if (portalcount == 14)
        {
            transform.position = new Vector3(-33.0200005f,-302.160004f,0);
        }
        else if (portalcount == 15)
        {
            transform.position = new Vector3(270.889984f,-295.559998f,0);
        }
        else if (portalcount == 16)
        {
            transform.position = new Vector3(562f,-286f,0);
        }
        else if (portalcount == 17)
        {
            transform.position = new Vector3(-376.880005f,-441.880005f,0);
        }
        else if (portalcount == 18)
        {
            transform.position = new Vector3(-61.2000008f,-502.399994f,0);
        }
        else if (portalcount == 19)
        {
            transform.position = new Vector3(288f,-464.600006f,0);
        }
        else if (portalcount == 20)
        {
            transform.position = new Vector3(585.960022f,-458.959991f,0);
        }
        else if (portalcount == 21)
        {
            transform.position = new Vector3(-776.400024f,-604.200012f,0);
        }
        else if (portalcount == 22)
        {
            transform.position = new Vector3(-368.149994f,-592.820007f,0);
        }
        else if (portalcount == 23)
        {
            transform.position = new Vector3(-27.0599995f,-591.354614f,0);
        }
        else if (portalcount == 24)
        {
            transform.position = new Vector3(277f,-588.5f,0);
        }
        else if (portalcount == 25)
        {
            transform.position = new Vector3(512.5f,-590.5f,0);
        }
        else if (portalcount == 26)
        {
            transform.position = new Vector3(-763.900024f,-746.130005f,0);
        }
        else if (portalcount == 27)
        {
            transform.position = new Vector3(-373.48999f,-732.150024f,0);
        }
        else if (portalcount == 28)
        {
            transform.position = new Vector3(-59.1599998f,-742.409973f,0);
        }
        else if (portalcount == 29)
        {
            transform.position = new Vector3(209.699997f,-798.900024f,0);
        }
        else if (portalcount == 30)
        {
            transform.position = new Vector3(541.163025f,-772.27063f,0);
        }
        else if (portalcount == 31)
        {
            transform.position = new Vector3(-802.51001f,-910.080017f,0);
        }
        else if (portalcount == 32)
        {
            transform.position = new Vector3(-465.299988f,-955.817383f,0);
        }
        else if (portalcount == 33)
        {
            transform.position = new Vector3(-145.429993f,-838.697388f,0);
        }
        else if (portalcount == 34)
        {
            transform.position = new Vector3(188.520004f,-904.367432f,0);
        }
        else if (portalcount == 35)
        {
            transform.position = new Vector3(530.299988f,-863.599976f,0);
        }
        else if (portalcount == 36)
        {
            transform.position = new Vector3(-793.810059f,-995.122803f,0);
        }
        else if (portalcount == 37)
        {
            transform.position = new Vector3(-33.0200005f,-302.160004f,0);
        }
        else if (portalcount == 38)
        {
            transform.position = new Vector3(-78.210022f,-1020.91187f,0);
        }
        else if (portalcount == 39)
        {
            transform.position = new Vector3(280.429962f,-1047.50818f,0);
        }
        else if (portalcount == 40)
        {
            transform.position = new Vector3(608.5f,-1035.86804f,0);
        }
        else if (portalcount == 41) 
        {
            transform.position = new Vector3(939.47998f, 19.2399998f, 0);
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
