using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class CafeGameManager : MonoBehaviour
{
    public static CafeGameManager Instance;

    public int lives = 5;
    public int coins = 0;

    public List<Image> lifeImages;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public TMP_Text coinsText;
    public Image coinImage;

    public GameObject gameOverPanel;

    void Awake()
    {
        try
        {
            Instance = this;
            Debug.Log("CafeGameManager Awake: Instance установлен");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Ошибка в CafeGameManager.Awake: " + e);
        }
    }

    void Start()
    {
        try
        {
            UpdateLivesUI();
            UpdateCoinsUI();
            Debug.Log("CafeGameManager Start: Инициализация UI завершена");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Ошибка в CafeGameManager.Start: " + e);
        }
    }

    public void AddCoin()
    {
        coins++;
        UpdateCoinsUI();
        Debug.Log($"Монета добавлена. Текущее количество монет: {coins}");
    }

    public void LoseLife()
    {
        lives--;
        UpdateLivesUI();
        Debug.Log($"Жизнь потеряна. Осталось жизней: {lives}");

        if (lives <= 0)
        {
            Debug.Log("Жизни закончились. GameOver вызывается");
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);
        }
    }

    void UpdateLivesUI()
    {
        if (lifeImages == null || lifeImages.Count == 0)
        {
            Debug.LogWarning("lifeImages не назначены!");
            return;
        }

        for (int i = 0; i < lifeImages.Count; i++)
        {
            if (lifeImages[i] != null)
                lifeImages[i].sprite = i < lives ? fullHeart : emptyHeart;
        }
        Debug.Log("UI жизней обновлён");
    }

    void UpdateCoinsUI()
    {
        if (coinsText == null)
        {
            Debug.LogWarning("coinsText не назначен!");
            return;
        }

        coinsText.text = coins.ToString();
        Debug.Log("UI монет обновлён");
    }
}
