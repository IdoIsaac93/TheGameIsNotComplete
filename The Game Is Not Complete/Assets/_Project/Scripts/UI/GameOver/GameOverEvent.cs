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
    private Button _loadGame;
    private Button _exitGameButton;
    private GameObject _pauseMenu;
    private GameObject _canvasUI;
    

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
        _loadGame = _uiDocument.rootVisualElement.Q<Button>("LoadGame");
        _exitGameButton = _uiDocument.rootVisualElement.Q<Button>("ExitGame");


        _restartButton.clicked += OnRestartClick;
        _mainMenuButton.clicked += OnMainMenuClick;
        _loadGame.clicked += OnLoadGameClick;
        _exitGameButton.clicked += OnExitGame;


        //get all buttons and register click event
        _menuButtons = _uiDocument.rootVisualElement.Query<Button>().ToList();
        _menuButtons.ForEach(button => button.clicked += OnAllButtonsClick);


        _canvasUI = GameObject.Find("CanvasUI");
        _pauseMenu = GameObject.Find("PauseMenu");

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
        //Disable pause menu and main Canvas UI
        if (_pauseMenu != null)
        {
            _pauseMenu.SetActive(false);
        }
        if (_canvasUI != null)
        {
            _canvasUI.SetActive(false);
        }
        Time.timeScale = 0;
    }

    /// <summary>
    /// Buttons Click Events
    /// </summary>

    private void OnRestartClick()
    {
        DataPersistanceManager.Instance.NewGame();
    }

    private void OnMainMenuClick()
    {
        StartCoroutine(LoadSceneWithDelay("MainMenu"));
    }

    private void OnLoadGameClick()
    {
        DataPersistanceManager.Instance.LoadGame();
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
