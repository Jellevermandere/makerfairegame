using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

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
    private float gameTime = 120;

    [Header("Scene objects")]
    [SerializeField]
    private InputController inputController;
    [SerializeField]
    private Spawner spawner;
    [SerializeField]
    private Camera overviewCamera;
    [SerializeField]
    private RacerVisualListScriptableObject racerVisualList;

    private float currentGameTime = 0f;

    public static string winnerText = "";
    public List<RacerController> racerPlacement = new List<RacerController>();


    // Start is called before the first frame update
    void Start()
    {
        spawner.SpawnRacers();
        currentGameTime = gameTime;
        Invoke("SetupCam", 1f);

        spawner.SpawnBallAsync(maxBalls, spawnDelay);

    }

    private void Update()
    {
        if (playing)
        {
            currentGameTime -= Time.deltaTime;

            if(currentGameTime <= 0)
            {
                EndGame();
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

    public void EndGame()
    {
        playing = false;

        winnerText = "The Winner is: \n" + racerVisualList.racerVisuals[racerPlacement[0].playerNr].playerName + "\n" + racerPlacement[0].score;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }
}
