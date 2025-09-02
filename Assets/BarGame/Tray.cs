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
            Debug.Log($"Стакан {cup.name} добавлен на поднос");
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
