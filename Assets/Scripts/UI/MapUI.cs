using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapUI : MonoBehaviour
{
    [Header("Открытие карты")]
    public Button mapButton;          // Кнопка иконки карты (MapButton)
    public GameObject mapPanel;       // Панель карты (MapPanel)
    public Button closeMapButton;     // 👈 Новая кнопка "Закрыть карту"

    [System.Serializable]
    public class LocationBinding
    {
        public string locationName;   // Должен совпадать с Location.name в LocationManager
        public Button button;         // Кнопка на карте
    }

    [Header("Кнопки локаций на карте")]
    public List<LocationBinding> locationButtons = new List<LocationBinding>();

    [Header("Ссылки")]
    public LocationManager locationManager;

    private void Awake()
    {
        if (locationManager == null)
            locationManager = FindObjectOfType<LocationManager>();

        if (mapPanel != null)
            mapPanel.SetActive(false);
    }

    private void Start()
    {
        if (mapButton != null)
            mapButton.onClick.AddListener(ToggleMap);

        if (closeMapButton != null)
            closeMapButton.onClick.AddListener(() => mapPanel.SetActive(false));

        foreach (var lb in locationButtons)
        {
            if (lb != null && lb.button != null)
            {
                string loc = lb.locationName;
                lb.button.onClick.AddListener(() => GoTo(loc));
            }
        }
    }

    public void ToggleMap()
    {
        if (mapPanel != null)
            mapPanel.SetActive(!mapPanel.activeSelf);
    }

    private void GoTo(string locationName)
    {
        if (locationManager != null)
            locationManager.ChangeLocation(locationName);
        else
            Debug.LogWarning("MapUI: не назначен LocationManager.");

        if (mapPanel != null)
            mapPanel.SetActive(false);
    }
}
