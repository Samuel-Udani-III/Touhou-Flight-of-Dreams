using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;  // Set a damage value for the projectile (can be adjusted per projectile type)

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the projectile hits the border
        if (collision.gameObject.CompareTag("Border"))
        {
            Destroy(gameObject);  // Destroy the projectile when it hits the border
        }

        // Check if the projectile hits the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Destroy the projectile when it hits the player
            Destroy(gameObject);

            // Call the player's OnHitByProjectile method and pass the damage value
            collision.gameObject.GetComponent<Player>().OnHitByProjectile(damage);

            // Ignore any further physics interaction between the projectile and the player
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }

        // Check if the projectile hits another projectile (e.g., player projectiles)
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            // Destroy the enemy projectile
            Destroy(collision.gameObject);

            // Destroy the player projectile as well (optional)
            Destroy(gameObject);
        }
    }
}
