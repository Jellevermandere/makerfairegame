using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISliderText : MonoBehaviour
{
    [SerializeField]
    private Text nrText;
    [SerializeField]
    private Text timeText;

    public void SetText(float value)
    {
        nrText.text = value.ToString();
        GameManager.nrOfPlayers = Mathf.RoundToInt(value);
    }
    public void SetTmeText(float value)
    {
        nrText.text = value.ToString();
        GameManager.gameTime = value;
    }
}
