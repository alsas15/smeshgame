using UnityEngine;

public class Cone : MonoBehaviour
{
    public int scoopsCount = 0;
    public int maxScoops = 3;

    public Transform scoopParent; // пустой объект, куда будем "класть" шарики
    public Vector3 scoopOffset = new Vector3(0, 0.5f, 0); // смещение каждого шарика

    public void AddScoop(GameObject scoopPrefab)
    {
        if (scoopsCount >= maxScoops) return;

        Vector3 pos = scoopParent.position + scoopOffset * scoopsCount;
        Instantiate(scoopPrefab, pos, Quaternion.identity, scoopParent);

        scoopsCount++;
    }
}
