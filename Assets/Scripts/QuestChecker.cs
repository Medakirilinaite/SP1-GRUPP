using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class QuestChecker : MonoBehaviour
{
    [SerializeField] private int levelIndex;
    [SerializeField] private float textSpeed = 0.05f;
    [SerializeField] private GameObject worldPanel; 
    [SerializeField] private TextMeshProUGUI worldText; 
    [TextArea(3, 10)] [SerializeField] private string[] finishedLines;
    [TextArea(3, 10)] [SerializeField] private string[] unfinishedLines;

    private Animator anim;
    private bool hasTriggeredSuccess = false;
    private Coroutine typingCoroutine;

    private void Start()
    {
        anim = GetComponent<Animator>();
        if (worldPanel != null) worldPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerQuest playerQuest = other.GetComponent<PlayerQuest>();
            if (playerQuest == null) return;

            if (worldPanel != null) worldPanel.SetActive(true);
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);

            if (playerQuest.GetMelons() >= playerQuest.GetMelonsToCollect())
            {
                if (hasTriggeredSuccess) return;
                hasTriggeredSuccess = true;

                typingCoroutine = StartCoroutine(TypeText(finishedLines));
                if (anim != null) anim.SetTrigger("Flag");
                Invoke(nameof(LoadNextLevel), 5.0f); 
            }
            else
            {
                if (!hasTriggeredSuccess)
                {
                    typingCoroutine = StartCoroutine(TypeText(unfinishedLines));
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggeredSuccess)
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

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(levelIndex);
    }
}