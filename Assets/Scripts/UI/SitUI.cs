using UnityEngine;
using UnityEngine.UI;

public class SitUI : MonoBehaviour
{
    [Header("Кнопки направлений")]
    public Button buttonNW;
    public Button buttonNE;
    public Button buttonSW;
    public Button buttonSE;

    [Header("Ссылка на контроллер игрока")]
    public PlayerController playerController;

    [Header("UI")]
    public GameObject sitPanel;          // Панель с 4 кнопками
    public Button sitToggleButton;       // Кнопка-иконка, которая открывает/закрывает интерфейс

    void Start()
    {
        // Привязка кнопок
        buttonNW.onClick.AddListener(() => SitInDirection("nw"));
        buttonNE.onClick.AddListener(() => SitInDirection("ne"));
        buttonSW.onClick.AddListener(() => SitInDirection("sw"));
        buttonSE.onClick.AddListener(() => SitInDirection("se"));

        // Привязка кнопки иконки
        if (sitToggleButton != null)
            sitToggleButton.onClick.AddListener(ToggleSitPanel);

        // По умолчанию скрываем панель
        if (sitPanel != null)
            sitPanel.SetActive(false);
    }

    void SitInDirection(string direction)
    {
        // Передаём в PlayerController нужные значения
        if (playerController != null && playerController.animator != null)
        {
            playerController.animator.isSitting = true;
            playerController.animator.isDancing = false;
            playerController.animator.sitDirection = direction;
        }

        // После выбора направления — скрываем панель
        if (sitPanel != null)
            sitPanel.SetActive(false);
    }

    void ToggleSitPanel()
    {
        if (sitPanel != null)
            sitPanel.SetActive(!sitPanel.activeSelf);
    }
}
