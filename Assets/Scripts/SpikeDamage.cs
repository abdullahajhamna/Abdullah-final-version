using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float damageCooldown = 1f; // Prevent rapid damage
    private float lastDamageTime;
    
    [Header("Effects")]
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private float knockbackForce = 5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time > lastDamageTime + damageCooldown)
        {
            DealDamage(collision);
        }
    }

    private void DealDamage(Collision2D collision)
    {
        // Register damage
        GameManager.Instance.TakeDamage();
        lastDamageTime = Time.time;
        
        // Apply knockback
        Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        
        // Play sound
        if (damageSound != null)
        {
            AudioSource.PlayClipAtPoint(damageSound, transform.position);
        }
    }
}