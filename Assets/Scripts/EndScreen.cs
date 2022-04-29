using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    private Text victoryText;

    // Start is called before the first frame update
    void Start()
    {
        victoryText.text = GameManager.winnerText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
