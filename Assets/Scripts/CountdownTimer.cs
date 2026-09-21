using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;

    [SerializeField] private float timeLeft;

    private Light2D myLight;


    private int levelIndex = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myLight = GetComponent<Light2D>();

    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            LoadNextLevel();
        }

        if (timeLeft<10)
        {
            print("under 10 sek");
            setLightLevel(timeLeft/10);
        }

        //timeText.text = ""+timeLeft;
        timeText.SetText("{0:0}", timeLeft);

    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(levelIndex);
    }
    private void setLightLevel(float lightLevel)
    {
        myLight.intensity = lightLevel;
    }
}
