using UnityEngine;

public class PauseController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pauseMenu; // Drag the PauseMenu prefab here

    private PauseMenuEvent pauseMenuEvent;
    private bool isPaused = false;

    void Start()
    {
        // Get the PauseMenuEvent component
        if (pauseMenu != null)
        {
            pauseMenuEvent = pauseMenu.GetComponent<PauseMenuEvent>();
            if (pauseMenuEvent == null)
            {
                Debug.LogError("PauseMenuEvent component not found on pauseMenu GameObject!");
            }
        }
        else
        {
            Debug.LogError("Pause Menu GameObject not assigned in the inspector!");
        }

        // Make sure the pause menu is hidden at start
        if (pauseMenuEvent != null)
        {
            pauseMenuEvent.HidePauseMenu();
        }

        // Ensure normal game speed at start
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Only process input when the game is running or if we're already paused
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed - toggling pause state");
            TogglePause();
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        Debug.Log($"Game paused: {isPaused}");

        if (isPaused)
        {
            // First show the menu, then pause the game
            if (pauseMenuEvent != null)
            {
                pauseMenuEvent.ShowPauseMenu();
            }
            Time.timeScale = 0f;
        }
        else
        {
            // First unpause the game, then hide the menu
            Time.timeScale = 1f;
            if (pauseMenuEvent != null)
            {
                pauseMenuEvent.HidePauseMenu();
            }
        }
    }
}