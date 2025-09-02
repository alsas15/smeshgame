using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror; // обязательно для сетевого запуска

public class LoginUI : MonoBehaviour
{
    public TMP_InputField nicknameInput; // поле ввода ника
    public Button startButton;           // кнопка "Начать игру"
    public GameObject loginCanvas;       // ссылка на весь Canvas

    void Start()
    {
        startButton.onClick.AddListener(OnStartClicked); // привязка кнопки
    }

    public void OnStartClicked()
    {
        string nick = nicknameInput.text.Trim();

        if (string.IsNullOrEmpty(nick))
        {
            Debug.Log("Ник не введён!");
            return;
        }

        PlayerPrefs.SetString("Nickname", nick); // сохраняем ник

        // запускаем хост-сессию (если игра сетовая и ты хост)
        NetworkManager.singleton.StartHost();

        // отключаем UI (LoginCanvas)
        loginCanvas.SetActive(false);
    }
}
