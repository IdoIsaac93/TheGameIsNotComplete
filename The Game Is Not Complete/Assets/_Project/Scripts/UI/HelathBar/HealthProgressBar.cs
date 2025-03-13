using System;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthProgressBar : MonoBehaviour
{
    private UIDocument _uiDocument;
    private ProgressBar _healthProgressBar;
    private float _progress = 0f;
    private Color _startColor = Color.red;
    private Color _endColor = Color.green;
    private VisualElement _progressFill;


    private void OnEnable()
    {
        _uiDocument =  GetComponent<UIDocument>();
        _healthProgressBar = _uiDocument.rootVisualElement.Q<ProgressBar>("HealthProgressBar");
        _progressFill = _healthProgressBar.Q<VisualElement>("ProgressFill");

        //Start Animation 
        InvokeRepeating(nameof(UpdateProgressBar), 0f, 0.02f);
    }

    void UpdateProgressBar()
    {
        _progress = Mathf.Lerp(_progress, 100f, Time.deltaTime * 0.5f);
        _healthProgressBar.value = _progress;

        //Change collor dynamically
        if (_progressFill != null)
        {
            Color _currentColor = Color.Lerp(_startColor, _endColor, _progress / 100f);
            _progressFill.style.backgroundColor = new StyleColor(_currentColor);
        }

        if (_progress >= 99.5f)
        {
            _progress = 0f;
        }
    }
}
