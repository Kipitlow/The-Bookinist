using NUnit.Framework;
using UnityEngine;

public class SnapPointManager : MonoBehaviour
{
    public GameObject[] snapPoints;
    public int layer; 

    public void SetUpSnapPoints(int layer)
    {
        foreach(GameObject snapPoint in snapPoints)
        {
            Vector3 pos = snapPoint.transform.position;
            snapPoint.transform.position = new Vector3 ( pos.x, pos.y, transform.position.z - 20 * Mathf.Pow(1.35f, layer));
        }
    }
}
