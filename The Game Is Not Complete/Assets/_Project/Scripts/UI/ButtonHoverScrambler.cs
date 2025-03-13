using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonHoverScrambler : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField, Range(0.05f, 1f)] private float scrambleDuration = 2f;
    [SerializeField, Range(0.05f, 1f)] private float scrambleInterval = 0.1f;
    private string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=[]{}|;:',.<>?/`~";


    // Store each button's original text and any running scramble coroutine.
    private Dictionary<Button, string> originalTexts = new Dictionary<Button, string>();
    private Dictionary<Button, Coroutine> runningCoroutines = new Dictionary<Button, Coroutine>();

    private void Start()
    {
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument is not assigned!");
            return;
        }

        var root = uiDocument.rootVisualElement;
        var buttons = root.Query<Button>().ToList();

        foreach (var button in buttons)
        {
            originalTexts[button] = button.text;
            button.RegisterCallback<PointerEnterEvent>(evt => StartScramble(button));
            button.RegisterCallback<PointerLeaveEvent>(evt => StopScramble(button));
        }
    }

    private void StartScramble(Button button)
    {
        if (runningCoroutines.ContainsKey(button))
            return;

        Coroutine scrambleRoutine = StartCoroutine(ScrambleRoutine(button));
        runningCoroutines[button] = scrambleRoutine;
    }

    private void StopScramble(Button button)
    {
        if (runningCoroutines.TryGetValue(button, out Coroutine coroutine))
        {
            StopCoroutine(coroutine);
            runningCoroutines.Remove(button);
        }

        if (originalTexts.TryGetValue(button, out string original))
        {
            button.text = original;
        }
    }

    private IEnumerator ScrambleRoutine(Button button)
    {
        string originalText = originalTexts[button];
        float elapsedTime = 0f;

        while (elapsedTime < scrambleDuration)
        {
            char[] scrambled = originalText.ToCharArray();
            for (int i = 0; i < scrambled.Length; i++)
            {
                if (Random.value < 0.5f)
                {
                    scrambled[i] = characters[Random.Range(0, characters.Length)];
                }
            }

            button.text = new string(scrambled);
            yield return new WaitForSecondsRealtime(scrambleInterval);

            elapsedTime += scrambleInterval;
        }

        StopScramble(button);  // Restore text after the duration
    }
}