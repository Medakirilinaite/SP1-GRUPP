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
    [SerializeField] private string[] finishedLines;
    [SerializeField] private string[] unfinishedLines;

    private Animator anim;
    private Coroutine typingCoroutine;

    private void Start()
    {
        anim = GetComponent<Animator>();
        if (worldPanel != null) 
        {
            worldPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerQuest playerQuest = other.GetComponent<PlayerQuest>();
            if (worldPanel != null) 
            {
                worldPanel.SetActive(true);
            }
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            if (playerQuest.GetMelons() >= playerQuest.GetMelonsToCollect())
            {
                typingCoroutine = StartCoroutine(TypeText(finishedLines));
                anim.SetTrigger("Flag");
                Invoke(nameof(LoadNextLevel), 5.0f); 
            }
            else
            {
                typingCoroutine = StartCoroutine(TypeText(unfinishedLines));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            if (worldPanel != null) 
            {
                worldPanel.SetActive(false);
            }
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