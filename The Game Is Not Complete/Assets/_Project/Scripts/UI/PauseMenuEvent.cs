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
    private Button _exitGameButton;

    private AudioSource _audioSource;
    private List<Button> _listButtons =  new List<Button>();


    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _audioSource = GetComponent<AudioSource>();
        if (_uiDocument == null)
        {
            Debug.LogError("UIDocument is not found!");
            return;
        }
        _restartLevelButton = _uiDocument.rootVisualElement.Q<Button>("RestartLevel");
        _mainMenuButton = _uiDocument.rootVisualElement.Q<Button>("MainMenu");
        _newGameButton = _uiDocument.rootVisualElement.Q<Button>("NewGame");
        _exitGameButton = _uiDocument.rootVisualElement.Q<Button>("ExitGame");
        _restartLevelButton.clicked += OnRestartLevelClick;
        _mainMenuButton.clicked += OnMainMenuClick;
        _newGameButton.clicked += OnNewGameClick;
        _exitGameButton.clicked += OnExitGameClick;


        // all buttons with registrerr click event 

        _listButtons = _uiDocument.rootVisualElement.Query<Button>().ToList();
        _listButtons.ForEach(button => button.clicked += OnAllButtonsClick);
    }

    public void ShowPauseMenu()
    {
        if (!_uiDocument.gameObject.activeSelf)
        {
            _uiDocument.gameObject.SetActive(true);  // Enable the UI object if disabled
        }
        _uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        Time.timeScale = 0; // Pause the game

    }

    public void HidePauseMenu()
    {
        if (_uiDocument.gameObject.activeSelf)
        {
            _uiDocument.gameObject.SetActive(false);  // Disable the UI object if enabled
        }
        _uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        Time.timeScale = 1; // Resume the game
    }

    /// Buttons Click Events
    private void OnRestartLevelClick() 
    {
        StartCoroutine(LoadSceneWithDelay());
    }

    private void OnMainMenuClick()
    {
        StartCoroutine(LoadSceneWithDelay("MainMenu"));
    }

    private void OnNewGameClick()
    {
        StartCoroutine(LoadSceneWithDelay("Level_001"));
    }

    private void OnExitGameClick()
    {
        Application.Quit();
    }

    private void OnAllButtonsClick() 
    { 
        Debug.Log("Button Clicked");
        if ( _audioSource != null)
        {
            _audioSource.Play();
        }else
        { 
            Debug.LogError("Audio Source is not found!");
        }
    }


    //Enumerators Overraids  for smooth transitions-->> no arguments current scene, scene name, scene index also available

    private IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(sceneTransitionDelay);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(sceneTransitionDelay);
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
    private IEnumerator LoadSceneWithDelay(int sceneIndex)
    {
        yield return new WaitForSeconds(sceneTransitionDelay);
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneIndex);
    }

}
