using UnityEngine;

public class TableZone : MonoBehaviour
{
    public bool isOccupied = false;      // место занято клиентом
    public bool isSeatOnly = false;      // только для сидения
    public bool isTrayOccupied = false;  // поднос уже на столе

    private Customer assignedCustomer;
    private Tray assignedTray;

    // Назначить клиента
    public void AssignCustomer(Customer customer)
    {
        isOccupied = true;
        assignedCustomer = customer;
    }

    // Освободить клиента
    public void FreeCustomer()
    {
        isOccupied = false;
        assignedCustomer = null;
    }

    public Customer GetCustomer()
    {
        return assignedCustomer;
    }

    // Назначить поднос
    public void AssignTray(Tray tray)
    {
        assignedTray = tray;
        isTrayOccupied = true;
    }

    // Освободить поднос
    public void FreeTray()
    {
        assignedTray = null;
        isTrayOccupied = false;
    }

    public Tray GetTray()
    {
        return assignedTray;
    }
}
