using System.Collections;
using UnityEngine;
using TMPro;  // Import the TextMeshPro namespace

public class Player : MonoBehaviour
{
    public float playerSpeed = 5f;      // Normal player movement speed
    public float recoverySpeed = 3f;    // Speed of player recovery movement
    private Rigidbody2D rb;
    private Vector2 playerDirection;
    private bool isHit = false;
    private float originalDrag;         // Store the original drag value for later restoration

    // Projectile variables
    public GameObject projectilePrefab;  // Reference to the projectile prefab
    public float projectileSpeed = 10f;  // Speed at which the projectile moves (faster than the player)
    public Transform shootPoint;        // Point from which the projectile will be shot

    // Health System
    public int maxHealth = 100;         // Max health value
    private int currentHealth;          // Current health value
    public TMP_Text healthText;         // Reference to the TextMeshPro Text UI element for displaying health

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalDrag = rb.drag; // Store the original drag value for later restoration
        currentHealth = maxHealth; // Initialize health to max health
        UpdateHealthText();  // Update health display at the start
    }

    // Update is called once per frame
    void Update()
    {
        // Get input for vertical and horizontal movement
        float directionX = Input.GetAxisRaw("Horizontal");  // Left/Right (A/D or Arrow keys)
        float directionY = Input.GetAxisRaw("Vertical");    // Up/Down (W/S or Arrow keys)

        // If the player is not hit, handle movement normally
        if (!isHit)
        {
            playerDirection = new Vector2(directionX, directionY).normalized; // Normalize for consistent speed
        }
        else
        {
            // During recovery, allow movement input but move at recovery speed
            playerDirection = new Vector2(directionX, directionY).normalized; // Normalize input direction
        }

        // Handle projectile shooting input (changed to spacebar)
        if (Input.GetKeyDown(KeyCode.Space))  // Spacebar for shooting
        {
            ShootProjectile();  // Call the function to shoot the projectile
        }
    }

    void FixedUpdate()
    {
        // If the player is hit, apply recovery movement
        if (isHit)
        {
            // Apply the recovery speed for both horizontal and vertical directions
            rb.velocity = new Vector2(playerDirection.x * recoverySpeed, playerDirection.y * recoverySpeed);
        }
        else
        {
            // Apply normal movement velocity when the player is not hit
            rb.velocity = new Vector2(playerDirection.x * playerSpeed, playerDirection.y * playerSpeed);
        }
    }

    // This function is called when the player is hit by a projectile
    public void OnHitByProjectile(int damage)
    {
        // Apply damage to the player's health
        currentHealth -= damage;

        // Ensure health doesn't drop below zero
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // Update the health UI after taking damage
        UpdateHealthText();

        // Temporarily stop any velocity to avoid unwanted movement during the hit phase
        rb.velocity = Vector2.zero;

        // Temporarily disable the player's collider to prevent further interactions
        Collider2D playerCollider = GetComponent<Collider2D>();
        playerCollider.enabled = false;

        // Disable external forces during recovery (apply drag to prevent being pushed)
        rb.drag = 1000f;  // High drag to freeze movement caused by external forces (like the projectile)
        rb.angularDrag = 1000f;  // Prevent any rotational forces (optional)

        // Disable movement while the player is hit
        isHit = true;

        // Check if health has reached zero and destroy the player if so
        if (currentHealth == 0)
        {
            DestroyPlayer(); // Destroy the player when health reaches zero
        }
        else
        {
            // Start the coroutine to reset player after a short recovery delay
            StartCoroutine(ResetPlayerAfterHit(playerCollider));
        }
    }

    // Coroutine to reset the player to allow movement again after the recovery phase
    private IEnumerator ResetPlayerAfterHit(Collider2D playerCollider)
    {
        // Wait for a short recovery time (e.g., 0.1 seconds)
        yield return new WaitForSeconds(0.1f);

        // Re-enable the player's collider after recovery
        playerCollider.enabled = true;

        // Reset the drag to its original value after recovery
        rb.drag = originalDrag;
        rb.angularDrag = originalDrag; // Reset angular drag if necessary

        // After the delay, allow player to move again normally
        isHit = false;
    }

    // Function to destroy the player when health reaches zero
    private void DestroyPlayer()
    {
        // Add any destruction effects here (e.g., animations, sounds)
        Debug.Log("Player Destroyed!");

        // Destroy the player GameObject
        Destroy(gameObject);
    }

    // Optional: Handle collisions with borders to prevent movement through them
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collides with an object tagged as "Border"
        if (collision.gameObject.CompareTag("Border"))
        {
            // Stop horizontal movement when hitting a border (this ensures the player is stopped at the border)
            rb.velocity = new Vector2(0, rb.velocity.y); // Prevent horizontal movement (but keep vertical)
        }
    }

    // Function to shoot a projectile
    private void ShootProjectile()
    {
        if (projectilePrefab != null && shootPoint != null)
        {
            // Instantiate the projectile at the shoot point
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

            // Log to check if it's being instantiated correctly
            Debug.Log("Projectile shot at position: " + shootPoint.position);

            // Ensure the Z position of the projectile is set to 0 (to stay in front of the background)
            projectile.transform.position = new Vector3(projectile.transform.position.x, projectile.transform.position.y, 0f);

            // Get the Rigidbody2D of the projectile and set its velocity
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            if (projectileRb != null)
            {
                // Use the forward direction of the player to shoot the projectile
                Vector2 shootingDirection = shootPoint.right;  // Shoot in the direction the player is facing

                // Ensure the projectile moves faster than the player by adding to the speed
                projectileRb.velocity = shootingDirection * projectileSpeed;

                // Optional: Add a visual debug line (shows the path of the projectile in the Scene view)
                Debug.DrawLine(shootPoint.position, shootPoint.position + new Vector3(shootingDirection.x, shootingDirection.y) * 10f, Color.red, 2f);
            }

            // Make sure the projectile stays in front of the background by adjusting the sorting layer
            SpriteRenderer projectileSprite = projectile.GetComponent<SpriteRenderer>();
            if (projectileSprite != null)
            {
                // Set the sorting layer of the projectile to be in front
                projectileSprite.sortingLayerName = "Foreground";  // Set this to a layer you know is in front of your background
                projectileSprite.sortingOrder = 1;  // Ensure it's in front of other objects on the same layer
            }
        }
    }

    // Update the health UI text
    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth + "/" + maxHealth; // Display health as numbers
        }
    }
}
