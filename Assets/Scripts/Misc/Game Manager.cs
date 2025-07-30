using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isPaused;

    public GameObject pausedUI;
    public GameObject gameOverUI;
    public GameObject tower;
    public GameObject player;

    private void Start()
    {
        // fix enemy movement jitter caused by difference between FixedUpdate() and Update()
        int refreshRate = Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value);
        Debug.Log(refreshRate);
        if (refreshRate > 0)
        {
            Time.fixedDeltaTime = 1f / refreshRate;
        }
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Game Scene") return;
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) // pause the game with Esc or P
        {
            PauseUnpause();
        }
    }
    public void SwitchScene()  // swap between Main Menu and Game Scene
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Main Menu")
        {
            SceneManager.LoadScene("Game Scene");
        }
        else if (currentScene == "Game Scene")
        {
            SceneManager.LoadScene("Main Menu");
        }
        Unfreeze();  // unfreeze the game if last scene ended paused (it always does)
    }
    public void ExitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }
    public void PauseUnpause() // pauses and unpauses
    {
        if (!isPaused)
        {
            Freeze();
            pausedUI?.SetActive(true);  // enables Pause UI
        }
        else
        {
            Unfreeze();
            pausedUI?.SetActive(false); // disables Pause UI
        }
    }
    public void GameOver() 
    {
        Freeze();
        gameOverUI?.SetActive(true);
    }
    public void Restart() // reload game
    {
        SceneManager.LoadScene("Game Scene");
        Unfreeze();
    }
    public void Freeze()  // stops most game processes
    {
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void Unfreeze() // starts most game processes
    {
        Time.timeScale = 1f;
        isPaused = false;
    }
}
