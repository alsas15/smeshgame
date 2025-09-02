using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OrderUI : MonoBehaviour
{
    public Image drinkImage;
    public Image icecreamImage;

    public Sprite[] cupWithDrinks;     // 4 варианта напитков
    public Sprite[] conesWithIcecream; // 3 варианта мороженого

    [Header("Терпение посетителя")]
    public Image[] patienceDots; // 5 точек
    public float patienceStep = 15f; // каждые 15 секунд тухнет точка

    private Order currentOrder;
    private Customer targetCustomer;
    private int currentDotIndex = 0;
    private Coroutine patienceRoutine;
    public Vector3 offset = new Vector3(0, 2f, 0); // над головой

    public void SetOrder(Order order)
    {
        currentOrder = order;
        drinkImage.sprite = cupWithDrinks[order.drinkIndex];
        icecreamImage.sprite = conesWithIcecream[order.icecreamIndex];
        // сбрасываем точки
        foreach (var dot in patienceDots)
            dot.color = Color.green;

        currentDotIndex = 0;

        // запускаем отсчёт терпения
        if (patienceRoutine != null)
            StopCoroutine(patienceRoutine);

        patienceRoutine = StartCoroutine(PatienceCountdown());
    }

    IEnumerator PatienceCountdown()
{
    while (currentDotIndex < patienceDots.Length)
    {
        yield return new WaitForSeconds(patienceStep);

        // текущая точка становится красной
        patienceDots[currentDotIndex].color = Color.red;
        currentDotIndex++;
    }

    // если все точки потухли — посетитель уходит
    if (targetCustomer != null)
    {
        targetCustomer.LeaveUnhappy();
    }
    else
    {
        Debug.LogWarning("OrderUI: targetCustomer is NULL!");
    }
}


    public Order GetOrder()
    {
        return currentOrder;
    }

    public void AttachToCustomer(Customer customer)
    {
        targetCustomer = customer;
    }

   public void StopAndDestroy()
{
    // Останавливаем таймер терпения, если есть корутина
    StopAllCoroutines();
    Destroy(gameObject);
}


    void Update()
    {
        if (targetCustomer != null)
        {
            Vector3 worldPos = targetCustomer.transform.position + offset;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            transform.position = screenPos;
        }
    }
}

