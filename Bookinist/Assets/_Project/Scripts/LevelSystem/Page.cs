using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour
{
    [SerializeField] List<GameObject> pageObjects;
    [SerializeField] int pageIndex;
    SpriteRenderer pageRenderer;
    int offset = 0;

    private void Start()
    {
        foreach (GameObject page in pageObjects)
        {
            pageRenderer = page.gameObject.GetComponent<SpriteRenderer>();
            pageRenderer.sortingOrder = pageIndex + offset;
            offset += 1;
        }
    }
}
