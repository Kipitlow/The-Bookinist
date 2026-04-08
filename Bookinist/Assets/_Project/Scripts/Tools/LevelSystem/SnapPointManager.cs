using UnityEngine;

/// <summary>
/// Ajuste la position Z des snap points pour qu'ils suivent le layer parent.
/// </summary>
public class SnapPointManager : MonoBehaviour
{
    #region Variables

    public GameObject[] snapPoints;

    #endregion

    #region Methods

    public void SetUpSnapPoints()
    {
        if (snapPoints == null) return;
        foreach (GameObject snapPoint in snapPoints)
        {
            if (snapPoint == null) continue;
            Vector3 pos = snapPoint.transform.position;
            snapPoint.transform.position = new Vector3(pos.x, pos.y, transform.position.z - 15);
        }
    }

    #endregion
}
