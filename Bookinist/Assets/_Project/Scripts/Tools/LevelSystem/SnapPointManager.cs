using NUnit.Framework;
using UnityEngine;

public class SnapPointManager : MonoBehaviour
{
    public GameObject[] snapPoints;

    public void SetUpSnapPoints()
    {
        foreach(GameObject snapPoint in snapPoints)
        {
            Vector3 pos = snapPoint.transform.position;
            snapPoint.transform.position = new Vector3 ( pos.x, pos.y, this.transform.position.z - 15 );
        }
    }
}
