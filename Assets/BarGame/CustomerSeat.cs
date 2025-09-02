using UnityEngine;

public class CustomerSeat : MonoBehaviour
{
    public Customer currentCustomer;
    public TableZone linkedZone; // ← привяжи в инспекторе нужную TableZone

    public void AssignCustomer(Customer c)
    {
        currentCustomer = c;

        if (linkedZone != null)
        {
            linkedZone.AssignCustomer(c);
        }
    }

    public void ClearSeat()
    {
        if (linkedZone != null)
        {
            linkedZone.FreeCustomer(); // ← исправлено с FreeZone()
        }

        currentCustomer = null;
    }

    public Customer GetCustomer()
    {
        return currentCustomer;
    }
}
