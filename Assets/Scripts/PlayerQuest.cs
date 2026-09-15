using UnityEngine;
using TMPro;

public class PlayerQuest : MonoBehaviour
{
    [SerializeField] private int melonsToCollect = 10;
    [SerializeField] private TMP_Text melonText;
    [SerializeField] private AudioClip pickupSoundEffect;
    private int melons = 0;
    private AudioSource audioSource;

    private void Start()
    {
        melonText.text = "" + melons;
        audioSource = GetComponent<AudioSource>();
    }

    public void AddMelon()
    {
        melons++;
        melonText.text = "" + melons;
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(pickupSoundEffect);
    }

    public int GetMelons() {return melons;}
    public int GetMelonsToCollect() {return melonsToCollect;}
}
