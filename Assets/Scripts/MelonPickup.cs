using UnityEngine;

public class MelonPickup : MonoBehaviour
{
    [SerializeField] private GameObject melonParticleSystem;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<PlayerQuest>().AddMelon();
            Instantiate(melonParticleSystem, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
