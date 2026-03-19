using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CaseView : MonoBehaviour, IPointerClickHandler
{
    public int X { get; set; }
    public int Y { get; set; }

    public Action<int, int> OnCaseClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Case cliquée : {X},{Y}");
        OnCaseClicked?.Invoke(X, Y);
    }
}