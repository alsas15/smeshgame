using UnityEngine;
using System.Collections.Generic;

public class Tray : MonoBehaviour
{
    public List<Cup> cups = new List<Cup>();
    public List<Waffle> waffles = new List<Waffle>();
    public TableZone currentZone; // зона, на которой находится поднос

    // вызывается при установке стакана на поднос
public void AddCup(Cup cup)
{
    if (!cups.Contains(cup))
    {
        cups.Add(cup);

        string zoneName = currentZone != null ? currentZone.name : "—";
        Customer customer = currentZone != null ? currentZone.GetCustomer() : null;
        string customerName = customer != null ? customer.name : "—";
        string spriteName = (customer != null && customer.GetCurrentSpriteName() != null) 
                            ? customer.GetCurrentSpriteName() : "—";

        Debug.Log($"Стакан {cup.name} добавлен на поднос {name} на зоне {zoneName} с клиентом {customerName} (спрайт: {spriteName})");
    }
    else
    {
        Debug.LogError($"Ошибка: стакан {cup.name} уже есть на подносе, не добавлен повторно");
    }
}



    public void AddWaffle(Waffle waffle)
    {
        if (!waffles.Contains(waffle))
        {
            waffles.Add(waffle);
            Debug.Log($"Вафля {waffle.name} добавлена на поднос");
        }
        else
        {
            Debug.LogError($"Ошибка: вафля {waffle.name} уже есть на подносе, не добавлена повторно");
        }
    }

    public Order GetOrder()
    {
        Order order = new Order
        {
            drinkIndex = -1,      // по умолчанию пустой
            icecreamIndex = -1    // по умолчанию пустой
        };

        // берём первый заполненный стакан
        foreach (var cup in cups)
        {
            if (cup != null && cup.isFilled)
            {
                order.drinkIndex = cup.filledIndex;
                break;
            }
        }

        // берём первую заполненную вафлю
        foreach (var waffle in waffles)
        {
            if (waffle != null && waffle.isFilled)
            {
                order.icecreamIndex = waffle.filledIndex;
                break;
            }
        }

        return order;
    }
    public void TryCheckOrders()
{
    if (currentZone == null) return;

    Customer customer = currentZone.GetCustomer();
    if (customer == null) 
    {
        Debug.Log("ℹ Поднос на зоне, но клиент ещё не сидит — проверка заказа откладывается.");
        return;
    }

    // Лог, показывающий связанного клиента и его спрайт
    Debug.Log($"⚡ Поднос {name} связан с клиентом {customer.name} (спрайт: {customer.GetCurrentSpriteName()})");

    // Проверяем стаканы
    foreach (var cup in cups)
    {
        if (cup != null && cup.isFilled)
        {
            Debug.Log($"⚡ Стакан {cup.name} у клиента {customer.name}, проверяем заказ.");
            customer.CheckTray(this);
            break; // проверяем только один стакан на Tray
        }
    }

    // Проверяем вафли
    foreach (var waffle in waffles)
    {
        if (waffle != null && waffle.isFilled)
        {
            Debug.Log($"⚡ Вафля {waffle.name} у клиента {customer.name}, проверяем заказ.");
            customer.CheckTray(this);
            break;
        }
    }
}


    public void Clear()
    {
        foreach (var cup in cups)
        {
            if (cup != null) Destroy(cup.gameObject);
        }
        foreach (var waffle in waffles)
        {
            if (waffle != null) Destroy(waffle.gameObject);
        }

        cups.Clear();
        waffles.Clear();
    }
}
