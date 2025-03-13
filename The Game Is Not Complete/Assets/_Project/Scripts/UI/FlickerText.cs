using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FlickerText : MonoBehaviour
{
    [SerializeField] public UIDocument uiDocument;
    [SerializeField] public float minInterval = 0.5f;
    [SerializeField] public float maxInterval = 3f;

    private List<VisualElement> textElements = new List<VisualElement>();
    private string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";

    void Start()
    {
        if (uiDocument == null)
        {
            Debug.LogError("UI Document is missing!");
            return;
        }

        VisualElement root = uiDocument.rootVisualElement;
        textElements.AddRange(root.Query<Label>().ToList());
        textElements.AddRange(root.Query<Button>().ToList());

        if (textElements.Count > 0)
        {
            StartCoroutine(FlickerRoutine());
        }
        else
        {
            Debug.LogWarning("No text elements found!");
        }
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            
            yield return new WaitForSecondsRealtime(Random.Range(minInterval, maxInterval));

            if (textElements.Count == 0) continue;

            // Choose a random element
            VisualElement element = textElements[Random.Range(0, textElements.Count)];
            string originalText = GetText(element);

            if (string.IsNullOrEmpty(originalText)) continue;

            int randomIndex = Random.Range(0, originalText.Length);
            char randomChar = characters[Random.Range(0, characters.Length)];

            // Replace the character
            char[] textArray = originalText.ToCharArray();
            textArray[randomIndex] = randomChar;
            string flickeredText = new string(textArray);

            // Apply flicker effect
            SetText(element, flickeredText);
            yield return new WaitForSecondsRealtime(0.1f); // Short flicker duration
            SetText(element, originalText); // Restore original text
        }
    }

    private string GetText(VisualElement element)
    {
        if (element is Label label) return label.text;
        if (element is Button button) return button.text;
        return "";
    }

    private void SetText(VisualElement element, string text)
    {
        if (element is Label label) label.text = text;
        if (element is Button button) button.text = text;
    }
}
