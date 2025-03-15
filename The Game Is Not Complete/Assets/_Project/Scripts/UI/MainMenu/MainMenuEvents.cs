using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _uiDocument;
    private AudioSource _audioSource;
    private Dictionary<string, Button> _buttons = new();

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _audioSource = GetComponent<AudioSource>();

        if (_uiDocument == null)
        {
            Debug.LogError("UIDocument component not found!");
            return;
        }

        _buttons["Start"] = _uiDocument.rootVisualElement.Q<Button>("StartButton");
        _buttons["Settings"] = _uiDocument.rootVisualElement.Q<Button>("Settings");
        _buttons["LoadGame"] = _uiDocument.rootVisualElement.Q<Button>("LoadGame");
        _buttons["Exit"] = _uiDocument.rootVisualElement.Q<Button>("Exit");

        _buttons["Start"].clicked += OnPlayGameClick;
        _buttons["Settings"].clicked += OnSettingsClick;
        _buttons["LoadGame"].clicked += OnLoadGameClick;
        _buttons["Exit"].clicked += OnExitGame;

        foreach (var button in _buttons.Values)
        {
            button.clicked += PlayButtonSound;
            button.clicked += () => PulseGlow(button);
        }
    }

    private void PlayButtonSound()
    {
        if (_audioSource != null)
        {
            _audioSource.Play();
        }
    }

    private void PulseGlow(Button button)
    {
        button.style.borderTopWidth = 6; // Thicker border for pulse
        button.style.borderBottomWidth = 6;
        button.style.borderLeftWidth = 6;
        button.style.borderRightWidth = 6;
        button.style.opacity = 1.0f;
        Invoke(nameof(ResetGlow), 0.2f); // Reset after 0.2 seconds
    }

    private void ResetGlow()
    {
        foreach (var button in _buttons.Values)
        {
            button.style.borderTopWidth = 4; // Back to normal
            button.style.borderBottomWidth = 4;
            button.style.borderLeftWidth = 4;
            button.style.borderRightWidth = 4;
            button.style.opacity = 0.9f;
        }
    }

    private void OnPlayGameClick()
    {
        
        DataPersistanceManager.Instance.NewGame(1);
        // SceneManager.LoadScene("DemoLevel");
    }

    private void OnSettingsClick()
    {
        Debug.Log("Opening settings...");
    }

    private void OnLoadGameClick()
    {
        //subject to change when save/load system is implemented
        DataPersistanceManager.Instance.LoadGame();
        Debug.Log("Loading game...");
    }

    private void OnExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void OnDisable()
    {
        UnregisterEvents();
    }

    private void UnregisterEvents()
    {
        if (_buttons.Count == 0) return;

        _buttons["Start"].clicked -= OnPlayGameClick;
        _buttons["Settings"].clicked -= OnSettingsClick;
        _buttons["LoadGame"].clicked -= OnLoadGameClick;
        _buttons["Exit"].clicked -= OnExitGame;

        foreach (var button in _buttons.Values)
        {
            button.clicked -= PlayButtonSound;
            button.clicked -= () => PulseGlow(button);
        }
    }
}