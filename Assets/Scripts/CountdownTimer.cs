using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private int index;
    [SerializeField] private float timeLeft;

    private GameObject startZone;
    private Light2D myLight;

    public bool startTimer = false;
    //private int levelIndex = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myLight = GetComponent<Light2D>();
        startTimer = false;
    }

    // Update is called once per frame
    void Update()
    {

        if(startTimer == true)
        {
            timeLeft -= Time.deltaTime;


            if (timeLeft < 10)
            {
                //print("under 10 sek");
                setLightLevel(timeLeft / 10);
            }

            //timeText.text = ""+timeLeft;
            timeText.SetText("{0:0}", timeLeft);

            if( timeLeft <= 0){
                Application.LoadLevel(index);
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("Player has entered starting zone");
            startTimer = true;
        }
    }
    private void LoadNextLevel()
    {
        SceneManager.LoadScene(index);
    }
    private void setLightLevel(float lightLevel)
    {
        myLight.intensity = lightLevel;
    }
}
