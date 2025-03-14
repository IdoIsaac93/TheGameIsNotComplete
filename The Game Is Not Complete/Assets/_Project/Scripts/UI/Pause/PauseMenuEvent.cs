using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenuEvent : MonoBehaviour
{
    [SerializeField] float sceneTransitionDelay = 1.5f;
    private UIDocument _uiDocument;
    private Button _restartLevelButton;
    private Button _mainMenuButton;
    private Button _newGameButton;
    private Button _loadGameButton;
    private Button _exitGameButton;
    private AudioSource _audioSource;
    private List<Button> _listButtons = new List<Button>();
    private bool _isPaused = false;
    private GameObject _canvasUI;
    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _audioSource = GetComponent<AudioSource>();


        if (_uiDocument == null)
        {
            Debug.LogError("UIDocument is not found!");
            return;
        }

        // Cache the CanvasUI reference once
        _canvasUI = GameObject.FindWithTag("CanvasUI");
        if (_canvasUI == null)
        {
            Debug.LogError("CanvasUI is not found!");
        }

        if (_uiDocument == null)
        {
            Debug.LogError("UIDocument is not found!");
            return;
        }

        InitializeButtons();
        // Hide the pause menu initially
        HidePauseMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle pause menu
            TogglePause();
        }
    }

    private void OnEnable()
    {
        Debug.Log("PauseMenu OnEnable called");
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        // Clear any existing references
        _restartLevelButton = null;
        _mainMenuButton = null;
        _newGameButton = null;
        _exitGameButton = null;
        _loadGameButton = null;
        _listButtons.Clear();

        if (_uiDocument == null || _uiDocument.rootVisualElement == null) return;

        // Get button references
        _restartLevelButton = _uiDocument.rootVisualElement.Q<Button>("RestartLevel");
        _mainMenuButton = _uiDocument.rootVisualElement.Q<Button>("MainMenu");
        _newGameButton = _uiDocument.rootVisualElement.Q<Button>("NewGame");
        _exitGameButton = _uiDocument.rootVisualElement.Q<Button>("ExitGame");
        _loadGameButton = _uiDocument.rootVisualElement.Q<Button>("LoadGame");


        // Register click events - with event removal first to prevent duplicates
        if (_restartLevelButton != null)
        {
            _restartLevelButton.clicked -= OnRestartLevelClick;
            _restartLevelButton.clicked += OnRestartLevelClick;
        }

        if (_mainMenuButton != null)
        {
            _mainMenuButton.clicked -= OnMainMenuClick;
            _mainMenuButton.clicked += OnMainMenuClick;
        }

        if (_newGameButton != null)
        {
            _newGameButton.clicked -= OnNewGameClick;
            _newGameButton.clicked += OnNewGameClick;
        }

        if (_loadGameButton != null)
        {
            _loadGameButton.clicked -= OnLoadGameClick;
            _loadGameButton.clicked += OnLoadGameClick;
        }

        if (_exitGameButton != null)
        {
            _exitGameButton.clicked -= OnExitGameClick;
            _exitGameButton.clicked += OnExitGameClick;
        }

        // Get all buttons and register the common click event
        _listButtons = _uiDocument.rootVisualElement.Query<Button>().ToList();
        foreach (var button in _listButtons)
        {
            button.clicked -= OnAllButtonsClick;
            button.clicked += OnAllButtonsClick;

            // Make buttons more responsive by adding mouse events
            button.RegisterCallback<ClickEvent>(evt => {
                
            });

            button.RegisterCallback<MouseDownEvent>(evt => {
                
            });
        }

        //Debug.Log($"Total buttons registered: {_listButtons.Count}");
    }

    public void ShowPauseMenu()
    {
        
        

        if (_uiDocument == null)
        {
            _uiDocument = GetComponent<UIDocument>();
            if (_uiDocument == null)
            {
                Debug.LogError("UIDocument is not found on PauseMenuEvent!");
                return;
            }
        }

        _uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        _uiDocument.rootVisualElement.style.visibility = Visibility.Visible;

        // Ensure all buttons are initialized and event handlers are attached
        InitializeButtons();
    }

    public void HidePauseMenu()
    {
        

        if (_uiDocument != null && _uiDocument.rootVisualElement != null)
        {
            _uiDocument.rootVisualElement.style.display = DisplayStyle.None;
            _uiDocument.rootVisualElement.style.visibility = Visibility.Hidden;
        }

        
    }


    private void TogglePause() 
    {
        _isPaused = !_isPaused;
        //using Time.timeScale to pause the game with ternary operator to toggle between 0 and 1
        Time.timeScale = _isPaused ? 0f : 1f;

        if (_canvasUI != null)
        {
            _canvasUI.SetActive(!_isPaused);
        }
        else 
        { 
        
            Debug.LogError("CanvasUI is not found!");
        }


        if (_isPaused)
        {
            _canvasUI.SetActive(false);
            ShowPauseMenu();

        }
        else
        {

            _canvasUI.SetActive(true);
            HidePauseMenu();

        }

        Debug.Log(_isPaused ? "Game Paused" : "Game Unpause");
    }



    /// Buttons Click Events
    private void OnRestartLevelClick()
    {
        DataPersistanceManager.Instance.NewGame();
    }

    private void OnMainMenuClick()
    {
        Debug.Log("Main Menu clicked");
        // Set timeScale back to 1 immediately to ensure UI responsiveness
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneWithDelay("MainMenu"));

    }

    private void OnNewGameClick()
    {
        Debug.Log("New Game clicked");
        // Set timeScale back to 1 immediately to ensure UI responsiveness
        Time.timeScale = 1f;
        DataPersistanceManager.Instance.NewGame();

    }

    private void OnLoadGameClick()
    {
        // Set timeScale back to 1 immediately to ensure UI responsiveness
        Time.timeScale = 1f;
        DataPersistanceManager.Instance.LoadGame();
    }

    private void OnExitGameClick()
    {
        Debug.Log("Exit Game clicked");
        // Set timeScale back to 1 immediately
        Time.timeScale = 1f;
        Application.Quit();
    }

    private void OnAllButtonsClick()
    {
        Debug.Log("Button Clicked - common handler");
        if (_audioSource != null)
        {
            _audioSource.Play();
        }
        else
        {
            Debug.LogError("Audio Source is not found!");
        }
    }

    //Enumerators for smooth transitions
    private IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSecondsRealtime(sceneTransitionDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSecondsRealtime(sceneTransitionDelay);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator LoadSceneWithDelay(int sceneIndex)
    {
        yield return new WaitForSecondsRealtime(sceneTransitionDelay);
        SceneManager.LoadScene(sceneIndex);
    }
}