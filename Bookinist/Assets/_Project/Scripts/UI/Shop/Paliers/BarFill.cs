using TMPro;
using UnityEngine;

public class BarFill : MonoBehaviour
{
    public float Width, Height, curXP, maxXP;

    [SerializeField]
    private RectTransform FillBar;

    public void RefreshBarUI()
    {

        float newHeight = (curXP / maxXP) * Height;
        //Mathf.Lerp()
        FillBar.sizeDelta = new Vector2(Width, newHeight);
    }

    public void ModifCur(int newVal)
    {
        curXP = Mathf.Clamp(curXP += newVal, 0, maxXP);
    }
}
