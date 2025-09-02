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

    // Вместо корутины — сразу спавним по одному клиенту на каждое место
    foreach (var seat in seats)
    {
        if (seat != null)
        {
            SpawnNextCustomer(seat);
        }
    }
}


    IEnumerator SpawnCustomers()
    {
        Debug.Log("Старт корутины SpawnCustomers");

        for (int i = 0; i < seats.Length; i++)
        {
            Debug.Log($"Спавн клиента {i} - перед WaitForSecondsRealtime, Time.realtimeSinceStartup={Time.realtimeSinceStartup}");
            yield return new WaitForSecondsRealtime(spawnDelay);
            Debug.Log($"Спавн клиента {i} - после WaitForSecondsRealtime, Time.realtimeSinceStartup={Time.realtimeSinceStartup}");

            SpawnCustomerSafe(i);
        }

        Debug.Log("Корутіна SpawnCustomers завершена");
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

    private void SpawnCustomerSafe(int i)
    {
        try
        {
            if (seats[i] == null)
            {
                Debug.LogError($"❌ CustomerManager: seats[{i}] не назначен!");
                return;
            }

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            if (spawnPoint == null)
            {
                Debug.LogError("❌ CustomerManager: один из spawnPoints пуст!");
                return;
            }

            GameObject clientGO = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
            
            // Дебаг после Instantiate
            Debug.Log($"Префаб {clientGO.name} создан на позиции {clientGO.transform.position}, активен: {clientGO.activeSelf}");

            Customer c = clientGO.GetComponent<Customer>();
            if (c == null)
            {
                Debug.LogError("❌ CustomerManager: префаб не содержит скрипта Customer!");
                Destroy(clientGO);
                return;
            }

            Debug.Log($"✅ Создан клиент {c.name} для места {seats[i].name}");

            seats[i].AssignCustomer(c);
            Debug.Log($"Customer {c.name} назначен на место {seats[i].name}");

            c.Init(seats[i].transform.position, seats[i]);
            Debug.Log($"Customer {c.name} начинает движение к месту");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Ошибка при спавне клиента {i}: {ex}");
        }
    }
}
