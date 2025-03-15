using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class CountdownDisplay : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument; // Assign in Inspector
    [SerializeField] private float startFontSize = 20f; // Starting font size
    [SerializeField] private float endFontSize = 100f; // Ending font size

    private Label countdownLabel;
    private SceneController sceneController; // Reference to SceneController
    private bool isCountingDown = false;

    void Start()
    {
        // Get the SceneController singleton
        sceneController = SceneController.Instance;
        if (sceneController == null)
        {
            Debug.LogError("SceneController not found!");
            return;
        }

        // Initialize UI Toolkit
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument not assigned!");
            return;
        }

        VisualElement root = uiDocument.rootVisualElement;
        countdownLabel = root.Q<Label>("countdown-label");

        if (countdownLabel == null)
        {
            Debug.LogError("Countdown Label not found! Ensure it’s named 'countdown-label' in the UI Builder.");
            return;
        }

        // Set initial styles
        countdownLabel.style.display = DisplayStyle.None;
        countdownLabel.style.color = new StyleColor(Color.black); // Solid black fill
        countdownLabel.style.unityTextOutlineWidth = 0.2f; // White border thickness
        countdownLabel.style.unityTextOutlineColor = new StyleColor(Color.white); // White border color
        countdownLabel.style.textShadow = new TextShadow
        {
            offset = new Vector2(2f, -2f), // Shadow offset (right-down)
            blurRadius = 4f, // Slight blur for style
            color = new Color(0, 0, 0, 0.5f) // Semi-transparent black shadow
        };
    }

    void Update()
    {
        if (sceneController.isWaveInProgress || isCountingDown) return;

        // Start countdown when waveTimer is reset (i.e., wave just completed)
        if (sceneController.waveTimer <= 0f && !isCountingDown)
        {
            StartCoroutine(CountdownRoutine(sceneController.timeBetweenWaves));
        }

        // Update countdown display if already counting
        if (isCountingDown)
        {
            float timeRemaining = sceneController.timeBetweenWaves - sceneController.waveTimer;
            int currentNumber = Mathf.CeilToInt(timeRemaining);
            countdownLabel.text = currentNumber.ToString();
        }
    }

    private IEnumerator CountdownRoutine(float countdownDuration)
    {
        isCountingDown = true;
        countdownLabel.style.display = DisplayStyle.Flex; // Show the countdown

        float elapsedTime = 0f;
        int lastNumber = Mathf.CeilToInt(countdownDuration);

        while (elapsedTime < countdownDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / countdownDuration; // Normalized time (0 to 1)

            // Update the displayed number based on SceneController's waveTimer
            int currentNumber = Mathf.CeilToInt(sceneController.timeBetweenWaves - sceneController.waveTimer);
            if (currentNumber != lastNumber)
            {
                lastNumber = currentNumber;
                countdownLabel.text = currentNumber.ToString();
                countdownLabel.style.fontSize = startFontSize; // Reset size to 20 for new number
            }

            // Smoothly interpolate font size from start (20) to end (100) for each second
            float timeSinceLastNumber = elapsedTime % 1f; // Time within the current second (0 to 1)
            float currentSize = Mathf.Lerp(startFontSize, endFontSize, timeSinceLastNumber);
            countdownLabel.style.fontSize = currentSize;

            yield return null;
        }

        // Finalize countdown
        countdownLabel.text = "0";
        countdownLabel.style.fontSize = endFontSize; // Full size for "0"
        yield return new WaitForSeconds(0.5f); // Brief pause on "0"
        countdownLabel.style.display = DisplayStyle.None; // Hide after countdown

        isCountingDown = false;
    }
}