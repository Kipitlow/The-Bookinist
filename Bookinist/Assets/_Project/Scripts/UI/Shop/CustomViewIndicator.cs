using UnityEngine;
using UnityEngine.UI;

public class CustomViewIndicator : MonoBehaviour
{
    [SerializeField] private CamManager _camManager;

    public void UpdateViewIndicator()
    {
        Image[] allViews = GetComponentsInChildren<Image>();
        foreach (Image view in allViews)
        {
            if(view == allViews[_camManager.GetCurrentIndexView()])
            {
                view.color = Color.gray;
            }
            else
            {
                view.color = Color.white;
            }
        }
    }
}
