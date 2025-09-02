using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Transform spawnParent; // например Canvas или отдельный контейнер
    public GameObject[] drinkPrefabs;
    public GameObject[] iceCreamPrefabs;
    public GameObject[] trayPrefabs;

    public void SpawnDrink(int index)
    {
        Instantiate(drinkPrefabs[index], spawnParent);
    }

    public void SpawnIceCream(int index)
    {
        Instantiate(iceCreamPrefabs[index], spawnParent);
    }

    public void SpawnTray(int index)
    {
        Instantiate(trayPrefabs[index], spawnParent);
    }
}
