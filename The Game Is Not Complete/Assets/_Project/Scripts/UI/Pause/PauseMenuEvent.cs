using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenuEvent : MonoBehaviour
{
    [SerializeField] float sceneTransitionDelay = 1.5f;
    private UIDocument _uiDocument;
    private Button _mainMenuButton;
    private Button _newGameButton;
    private Button _exitGameButton;
    private Button _pauseMenuButton;
    private Button _closeButton;
    private Button _skipTimerButton;
    private AudioSource _audioSource;
    private VisualElement _menuMainContainer;
    private VisualElement _playButtonsContainer;
    private List<Button> _listButtons = new List<Button>();
    private bool _isPaused = false;
    private GameObject _canvasUI;
    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _audioSource = GetComponent<AudioSource>();
        _menuMainContainer = _uiDocument.rootVisualElement.Q<VisualElement>("MainContainer");

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
        
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        // Clear any existing references
        
        _mainMenuButton = null;
        _newGameButton = null;
        _exitGameButton = null;
        _pauseMenuButton = null;
        _closeButton = null;
        _skipTimerButton = null;

        _listButtons.Clear();

        if (_uiDocument == null || _uiDocument.rootVisualElement == null) return;

        // Get button references
        
        _mainMenuButton = _uiDocument.rootVisualElement.Q<Button>("MainMenu");
        _newGameButton = _uiDocument.rootVisualElement.Q<Button>("NewGame");
        _exitGameButton = _uiDocument.rootVisualElement.Q<Button>("ExitGame");
        _pauseMenuButton = _uiDocument.rootVisualElement.Q<Button>("Pause");
        _closeButton = _uiDocument.rootVisualElement.Q<Button>("CloseMenu");
        _skipTimerButton = _uiDocument.rootVisualElement.Q<Button>("Foward");
        _playButtonsContainer = _uiDocument.rootVisualElement.Q<VisualElement>("PlayButtonsContainer");


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


        if (_exitGameButton != null)
        {
            _exitGameButton.clicked -= OnExitGameClick;
            _exitGameButton.clicked += OnExitGameClick;
        }

        if (_pauseMenuButton != null)
        {
            _pauseMenuButton.clicked -= OnPauseMenuButton;
            _pauseMenuButton.clicked += OnPauseMenuButton;
        }

        if (_closeButton != null)
        {
            _closeButton.clicked -= OnCloseMenubutton;
            _closeButton.clicked += OnCloseMenubutton;
        }

        if (_skipTimerButton != null)
        {
            _skipTimerButton.clicked -= OnSkipButton;
            _skipTimerButton.clicked += OnSkipButton;
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

        _menuMainContainer.style.display = DisplayStyle.Flex;
        _menuMainContainer.style.visibility = Visibility.Visible;
        _menuMainContainer.AddToClassList("bottomsheet-show");

        //_uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        //_uiDocument.rootVisualElement.style.visibility = Visibility.Visible;

        // Ensure all buttons are initialized and event handlers are attached
        InitializeButtons();
    }

    public void HidePauseMenu()
    {
        
        
        if (_uiDocument != null && _uiDocument.rootVisualElement != null)
        {
            _menuMainContainer.style.display = DisplayStyle.None;
            _menuMainContainer.style.visibility = Visibility.Hidden;
            _menuMainContainer.AddToClassList("bottomsheet");
            //_uiDocument.rootVisualElement.style.display = DisplayStyle.None;
            //_uiDocument.rootVisualElement.style.visibility = Visibility.Hidden;
            //uiDocument.rootVisualElement.style.display = DisplayStyle.None;

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
        DataPersistanceManager.Instance.NewGame(1);

    }

    private void OnExitGameClick()
    {
        Debug.Log("Exit Game clicked");
        // Set timeScale back to 1 immediately
        Time.timeScale = 1f;
        Application.Quit();
    }

    private void OnPauseMenuButton()
    {
       
        // Set timeScale back to 1 immediately to ensure UI responsiveness
        Time.timeScale = 0f;
        //_pauseMenuButton.style.display = DisplayStyle.None;
        _playButtonsContainer.style.display = DisplayStyle.None;

        ShowPauseMenu();
    }

    private void OnCloseMenubutton()
    {
        _menuMainContainer.RemoveFromClassList("bottomsheet-show");
        _menuMainContainer.style.display = DisplayStyle.None;
        _menuMainContainer.style.visibility = Visibility.Hidden;
        //_pauseMenuButton.style.display = DisplayStyle.Flex;
        _playButtonsContainer.style.display = DisplayStyle.Flex;

        Time.timeScale = 1f;
        
    }

    private void OnSkipButton() { 
        SceneController.Instance.SkipTimer();
    }


    private void OnAllButtonsClick()
    {
        
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