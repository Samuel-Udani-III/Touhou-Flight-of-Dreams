using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveRangeX = 3f;    // Range for random X-axis movement
    public float moveRangeY = 2f;    // Range for random Y-axis movement
    public float moveInterval = 1f;  // Time interval between random movement changes
    private float nextMoveTime = 0f;
    private Vector3 direction;       // Direction of movement (for X and Y axes)

    public GameObject projectilePrefab; // Reference to the projectile prefab
    public float fireRate = 2f;        // Time between each shot
    private float nextFireTime = 0f;   // Timer to track when to fire again

    public float projectileSpeed = 10f; // Speed of the projectiles (adjustable)

    public int maxHitPoints = 30;  // Max hits before destruction, now editable in the Inspector
    private int currentHitPoints;  // Current HP of the enemy, initialized dynamically

    // Reference to the LevelProgression script
    public LevelProgression levelProgression;

    void Start()
    {
        // Initialize direction with random values for X and Y axes
        direction = new Vector3(Random.Range(-moveRangeX, moveRangeX), Random.Range(-moveRangeY, moveRangeY), 0);
        
        // Ensure that the levelProgression reference is assigned
        if (levelProgression == null)
        {
            levelProgression = FindObjectOfType<LevelProgression>(); // Finds the LevelProgression script in the scene
        }

        // Initialize current hit points to the max hit points at the start
        currentHitPoints = maxHitPoints;
    }

    void Update()
    {
        // Check if it's time to change the movement direction on both axes
        if (Time.time >= nextMoveTime)
        {
            // Randomize the direction for both X and Y axes
            direction = new Vector3(Random.Range(-moveRangeX, moveRangeX), Random.Range(-moveRangeY, moveRangeY), 0);
            // Set the next time the movement direction will change
            nextMoveTime = Time.time + moveInterval;
        }

        // Apply the random movement in both X and Y axes
        transform.Translate(direction * Time.deltaTime);

        // Fire projectiles at a certain interval
        if (Time.time >= nextFireTime)
        {
            FireProjectile();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FireProjectile()
    {
        // Fire two projectiles: one upwards (north) and one downwards (south)
        for (int i = 0; i < 2; i++)
        {
            // Instantiate the main projectile at the enemy's position
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Set velocity based on the projectile direction
                if (i == 0) // Upwards (north)
                {
                    rb.velocity = Vector2.up * projectileSpeed;  // Use the adjustable projectile speed
                }
                else if (i == 1) // Downwards (south)
                {
                    rb.velocity = Vector2.down * projectileSpeed;  // Use the adjustable projectile speed
                }
            }

            // Optionally, adjust the projectile's position to slightly offset them vertically
            if (i == 0) // North
            {
                projectile.transform.position += new Vector3(0, 0.5f, 0); // Adjust north (up)
            }
            else if (i == 1) // South
            {
                projectile.transform.position += new Vector3(0, -0.5f, 0); // Adjust south (down)
            }
        }
    }

    // Method to handle collisions with projectiles
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            // Decrease current HP when hit by a projectile
            currentHitPoints--;

            // Destroy the projectile
            Destroy(collision.gameObject);

            // Check if the current HP is 0 or below, indicating the enemy is defeated
            if (currentHitPoints <= 0)
            {
                // Destroy the enemy
                Destroy(gameObject);

                // Notify the LevelProgression script that an enemy has been defeated
                if (levelProgression != null)
                {
                    levelProgression.EnemyDefeated();
                }
            }
        }
    }
}
