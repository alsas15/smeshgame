using System.Collections;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform[] spawnPoints;   // LeftSpawn, RightSpawn

    [Header("Seats for Customers")]
    public CustomerSeat[] seats;      // Point1, Point2, Point3

    [Header("Customer Prefab")]
    public GameObject customerPrefab;

    [Header("Spawn Settings")]
    public float spawnDelay = 2f;

    private int seatIndex = 0; // индекс для поочерёдного обхода мест

    void Start()
    {
        Debug.Log("CustomerManager Start");

        if (customerPrefab == null)
        {
            Debug.LogError("❌ CustomerManager: customerPrefab не назначен!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("❌ CustomerManager: spawnPoints пуст! Добавь LeftSpawn и RightSpawn.");
            return;
        }

        if (seats == null || seats.Length == 0)
        {
            Debug.LogError("❌ CustomerManager: seats пуст! Добавь Point1, Point2, Point3.");
            return;
        }

        // запускаем корутину поочередного спавна
        StartCoroutine(SpawnCustomersSequentially());
    }

    private IEnumerator SpawnCustomersSequentially()
    {
        while (true) // бесконечно генерируем клиентов
        {
            CustomerSeat seat = seats[seatIndex];

            // ждём пока место освободится
            while (seat.currentCustomer != null)
            {
                yield return null;
            }

            // небольшая задержка перед приходом нового клиента
            yield return new WaitForSeconds(spawnDelay);

            // создаём нового клиента
            SpawnNextCustomer(seat);

            // переключаемся на следующее место (по кругу)
            seatIndex = (seatIndex + 1) % seats.Length;
        }
    }

    public void SpawnNextCustomer(CustomerSeat seat)
    {
        if (seat == null)
        {
            Debug.LogError("❌ SpawnNextCustomer: seat == null");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject clientGO = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);

        Customer c = clientGO.GetComponent<Customer>();
        if (c == null)
        {
            Debug.LogError("❌ CustomerPrefab не содержит Customer!");
            Destroy(clientGO);
            return;
        }

        Debug.Log($"✅ Новый клиент {c.name} назначен на место {seat.name}");

        seat.AssignCustomer(c);
        c.Init(seat.transform.position, seat);
    }
}
