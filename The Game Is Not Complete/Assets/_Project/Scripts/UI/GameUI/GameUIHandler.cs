using System;
using UnityEngine;
using UnityEngine.UIElements;
public class GameUIHandler : MonoBehaviour
{
    public PlayerResources playerResources;
    public UIDocument _UIDocument;

    private Label m_HealthLabel;
    private VisualElement m_HealthBarMask;



    private void Awake()
    {
        //playerResources = FindFirstObjectByType<PlayerResources>();
        PlayerResources.OnHealthChanged += HealthChanged;
        m_HealthLabel = _UIDocument.rootVisualElement.Q<Label>("HealthLabel");
        m_HealthBarMask = _UIDocument.rootVisualElement.Q<VisualElement>("HealthBarMask");
        HealthChanged();
    }


    private void OnEnable()
    {
        GetUiElements();
    }

    private void HealthChanged()
    {
        float healthRatio = (float)PlayerResources.Instance.GetCurrentHealth() / PlayerResources.Instance.GetMaxHealth();
        float healthPercent = Mathf.Lerp(8, 88, healthRatio);
        m_HealthBarMask.style.width = Length.Percent(healthPercent);
        m_HealthLabel.text = $"{PlayerResources.Instance.GetCurrentHealth()}/{PlayerResources.Instance.GetMaxHealth()}";

    }

    private void OnDestroy()
    {
        //for no memory leaks when the game is closed
        PlayerResources.OnHealthChanged -= HealthChanged;
    }


    private void OnDisable()
    {
        //for no memory leaks when the game is closed
        PlayerResources.OnHealthChanged -= HealthChanged;
    }

    private void GetUiElements() {
        PlayerResources.OnHealthChanged += HealthChanged;
        m_HealthLabel = _UIDocument.rootVisualElement.Q<Label>("HealthLabel");
        m_HealthBarMask = _UIDocument.rootVisualElement.Q<VisualElement>("HealthBarMask");
        HealthChanged();  
    }

}
