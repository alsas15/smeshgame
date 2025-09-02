using UnityEngine;
using UnityEngine.EventSystems; // 👈 добавлено

[RequireComponent(typeof(PlayerAnimator))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;

    private Vector3 targetPosition;
    private bool isMoving = false;

    private Camera cam;
    public PlayerAnimator animator;
    private DanceTrigger danceTrigger; // 👈 добавлено

    void Start()
    {
        cam = Camera.main;
        animator = GetComponent<PlayerAnimator>();
        danceTrigger = GetComponent<DanceTrigger>(); // 👈 берём ссылку
        targetPosition = transform.position;
    }

    void Update()
    {
        HandleInput();
        HandleMovement();
    }

    void HandleInput()
    {
        // Если клик по UI — игнорируем
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return; // ⛔ Не двигаем игрока
        }

        // Если в фокусе какой-то UI элемент (например, поле ввода чата) — игнорируем
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            if (EventSystem.current.currentSelectedGameObject.GetComponent<TMPro.TMP_InputField>() != null)
                return;
        }

        // Обработка клика для перемещения
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
            worldPos.z = 0f;
            targetPosition = worldPos;
            isMoving = true;

            // 🛑 Останавливаем танец, если он активен
            animator.isSitting = false;
            animator.isDancing = false;

            if (danceTrigger != null)
            {
                danceTrigger.StopDance();
            }
        }

        // Горячие клавиши (оставил твои)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.isDancing = !animator.isDancing;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.isSitting = true;
            animator.sitDirection = "se"; // можно менять
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.isSitting = false;
        }
    }

    void HandleMovement()
    {
        if (!isMoving || animator.isSitting || animator.isDancing) return;

        Vector3 dir = targetPosition - transform.position;
        float distance = dir.magnitude;

        if (distance < 0.05f)
        {
            isMoving = false;
            return;
        }

        dir.Normalize();
        transform.position += dir * moveSpeed * Time.deltaTime;

        animator.currentDirection = AngleToDirection(dir);
    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("NoWalk"))
    {
        // Отменяем движение
        isMoving = false;
        targetPosition = transform.position;
    }
}

    string AngleToDirection(Vector3 dir)
    {
        float angle = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
        angle = (angle + 360) % 360;

        if (angle >= 337.5f || angle < 22.5f) return "e";
        if (angle < 67.5f) return "ne";
        if (angle < 112.5f) return "n";
        if (angle < 157.5f) return "nw";
        if (angle < 202.5f) return "w";
        if (angle < 247.5f) return "sw";
        if (angle < 292.5f) return "s";
        return "se";
    }
}
