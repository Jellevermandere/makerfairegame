using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private List<PlayerInput> playerInputs = new List<PlayerInput>();

    [SerializeField]
    private bool UseWiimotes = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetAllplayerInputs();
    }

    public void UpdateInputMethod()
    {
        if (UseWiimotes)
        {
            while (playerInputs.Count < WiimoteManager.Wiimotes.Count)
            {
                playerInputs.Add(new PlayerInput());
            }
        }
    }

    void GetAllplayerInputs()
    {
        if (UseWiimotes)
        {
            for (int i = 0; i < WiimoteManager.Wiimotes.Count; i++)
            {
                // x = Forwards and backwards
                // y = Left(1) and Right(-1)
                Vector3 wiimoteAccelData = WiimoteManager.Wiimotes[i].Accel.GetAccelVector();
                Vector2 wiimoteSteer = new Vector2(-wiimoteAccelData.y, wiimoteAccelData.x);

                playerInputs[i].moveDirection = wiimoteSteer;
            }
        }
        else
        {
            playerInputs[0].moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        
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
