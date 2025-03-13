using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverEvent : MonoBehaviour
{
    [SerializeField] float sceneTransitionDelay = 1.5f;
    private UIDocument _uiDocument;
    private Button _restartButton;
    private Button _mainMenuButton;
    private Button _newGame;
    private Button _exitGameButton;
    

    private AudioSource _audioSource;
    private List<Button> _menuButtons = new List<Button>();

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _audioSource = GetComponent<AudioSource>();

        if (_uiDocument == null)
        {
            Debug.LogError("UIDocument is not found!");
            return;
        }

        _restartButton = _uiDocument.rootVisualElement.Q<Button>("RestartLevel");
        _mainMenuButton = _uiDocument.rootVisualElement.Q<Button>("MainMenu");
        _newGame = _uiDocument.rootVisualElement.Q<Button>("NewGame");
        _exitGameButton = _uiDocument.rootVisualElement.Q<Button>("ExitGame");


        _restartButton.clicked += OnRestartClick;
        _mainMenuButton.clicked += OnMainMenuClick;
        _newGame.clicked += OnNewGameClick;
        _exitGameButton.clicked += OnExitGame;


        //get all buttons and register click event
        _menuButtons = _uiDocument.rootVisualElement.Query<Button>().ToList();
        _menuButtons.ForEach(button => button.clicked += OnAllButtonsClick);


    }

    public void ShowGameOverScreen()
    {
        if (_uiDocument == null)
        {
            _uiDocument = GetComponent<UIDocument>();
            if (_uiDocument == null)
            {
                Debug.LogError("UIDocument is not found on GameOverEvent!");
                return;
            }
        }

        if (!_uiDocument.gameObject.activeSelf)
        {
            _uiDocument.gameObject.SetActive(true);
        }

        _uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        Time.timeScale = 0;
    }

    /// <summary>
    /// ********************************************************************************************************
    /// Buttons Click Events
    /// </summary>

    private void OnRestartClick()
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

    private void OnExitGame()
    {
        Application.Quit();
    }   


    private void OnAllButtonsClick()
    {
        if (_audioSource != null)
        {
            _audioSource.Play();
        }
    }





    //Enumerators Overraids  for smooth transitions-->> no arguments current scene, scene name, scene index also available
    private IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSecondsRealtime(sceneTransitionDelay);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSecondsRealtime(sceneTransitionDelay);
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator LoadSceneWithDelay(int sceneIndex)
    {
        yield return new WaitForSecondsRealtime(sceneTransitionDelay);
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneIndex);
    }

}
