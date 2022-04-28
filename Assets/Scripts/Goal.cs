using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private RacerController racerController;
    [SerializeField]
    private RacerVisualListScriptableObject racerVisualList;

    public int GetPlayerNr()
    {
        return racerController.playerNr;
    }

}
