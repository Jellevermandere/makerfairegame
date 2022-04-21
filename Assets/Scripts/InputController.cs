using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private List<PlayerInput> playerInputs = new List<PlayerInput>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetAllplayerInputs();
    }

    void GetAllplayerInputs()
    {
        playerInputs[0].moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    /// <summary>
    /// Returns the current input the from the player
    /// </summary>
    /// <param name="nr">the player number</param>
    /// <returns>the player input class</returns>
    public PlayerInput GetPlayerInput(int nr)
    {
        return playerInputs[nr];
    }
}

[System.Serializable]
public class PlayerInput
{
    [Tooltip("The direction for the player to go")]
    public Vector2 moveDirection = Vector2.zero;
}
