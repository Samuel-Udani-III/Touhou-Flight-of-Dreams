using UnityEngine;

public class PlayerProjectileLogic : MonoBehaviour
{
    // Reference to a particle effect (optional, assign in the Inspector)
    public GameObject hitEffect; 
    
    private Rigidbody2D rb;

    void Start()
    {
        // Get the Rigidbody2D component attached to the projectile
        rb = GetComponent<Rigidbody2D>();

        // Ignore collisions between the player and the player's projectile
        int playerLayer = LayerMask.NameToLayer("Player");
        int playerProjectileLayer = LayerMask.NameToLayer("PlayerProjectile");
        Physics2D.IgnoreLayerCollision(playerLayer, playerProjectileLayer, true);
    }

    // Set the direction of the projectile based on the shooting direction
    public void FireProjectile(Vector2 direction, float speed)
    {
        // Apply velocity to the projectile in the direction it was fired.
        rb.velocity = direction.normalized * speed;

        // Optional: You can log the projectile's velocity or direction for debugging purposes
        // Debug.Log("Projectile fired with velocity: " + rb.velocity);
    }

    // The player's projectile will be destroyed when colliding with an enemy, enemy's projectile, or the border
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the projectile collided with an enemy, enemy's projectile, or the border
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyProjectile") || collision.gameObject.CompareTag("Border"))
        {
            // Destroy this projectile
            Destroy(gameObject);

            // Optionally, trigger a hit effect (particle effect)
            if (hitEffect != null)
            {
                // Instantiate a particle effect at the collision point
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            // Optionally, you can also play a sound when the projectile hits
            // For example, you can play a sound here:
            // AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }
    }
}
