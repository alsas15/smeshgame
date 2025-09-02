using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Порядок: N, NE, E, SE, S, SW, W, NW (8 направлений)
    [SerializeField] private Sprite[] walkSprites = new Sprite[8];

    // Порядок: NE, NW, SE, SW (4 сидячих позы)
    [SerializeField] private Sprite[] sitSprites = new Sprite[4];

    public bool isSitting = false;
    public string sitDirection = "se";

    public bool isDancing = false;
    private float danceTimer = 0f;

    public string currentDirection = "s"; // направление по умолчанию

    void Awake()
    {
        // Автоматическое назначение, если не указано в инспекторе
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer не найден на объекте: " + gameObject.name);
            }
        }
    }

    void Update()
    {
        if (isSitting)
        {
            UpdateSitSprite();
        }
        else if (isDancing)
        {
            UpdateDanceSprite();
        }
        else
        {
            UpdateWalkSprite();
        }
    }

    void UpdateWalkSprite()
    {
        int index = DirectionToIndex(currentDirection);
        if (index >= 0 && index < walkSprites.Length)
        {
            spriteRenderer.sprite = walkSprites[index];
        }
    }

    void UpdateSitSprite()
    {
        int index = SitDirectionToIndex(sitDirection);
        if (index >= 0 && index < sitSprites.Length)
        {
            spriteRenderer.sprite = sitSprites[index];
        }
    }

    void UpdateDanceSprite()
    {
        danceTimer += Time.deltaTime * 10f;
        float offset = Mathf.Sin(danceTimer) * 5f;
        transform.localPosition = new Vector3(offset, 0f, 0f);

        UpdateWalkSprite(); // Можно использовать текущий спрайт для танца
    }

    int DirectionToIndex(string dir)
    {
        switch (dir)
        {
            case "n": return 0;
            case "ne": return 1;
            case "e": return 2;
            case "se": return 3;
            case "s": return 4;
            case "sw": return 5;
            case "w": return 6;
            case "nw": return 7;
            default: return 4;
        }
    }

    int SitDirectionToIndex(string dir)
    {
        switch (dir)
        {
            case "ne": return 0;
            case "nw": return 1;
            case "se": return 2;
            case "sw": return 3;
            default: return 2;
        }
    }
}
