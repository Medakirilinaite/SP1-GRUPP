using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healthToRestore;
    [SerializeField] private GameObject cherrieParticleSystem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bool hasRestoredHealth = other.gameObject.GetComponent<PlayerHealth>().RestoreHealth(healthToRestore);

            if (hasRestoredHealth)
            {
                Instantiate(cherrieParticleSystem, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
