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
        float healthRatio = (float)playerResources.GetCurrentHealth() / playerResources.GetMaxHealth();
        float healthPercent = Mathf.Lerp(8, 88, healthRatio);
        m_HealthBarMask.style.width = Length.Percent(healthPercent);
        m_HealthLabel.text = $"{playerResources.GetCurrentHealth()}/{playerResources.GetMaxHealth()}";

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
