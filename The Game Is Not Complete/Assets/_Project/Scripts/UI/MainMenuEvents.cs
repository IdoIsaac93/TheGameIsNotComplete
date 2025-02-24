using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Button _startButton;
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

        // Get the start button and register click event
        _startButton = _uiDocument.rootVisualElement.Q<Button>("StartButton");

        if (_startButton != null)
        {
            _startButton.clicked += OnPlayGameClick;
        }
        else
        {
            Debug.LogError("Start Button Not Found");
        }

        // Get all buttons and register click event
        _menuButtons = _uiDocument.rootVisualElement.Query<Button>().ToList();
        _menuButtons.ForEach(button => button.clicked += OnAllButtonsClick);


    }


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
