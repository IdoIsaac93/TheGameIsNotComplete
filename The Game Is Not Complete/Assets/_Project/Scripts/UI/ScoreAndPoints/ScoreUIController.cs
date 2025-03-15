using UnityEngine;
using UnityEngine.UIElements;

public class ScoreUIController : MonoBehaviour
{
    public UIDocument _UIDocument; // Match your public UIDocument field

    private Label scoreLabel;
    private Label systemPointsLabel;

    private void Awake()
    {
        //playerResources = FindFirstObjectByType<PlayerResources>(); // Original comment: no longer needed with singleton
        PlayerResources.OnScoreChanged += ScoreChanged;
        PlayerResources.OnSysPointsChanged += SystemPointsChanged;
        // Subscribe to OnHealthChanged since score can change with damage
        PlayerResources.OnHealthChanged += HealthChanged;

        scoreLabel = _UIDocument.rootVisualElement.Q<Label>("scoreValue");
        systemPointsLabel = _UIDocument.rootVisualElement.Q<Label>("systemPointsValue");

        // Initial update
        UpdateAll();
    }

    private void OnEnable()
    {
        GetUiElements();
    }

    private void HealthChanged()
    {
        // Only update score since health changes can affect it
        if (scoreLabel != null)
        {
            scoreLabel.text = PlayerResources.Instance.GetScore().ToString();
        }
    }

    private void ScoreChanged()
    {
        if (scoreLabel != null)
        {
            scoreLabel.text = PlayerResources.Instance.GetScore().ToString();
        }
    }

    private void SystemPointsChanged()
    {
        if (systemPointsLabel != null)
        {
            systemPointsLabel.text = PlayerResources.Instance.GetSystemPoints().ToString();
        }
    }

    private void UpdateAll()
    {
        if (scoreLabel != null)
        {
            scoreLabel.text = PlayerResources.Instance.GetScore().ToString();
        }
        if (systemPointsLabel != null)
        {
            systemPointsLabel.text = PlayerResources.Instance.GetSystemPoints().ToString();
        }
    }

    private void OnDestroy()
    {
        //for no memory leaks when the game is closed
        PlayerResources.OnHealthChanged -= HealthChanged;
        PlayerResources.OnScoreChanged -= ScoreChanged;
        PlayerResources.OnSysPointsChanged -= SystemPointsChanged;
    }

    private void OnDisable()
    {
        //for no memory leaks when the game is closed
        PlayerResources.OnHealthChanged -= HealthChanged;
        PlayerResources.OnScoreChanged -= ScoreChanged;
        PlayerResources.OnSysPointsChanged -= SystemPointsChanged;
    }

    private void GetUiElements()
    {
        PlayerResources.OnHealthChanged += HealthChanged;
        PlayerResources.OnScoreChanged += ScoreChanged;
        PlayerResources.OnSysPointsChanged += SystemPointsChanged;

        scoreLabel = _UIDocument.rootVisualElement.Q<Label>("scoreValue");
        systemPointsLabel = _UIDocument.rootVisualElement.Q<Label>("systemPointsValue");

        UpdateAll();
    }
}