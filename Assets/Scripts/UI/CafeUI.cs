using UnityEngine;
using UnityEngine.UI;

public class CafeUI : MonoBehaviour
{
    [Header("UI панели")]
    public GameObject confirmPanel;   // Панель подтверждения
    public Button yesButton;          // Кнопка "Да"
    public Button noButton;           // Кнопка "Нет"

    [Header("Кафе и мини-игра")]
    public Button cafeButton;         // Кнопка-иконка кафе (на карте)
    public GameObject cafeMinigame;   // Объект с мини-игрой (или сцена)

    private void Awake()
    {
        if (confirmPanel != null)
            confirmPanel.SetActive(false); // изначально окно скрыто
    }

    private void Start()
    {
        if (cafeButton != null)
            cafeButton.onClick.AddListener(OpenConfirm);

        if (yesButton != null)
            yesButton.onClick.AddListener(StartMinigame);

        if (noButton != null)
            noButton.onClick.AddListener(() => confirmPanel.SetActive(false));
    }

    private void OpenConfirm()
    {
        if (confirmPanel != null)
            confirmPanel.SetActive(true);
    }

    private void StartMinigame()
    {
        if (confirmPanel != null)
            confirmPanel.SetActive(false);

        if (cafeMinigame != null)
        {
            // Если мини-игра сделана как отдельный объект
            cafeMinigame.SetActive(true);
        }
        else
        {
            // Если мини-игра в отдельной сцене
            // UnityEngine.SceneManagement.SceneManager.LoadScene("CafeScene");
            Debug.Log("Запуск мини-игры Кафе!");
        }
    }
}
