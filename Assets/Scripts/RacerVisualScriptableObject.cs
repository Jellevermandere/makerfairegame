using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RacerVisual", menuName = "ScriptableObjects/racerVisual")]
public class RacerVisualScriptableObject : ScriptableObject
{
    public string playerName = "color";
    public Material racerMaterial;
    public Material floorMaterial;
    public Color playerColor = Color.white;
    public Color fieldColor = Color.white;
}
