using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;

    private GameObject localPlayer;
    public GameObject LocalPlayer => localPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // опционально
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        // Получить ник из PlayerPrefs
        string nickname = PlayerPrefs.GetString("Nickname", "");

        if (string.IsNullOrEmpty(nickname))
        {
            Debug.LogError("Ник не найден. Сначала пройди через MainMenu сцену.");
            return;
        }

        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : Vector3.zero;

        localPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        // Установка ника
        NicknameDisplay nicknameDisplay = localPlayer.GetComponent<NicknameDisplay>();
        if (nicknameDisplay != null)
        {
            nicknameDisplay.SetNickname(nickname);
        }
    }
}
