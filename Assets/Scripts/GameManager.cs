using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Parameters")]
    public static int nrOfPlayers = 4;
    [SerializeField]
    private int maxBalls = 100;
    [SerializeField]
    private float spawnDelay = 1f;
    public bool playing = true;

    [Header("Scene objects")]
    [SerializeField]
    private InputController inputController;
    [SerializeField]
    private Spawner spawner;
    [SerializeField]
    private Camera overviewCamera;



    // Start is called before the first frame update
    void Start()
    {
        spawner.SpawnRacers();
        Invoke("SetupCam", 1f);

        spawner.SpawnBallAsync(maxBalls, spawnDelay);

    }

    // Update is called once per frame
    void Update()
    {
        
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
}
