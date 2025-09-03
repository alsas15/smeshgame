using UnityEngine;

public class Cup : MonoBehaviour
{
    public Sprite emptySprite;
    public Sprite[] drinkSprites;

    private SpriteRenderer sr;
    public bool isFilled = false;
    public int filledIndex = -1; // индекс напитка, который налит

    [HideInInspector] public Tray tray; // ссылка на поднос, на котором стоит стакан

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = emptySprite;
    }

    /// <summary>
    /// Наполнение стакана напитком
    /// </summary>
    /// <param name="drinkIndex">Индекс напитка</param>
    public void Fill(int drinkIndex)
{
    if (!isFilled && drinkIndex >= 0 && drinkIndex < drinkSprites.Length)
    {
        sr.sprite = drinkSprites[drinkIndex];
        isFilled = true;
        filledIndex = drinkIndex;

        Debug.Log($"Стакан {name} наполнен напитком {drinkIndex}, спрайт обновлён на {sr.sprite.name}");

        // ⚡ Проверка заказа теперь будет через Tray
        if (tray != null)
        {
            tray.TryCheckOrders();
        }
    }
}
}
