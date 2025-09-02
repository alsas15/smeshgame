using UnityEngine;

public enum ItemCategory
{
    Drink,
    IceCream,
    Tray,
    Bottle,
    IceCreamBall,
    Cone,
    Cup
}

public class ItemType : MonoBehaviour
{
    public ItemCategory category;

    [Header("Для бутылок и шариков мороженого")]
    public int subIndex; // 0 = первый вид, 1 = второй и т.д.
}
