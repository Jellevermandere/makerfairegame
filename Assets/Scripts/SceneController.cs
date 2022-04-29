using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("Reload Scene")]
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [ContextMenu("Load Play Scene")]
    public void LoadPlayScene()
    {
        SceneManager.LoadScene(1);
    }

    [ContextMenu("Main Menu")]
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}