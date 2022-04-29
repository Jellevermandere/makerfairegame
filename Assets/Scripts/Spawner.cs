using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField]
    private GameObject RacerPrefab;
    [SerializeField]
    private GameObject arenaLinePrefab;
    [SerializeField]
    private float ballSpawnRadius = 10;
    [SerializeField]
    private float spawnHeight = 10f;
    [SerializeField]
    private GameManager gm;

    [SerializeField]
    public List<Transform> balls = new List<Transform>();
    public List<RacerController> racers = new List<RacerController>();

    [SerializeField]
    private GameObject ballPrefab;

    public void SpawnRacers()
    {
        for (int i = 0; i < GameManager.nrOfPlayers; i++)
        {
            Transform playerNull = Instantiate(RacerPrefab, transform).transform;
            playerNull.Rotate(0, i * 360f / GameManager.nrOfPlayers, 0);
            RacerController racer = playerNull.GetComponentInChildren<RacerController>();
            racers.Add(racer);
            racer.playerNr = i;
            racer.gm = gm;
            if(TryGetComponent(out InputController input))
            {
                racer.inputController = input;
            }

            Transform arenaLine = Instantiate(arenaLinePrefab, transform).transform;
            arenaLine.Rotate(0, 180f/ GameManager.nrOfPlayers + i * 360f / GameManager.nrOfPlayers, 0);

        }
    }

    public async void SpawnBallAsync(int amount, float delay = 0f)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2 spawnLocation = Random.insideUnitCircle * ballSpawnRadius;

            balls.Add(Instantiate(ballPrefab, new Vector3(spawnLocation.x, spawnHeight, spawnLocation.y), Quaternion.identity, transform).transform);
            await Task.Delay(System.TimeSpan.FromSeconds(delay));
        }

    }
}
