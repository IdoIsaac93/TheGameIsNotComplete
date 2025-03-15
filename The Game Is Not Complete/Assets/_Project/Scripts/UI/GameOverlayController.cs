using UnityEngine;
using UnityEngine.UIElements;

public class GameOverlayController : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _root;
    private VisualElement _keyBindingsContainer;
    private Label _buildSpotLabel;
    private Label _upgradeLabel;
    private VisualElement _towerBuildIcons;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement;
        SetupOverlay();
    }

    private void SetupOverlay()
    {
        // Create main container
        var overlayContainer = new VisualElement();
        overlayContainer.AddToClassList("overlay-container");
        _root.Add(overlayContainer);

        // Key Bindings Section
        _keyBindingsContainer = new VisualElement();
        _keyBindingsContainer.AddToClassList("key-bindings");
        overlayContainer.Add(_keyBindingsContainer);

        // Build Spot Binding
        _buildSpotLabel = CreateKeyBinding("B/N", "Change Build Spot");
        // Upgrade Binding
        _upgradeLabel = CreateKeyBinding("H", "Upgrade (Can't choose)");

        // Tower Build Icons
        _towerBuildIcons = new VisualElement();
        _towerBuildIcons.AddToClassList("tower-build-icons");
        overlayContainer.Add(_towerBuildIcons);

        // Numpad Tower Build Indicators
        for (int i = 1; i <= 5; i++)
        {
            var towerIcon = CreateTowerBuildIcon(i);
            _towerBuildIcons.Add(towerIcon);
        }

        // Sell Indicator
        var sellIcon = CreateKeyBinding("0", "Sell Tower");
        _keyBindingsContainer.Add(sellIcon);
    }

    private Label CreateKeyBinding(string key, string description)
    {
        var bindingLabel = new Label
        {
            text = $"{key}: {description}"
        };
        bindingLabel.AddToClassList("key-binding");
        _keyBindingsContainer.Add(bindingLabel);
        return bindingLabel;
    }

    private VisualElement CreateTowerBuildIcon(int towerNumber)
    {
        var towerIcon = new VisualElement();
        towerIcon.AddToClassList("tower-icon");

        var numberLabel = new Label(towerNumber.ToString());
        numberLabel.AddToClassList("tower-number");

        towerIcon.Add(numberLabel);

        var descriptionLabel = new Label($"Build Tower {towerNumber}");
        descriptionLabel.AddToClassList("tower-description");

        towerIcon.Add(descriptionLabel);

        return towerIcon;
    }
}