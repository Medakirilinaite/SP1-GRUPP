using UnityEngine;

public class SpikesDamage : MonoBehaviour
{
    [SerializeField] private int damageGiven = 1;
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damageGiven);
        }
    }
}
