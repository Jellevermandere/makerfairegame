using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("Parameters")]
    public static int nrOfPlayers = 4;
    [SerializeField]
    private int maxBalls = 100;
    [SerializeField]
    private float spawnDelay = 1f;
    public bool playing = true;
    [SerializeField]
    public static float gameTime = 120;

    [Header("Scene objects")]
    [SerializeField]
    private InputController inputController;
    [SerializeField]
    private Spawner spawner;
    [SerializeField]
    private Camera overviewCamera;
    [SerializeField]
    private RacerVisualListScriptableObject racerVisualList;
    [SerializeField]
    private Text countdownText;
    [SerializeField]
    private Text timerText;

    private float currentGameTime = 0f;

    public static string winnerText = "";
    public List<RacerController> racerPlacement = new List<RacerController>();


    // Start is called before the first frame update
    void Start()
    {
        spawner.SpawnRacers();
        currentGameTime = gameTime;
        StartCoroutine(CountDown());

        spawner.SpawnBallAsync(maxBalls, spawnDelay);

        timerText.text = currentGameTime.ToString();

    }

    private void Update()
    {
        if (playing)
        {
            currentGameTime -= Time.deltaTime;
            timerText.text = Mathf.RoundToInt(currentGameTime).ToString();

            if (currentGameTime <= 0)
            {
                timerText.text = "0";
                StartCoroutine(EndGame());
            }
            UpdatePlacement();
        }
    }

    public void UpdatePlacement()
    {
        racerPlacement = spawner.racers.OrderByDescending(a => a.score).ToList();
    }

    public void SetPlaying(bool value)
    {
        playing = value;
    }

    void SetupCam()
    {
        if(overviewCamera)
        {
            if(nrOfPlayers == 3)
            {
                overviewCamera.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
            }
            else
            {
                overviewCamera.gameObject.SetActive(false);
            }

            
        }
    }

    public IEnumerator EndGame()
    {
        countdownText.text = "Finish!";
        playing = false;
        winnerText = "The Winner is: \n" + racerVisualList.racerVisuals[racerPlacement[0].playerNr].playerName + "\n" + racerPlacement[0].score;
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    IEnumerator CountDown()
    {
        countdownText.text = "";
        yield return new WaitForSeconds(2);
        SetupCam();

        yield return new WaitForSeconds(1);
        countdownText.text = "3";
        yield return new WaitForSeconds(1);
        countdownText.text = "2";
        yield return new WaitForSeconds(1);
        countdownText.text = "1";
        yield return new WaitForSeconds(1);
        countdownText.text = "GO!";
        playing = true;
        yield return new WaitForSeconds(1);
        countdownText.text = "";
    }
}
