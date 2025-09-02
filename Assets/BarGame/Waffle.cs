using UnityEngine;

public class Waffle : MonoBehaviour
{
    public Sprite emptySprite;
    public Sprite[] icecreamSprites;

    private SpriteRenderer sr;
    public bool isFilled = false;
    public int filledIndex = -1; // индекс мороженого, которое положено

    [HideInInspector] public Tray tray; // ссылка на поднос, на котором стоит вафля

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = emptySprite;
    }

    public void Fill(int icecreamIndex)
    {
        if (!isFilled && icecreamIndex >= 0 && icecreamIndex < icecreamSprites.Length)
        {
            sr.sprite = icecreamSprites[icecreamIndex];
            isFilled = true;
            filledIndex = icecreamIndex; // сохраняем индекс наполнения

            // уведомляем клиента, если вафля на подносе
            if (tray != null && tray.currentZone != null)
            {
                Customer customer = tray.currentZone.GetCustomer();
                if (customer != null)
                {
                    customer.CheckTray(tray);
                    Debug.Log($"Поднос {tray.name} обновлен клиентом {customer.name} после наполнения вафли");
                }
            }
        }
    }
}
