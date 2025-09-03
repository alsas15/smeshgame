using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour
{
    public Order currentOrder;
    public Sprite[] customerSprites;
    private SpriteRenderer spriteRenderer;

    [Header("UI")]
    public OrderUI orderUIPrefab; // префаб окна заказа
    private OrderUI orderUIInstance;

    private Vector3 seatPosition;
    private float moveDuration = 2f;
    private CustomerSeat assignedSeat;

    // ------------------------
    // Названия мороженого и напитков
    // ------------------------
    public string GetCurrentSpriteName()
{
    return spriteRenderer != null && spriteRenderer.sprite != null 
        ? spriteRenderer.sprite.name 
        : "нет спрайта";
}

    private string GetIcecreamName(int index)
    {
        switch (index)
        {
            case 0: return "vanilla_0";
            case 1: return "chocolate_0";
            case 2: return "strawberry_0";
            default: return $"Unknown({index})";
        }
    }

    private string GetDrinkName(int index)
    {
        switch (index)
        {
            case 0: return "cola_0";
            case 1: return "juice_0";
            case 2: return "water_0";
            case 3: return "tea_0";
            default: return $"Unknown({index})";
        }
    }

    // ------------------------
    // Unity Start
    // ------------------------
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetRandomSprite();
        Debug.Log($"Customer {name} создан со спрайтом {spriteRenderer.sprite.name}");
    }

    // ------------------------
    // Инициализация позиции и места
    // ------------------------
    public void Init(Vector3 seatPos, CustomerSeat seat)
    {
        Debug.Log($"Init клиента {name}: целевая позиция {seatPos}");

        seatPosition = seatPos;
        assignedSeat = seat;

        if (assignedSeat != null)
        {
            Debug.Log($"Назначаем клиента {name} на место {assignedSeat.name}");
            assignedSeat.AssignCustomer(this);
        }
        else
        {
            Debug.LogWarning($"Init: для клиента {name} место не назначено!");
        }

        Debug.Log($"Запуск корутины MoveToSeat для клиента {name}");
        StartCoroutine(MoveToSeat());
    }

    // ------------------------
    // Движение к месту
    // ------------------------
    IEnumerator MoveToSeat()
    {
        Debug.Log($"Start движения клиента {name} с позиции {transform.position} к {seatPosition}");
        Vector3 start = transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            transform.position = Vector3.Lerp(start, seatPosition, t);
            yield return null;
        }

        transform.position = seatPosition;
        Debug.Log($"Клиент {name} достиг позиции {seatPosition}");

        // создаём заказ
        currentOrder = new Order
        {
            drinkIndex = Random.Range(0, 4),
            icecreamIndex = Random.Range(0, 3)
        };
        Debug.Log($"Заказ клиента {name}: {currentOrder.drinkIndex} напиток, {currentOrder.icecreamIndex} мороженое");

        // создаём окно заказа
        if (orderUIPrefab != null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            orderUIInstance = Instantiate(orderUIPrefab, canvas.transform);
            orderUIInstance.AttachToCustomer(this);
            orderUIInstance.SetOrder(currentOrder);
            Debug.Log($"OrderUI создан для клиента {name}");
        }
        else
        {
            Debug.LogError("❌ OrderUI Prefab не назначен у Customer!");
        }
    }

    // ------------------------
    // Описание заказа для логов
    // ------------------------
    private string DescribeOrder(Order order)
    {
        string icecream = order.icecreamIndex >= 0 ? GetIcecreamName(order.icecreamIndex) : "—";
        string drink = order.drinkIndex >= 0 ? GetDrinkName(order.drinkIndex) : "—";
        return $"Мороженое: {icecream}, Напиток: {drink}";
    }

    // ------------------------
    // Проверка подноса
    // ------------------------
public void CheckTray(Tray tray)
{
    if (tray == null) return;

    Order trayOrder = tray.GetOrder();

    if (currentOrder == null)
    {
        Debug.LogError("❌ У клиента нет заказа, но пришёл поднос!");
        return;
    }

    // Проверяем напиток
    if (currentOrder.drinkIndex != -1)
    {
        if (trayOrder.drinkIndex != currentOrder.drinkIndex)
        {
            Debug.LogError(
                $"❌ Ошибка: напиток не совпадает!\n" +
                $"В заказе: {currentOrder.drinkIndex}, в стакане: {trayOrder.drinkIndex}"
            );
            return; // клиент недоволен
        }
        else
        {
            Debug.Log("✅ Напиток соответствует заказу.");
        }
    }

    // Проверяем мороженое
    if (currentOrder.icecreamIndex != -1)
    {
        if (trayOrder.icecreamIndex != currentOrder.icecreamIndex)
        {
            Debug.LogError(
                $"❌ Ошибка: мороженое не совпадает!\n" +
                $"В заказе: {currentOrder.icecreamIndex}, в вафле: {trayOrder.icecreamIndex}"
            );
            return; // клиент недоволен
        }
        else
        {
            Debug.Log("✅ Мороженое соответствует заказу.");
        }
    }

    // Если дошли сюда — заказ собран правильно
    Debug.Log("🎉 Заказ полностью выполнен!");
    Serve(trayOrder, true);
}



    // ------------------------
    // Проверка стакана
    // ------------------------
    public void CheckCup(Cup cup)
    {
        if (!cup.isFilled)
        {
            Debug.Log($"{name}: стакан пустой");
            return;
        }

        Debug.Log($"{name} проверяет стакан {cup.name} с напитком {GetDrinkName(cup.filledIndex)}");

        if (cup.filledIndex != currentOrder.drinkIndex)
        {
            Debug.LogWarning($"❌ Напиток не соответствует заказу. На подносе: {GetDrinkName(cup.filledIndex)}, Заказ: {GetDrinkName(currentOrder.drinkIndex)}");
            LeaveUnhappy();
        }
        else
        {
            Debug.Log($"✅ Напиток соответствует заказу");
            Serve(new Order { drinkIndex = cup.filledIndex, icecreamIndex = -1 }, true);
        }

        cup.isFilled = false;
        Destroy(cup.gameObject);
    }

    // ------------------------
    // Проверка вафли
    // ------------------------
    public void CheckWaffle(Waffle waffle)
    {
        if (!waffle.isFilled)
        {
            Debug.Log($"{name}: вафля пустая");
            return;
        }

        Debug.Log($"{name} проверяет вафлю {waffle.name} с мороженым {GetIcecreamName(waffle.filledIndex)}");

        if (waffle.filledIndex != currentOrder.icecreamIndex)
        {
            Debug.LogWarning($"❌ Мороженое не соответствует заказу. На подносе: {GetIcecreamName(waffle.filledIndex)}, Заказ: {GetIcecreamName(currentOrder.icecreamIndex)}");
            LeaveUnhappy();
        }
        else
        {
            Debug.Log($"✅ Мороженое соответствует заказу");
            Serve(new Order { drinkIndex = -1, icecreamIndex = waffle.filledIndex }, true);
        }

        waffle.isFilled = false;
        Destroy(waffle.gameObject);
    }

    // ------------------------
    // Завершение заказа
    // ------------------------
    private void Serve(Order trayOrder, bool success)
    {
        if (orderUIInstance == null)
        {
            Debug.LogWarning($"{name}: UI заказа не найден");
            return;
        }

        if (success)
        {
            Debug.Log($"{name}: заказ успешно выполнен");
            CafeGameManager.Instance.AddCoin();
            StartCoroutine(Leave());
        }
        else
        {
            Debug.Log($"{name}: заказ выполнен неверно, клиент недоволен");
            LeaveUnhappy();
        }
    }

    public void LeaveUnhappy()
    {
        Debug.Log($"{name} уходит недовольный");
        CafeGameManager.Instance.LoseLife();
        StartCoroutine(Leave());
    }

    // ------------------------
    // Уход клиента
    // ------------------------
IEnumerator Leave()
{
    Debug.Log($"{name} начинает уход");

    if (orderUIInstance != null)
    {
        orderUIInstance.StopAndDestroy();
        orderUIInstance = null;
        Debug.Log($"{name}: UI заказа удалён");
    }

    CustomerSeat seatToFree = assignedSeat;
    if (seatToFree != null)
    {
        seatToFree.ClearSeat();
        assignedSeat = null;
        Debug.Log($"{name}: место освобождено");
    }

    Vector3 start = transform.position;
    Vector3 target = start + (start.x < 0 ? Vector3.left : Vector3.right) * 10f;

    float t = 0f;
    while (t < 1f)
    {
        t += Time.deltaTime / moveDuration;
        transform.position = Vector3.Lerp(start, target, t);
        yield return null;
    }

    Destroy(gameObject);
    Debug.Log($"{name} покинул сцену и удалён");

    // ⚡ после удаления клиента заспавнить нового
    //if (seatToFree != null)
    //{
        //CustomerManager cm = FindObjectOfType<CustomerManager>();
        //if (cm != null)
        //{
            //cm.SpawnNextCustomer(seatToFree);
        //}
    //}
}


    // ------------------------
    // Случайный спрайт
    // ------------------------
    void SetRandomSprite()
    {
        if (customerSprites.Length == 0) return;
        int index = Random.Range(0, customerSprites.Length);
        spriteRenderer.sprite = customerSprites[index];
        Debug.Log($"{name}: выбран спрайт {spriteRenderer.sprite.name}");
    }
}
