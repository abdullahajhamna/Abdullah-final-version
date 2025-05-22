using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    [SerializeField] private int value = 1;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private AudioClip collectSound;
    
    [Header("Level Progression")]
    [SerializeField] private int coinsRequiredInLevel = 30; // Set this per-level
    
    private static int coinsCollectedThisLevel = 0;

    private void Start()
    {
        coinsCollectedThisLevel = 0; // Reset counter when level loads
    }

    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddCoins(value);
            coinsCollectedThisLevel += value;
            
            if (coinsCollectedThisLevel >= coinsRequiredInLevel)
            {
                GameManager.Instance.WinGame(true); // Go to next level
            }
            
            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            
            Destroy(gameObject);
        }
    }
}