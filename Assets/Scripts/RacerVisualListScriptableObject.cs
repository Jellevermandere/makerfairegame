using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RacerVisualList", menuName = "ScriptableObjects/racerVisualList")]
public class RacerVisualListScriptableObject : ScriptableObject
{
    public List<RacerVisualScriptableObject> racerVisuals = new List<RacerVisualScriptableObject>();
}
