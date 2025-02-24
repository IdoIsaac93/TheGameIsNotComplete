using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Button _startButton;
    private Button _settingsButton;
    private Button _creditsButton;
    private Button _quitGameButton;
    
    private List<Button> _menuButtons = new List<Button>();
    private AudioSource _audioSource;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _audioSource = GetComponent<AudioSource>();

        // Check if UIDocument is found
        if (_uiDocument == null)
        {
            Debug.LogError("UIDocument is not found!");
            return;
        }

        // Get buttons from UIDocument and register click events
        _startButton = _uiDocument.rootVisualElement.Q<Button>("StartButton");
        _settingsButton = _uiDocument.rootVisualElement.Q<Button>("Settings");
        _creditsButton = _uiDocument.rootVisualElement.Q<Button>("Credits");
        _quitGameButton = _uiDocument.rootVisualElement.Q<Button>("Exit");


        _startButton.clicked += OnPlayGameClick;
        _settingsButton.clicked += OnSettingsClick;
        _creditsButton.clicked += OnCreditsClick;
        _quitGameButton.clicked += OnExitGame;



        // Get all buttons and register click event
        _menuButtons = _uiDocument.rootVisualElement.Query<Button>().ToList();
        _menuButtons.ForEach(button => button.clicked += OnAllButtonsClick);


    }


    /// <summary>
    /// ********************************************************************************************************
    /// Buttons Click Events
    /// </summary>


    //Play game button click event :: start button
    private void OnPlayGameClick()
    {
        Debug.Log("Play Game Button Clicked");
        SceneManager.LoadScene("DemoLevel");
    }

    //All buttons click event
    private void OnAllButtonsClick()
    {
        if (_audioSource != null)
        {
            _audioSource.Play();
        }
    }

    //Settings button click event
    private void OnSettingsClick()
    {
        Debug.Log("Settings Button Clicked"); // remove when adding functionality
    }

    private void OnExitGame()
    {
        Debug.Log("Exit Game Button Clicked"); // remove when adding functionality
        Application.Quit();
    }

    private void OnCreditsClick()
    {
        Debug.Log("Credits Button Clicked"); // remove when adding functionality
    }



    //Nothing to see here just best practices
    private void OnDestroy()
    {
        if (_startButton != null)
        {
            _startButton.clicked -= OnPlayGameClick;
        }

        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].clicked -= OnAllButtonsClick;
        }
    }

    private void OnDisable()
    {
        if (_startButton != null)
        {
            _startButton.clicked -= OnPlayGameClick;
        }

        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].clicked -= OnAllButtonsClick;
        }
    }
}
