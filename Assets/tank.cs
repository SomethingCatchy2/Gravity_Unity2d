using UnityEngine;

public class BlockShooter : MonoBehaviour
{
    [Header("Shoot Settings")]
    public GameObject blockPrefab;
    public Transform target;
    public Transform shootPoint;       // NEW: Where the block is spawned from
    public float shootForce = 10f;
    public float shootInterval = 2f;

    private GameObject currentBlock;
    private float shootTimer;

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            ShootBlock();
            shootTimer = 0f;
        }
    }

    void ShootBlock()
    {
        // Delete the previously shot block
        if (currentBlock != null)
        {
            Destroy(currentBlock);
        }

        // Spawn a new block at the shoot point
        currentBlock = Instantiate(blockPrefab, shootPoint.position, Quaternion.identity);

        // Direction from shoot point to target
        Vector2 direction = (target.position - shootPoint.position).normalized;

        // Apply force to move toward the target
        Rigidbody2D rb = currentBlock.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(direction * shootForce, ForceMode2D.Impulse);
        }
    }
}