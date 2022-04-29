using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class Goal : MonoBehaviour
{
    [SerializeField]
    private List<MeshRenderer> colorMeshes = new List<MeshRenderer>(); 

    [SerializeField]
    private RacerController racerController;
    [SerializeField]
    private RacerVisualListScriptableObject racerVisualList;
    [SerializeField]
    private LayerMask ballLayer;
    [SerializeField]
    private TextMesh scoreText;

    private void Start()
    {
        SetColor();
        scoreText.text = "0";
    }

    public int GetPlayerNr()
    {
        return racerController.playerNr;
    }

    public void SetColor()
    {
        foreach (var colorMesh in colorMeshes)
        {
            colorMesh.material = racerVisualList.racerVisuals[GetPlayerNr()].floorMaterial;
        }
        scoreText.color = racerVisualList.racerVisuals[GetPlayerNr()].fieldColor;
    }

    public void AddScore(int amount)
    {
        racerController.score += amount;

        scoreText.text = racerController.score.ToString();
    }

}
