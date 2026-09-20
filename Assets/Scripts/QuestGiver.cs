using System.Collections;
using UnityEngine;
using TMPro;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private float textSpeed = 0.05f;
    [SerializeField] private GameObject worldPanel; 
    [SerializeField] private TextMeshProUGUI worldText; 
    [TextArea(3, 10)] [SerializeField] private string[] dialogueLines;

    private Coroutine typingCoroutine;

    private void Start()
    {
        if (worldPanel != null) worldPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (worldPanel != null) worldPanel.SetActive(true);
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeText(dialogueLines));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            if (worldPanel != null) worldPanel.SetActive(false);
        }
    }

    IEnumerator TypeText(string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            worldText.text = "";
            foreach (char c in lines[i].ToCharArray())
            {
                worldText.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
            if (i < lines.Length - 1) yield return new WaitForSeconds(2.0f);
        }
    }
}