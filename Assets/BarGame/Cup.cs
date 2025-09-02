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

    public void Fill(int drinkIndex)
    {
        if (!isFilled && drinkIndex >= 0 && drinkIndex < drinkSprites.Length)
        {
            sr.sprite = drinkSprites[drinkIndex];
            isFilled = true;
            filledIndex = drinkIndex; // сохраняем индекс

            // уведомляем клиента, если стакан на подносе
            if (tray != null && tray.currentZone != null)
            {
                Customer customer = tray.currentZone.GetCustomer();
                if (customer != null)
                {
                    customer.CheckTray(tray);
                    Debug.Log($"Поднос {tray.name} обновлен клиентом {customer.name} после наполнения стакана");
                }
            }
        }
    }
}
