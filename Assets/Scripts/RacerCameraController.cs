using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera), typeof(AudioListener))]
public class RacerCameraController : MonoBehaviour
{
    [SerializeField]
    private RacerController racerController;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        if (racerController)
        {
            if (racerController.playerNr != 0) GetComponent<AudioListener>().enabled = false;

            cam = GetComponent<Camera>();
            if (GameManager.nrOfPlayers == 2)
            {
                cam.rect = new Rect(racerController.playerNr == 0? 0:0.5f, 0, 0.5f, 1);
            }
            else if (GameManager.nrOfPlayers > 2)
            {
                cam.rect = new Rect(racerController.playerNr%2 == 0 ? 0 : 0.5f, racerController.playerNr > 1 ? 0 : 0.5f, 0.5f, 0.5f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
