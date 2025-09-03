using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Draggable2D : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging;

    [Header("Drop logic")]
    public LayerMask trayMask; // назначь здесь слой Tray в инспекторе
    public LayerMask tableZoneMask;
    public LayerMask cupMask;
    public LayerMask waffleMask;
    public bool snapToTrayCenter = true;   // привязка к центру коллайдера подноса
    public bool snapToZoneCenter = true;

    private Collider2D selfCol;
    private Camera cam;

    private TableZone currentZone;

    void Awake()
    {
        selfCol = GetComponent<Collider2D>();
        cam = Camera.main;
        if (cam.orthographic == false)
        {
            // Для 2D лучше ортографическая камера
            Debug.LogWarning("Camera is Perspective. Consider Orthographic for 2D.");
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
        var mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mouseWorld.x, mouseWorld.y, transform.position.z);

        if (currentZone != null)
        {
            currentZone.isOccupied = false;
            currentZone = null;
        }
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;
        var mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mouseWorld.x, mouseWorld.y, transform.position.z) + offset;
    }

void OnMouseUp()
{
    isDragging = false;

    // Проверяем тип предмета
    ItemType type = GetComponent<ItemType>();
    if (type == null) return;

    // ====== СЛУЧАЙ 1: НЕ ПОДНОС ======
    if (type.category != ItemCategory.Tray)
    {
        // --- ИНГРЕДИЕНТ: бутылка ---
        if (type.category == ItemCategory.Bottle)
        {
            Collider2D cupHit = Physics2D.OverlapPoint(transform.position, cupMask);
            if (cupHit != null)
            {
                Cup cup = cupHit.GetComponent<Cup>();
                if (cup != null && !cup.isFilled)
                {
                    cup.Fill(type.subIndex);   // стакан наполняется
                    Destroy(gameObject);       // бутылка исчезает
                    return;
                }
            }
        }

        // --- ИНГРЕДИЕНТ: шарик мороженого ---
        if (type.category == ItemCategory.IceCream)
        {
            Collider2D waffleHit = Physics2D.OverlapPoint(transform.position, waffleMask);
            if (waffleHit != null)
            {
                Waffle waffle = waffleHit.GetComponent<Waffle>();
                if (waffle != null && !waffle.isFilled)
                {
                    waffle.Fill(type.subIndex);   // рожок наполняется
                    Destroy(gameObject);          // шарик исчезает
                    return;
                }
            }
        }

// --- СТАВИМ CUP или WAFFLE НА ПОДНОС ---
var results = new Collider2D[8];
var filter = new ContactFilter2D();
filter.SetLayerMask(trayMask);
filter.useLayerMask = true;
filter.useTriggers = true;

int hits = selfCol.Overlap(filter, results);
if (hits > 0)
{
    foreach (var hit in results)
    {
        if (hit == null) continue;

        Tray tray = hit.GetComponent<Tray>();
        if (tray == null) continue;

        // 🔹 Убедимся, что поднос связан с зоной
        if (tray.currentZone == null)
        {
            var zoneCollider = Physics2D.OverlapPoint(tray.transform.position, tableZoneMask);
            if (zoneCollider != null)
            {
                TableZone zone = zoneCollider.GetComponent<TableZone>();
                if (zone != null)
                {
                    tray.currentZone = zone;
                    zone.AssignTray(tray);
                }
            }
        }

        // 🔹 Привязка предмета к центру подноса
        if (snapToTrayCenter)
        {
            Vector3 trayCenter = hit.bounds.center;
            Vector3 off = Vector3.zero;

            if (type.category == ItemCategory.Drink)
                off = new Vector3(0.9f, 0.7f, 0f); // справа
            else if (type.category == ItemCategory.IceCream)
                off = new Vector3(-1.1f, 0.4f, 0f); // слева

            transform.position = trayCenter + off;
        }

        // 🔹 Регистрируем предмет на подносе (после привязки зоны!)
        Cup cup = GetComponent<Cup>();
        if (cup != null)
        {
            tray.AddCup(cup);
        }

        Waffle waffle = GetComponent<Waffle>();
        if (waffle != null)
        {
            tray.AddWaffle(waffle);
        }

        return;
    }
}

// если не попали никуда → уничтожаем
Destroy(gameObject);
return;

    }

// ====== СЛУЧАЙ 2: ПОДНОС ======
{
    var results = new Collider2D[3];
    var filter = new ContactFilter2D();
    filter.SetLayerMask(tableZoneMask);
    filter.useLayerMask = true;
    filter.useTriggers = true;

    int hits = selfCol.Overlap(filter, results);
    Debug.Log($"Поднос проверяет зоны: найдено {hits} коллайдера(ов)");

    if (hits > 0)
    {
        foreach (var hit in results)
        {
            if (hit == null)
            {
                Debug.Log("Пропущен null коллайдер");
                continue;
            }

            TableZone zone = hit.GetComponent<TableZone>();
            if (zone == null)
            {
                Debug.Log("У объекта нет TableZone");
                continue;
            }

            Debug.Log($"TableZone: {zone.name}, isOccupied={zone.isOccupied}, isTrayOccupied={zone.isTrayOccupied}, isSeatOnly={zone.isSeatOnly}");

            // проверяем флаг подноса отдельно
            if (!zone.isTrayOccupied && !zone.isSeatOnly)
            {
                Vector3 zoneCenter = hit.bounds.center;
                transform.position = new Vector3(zoneCenter.x, zoneCenter.y, transform.position.z);

                Tray myTray = GetComponent<Tray>();
                if (myTray != null)
                {
                    zone.AssignTray(myTray);
                }

                currentZone = zone;
                Customer customer = zone.GetCustomer();


                Debug.Log($"ℹ Поднос {myTray.name} установлен на зону {zone.name}, связанный клиент: {customer.name} (спрайт: {customer.GetCurrentSpriteName()})");

                // ⚡ Убираем проверку заказа здесь
                // Проверка будет происходить позже при наполнении стакана или вафли
                // Если хочешь, можно добавить лог для отслеживания:
                Debug.Log("ℹ Заказ клиента пока не проверяется — поднос только установлен");

                return;
            }
            else
            {
                Debug.Log("Зона занята подносом или помечена как isSeatOnly");
            }
        }
    }
    else
    {
        Debug.Log("Коллайдеры TableZone не найдены");
    }

    // если поднос не попал в зону → удаляем
    Destroy(gameObject);
    Debug.Log("Поднос не попал ни в одну зону и удалён");
}
}
}