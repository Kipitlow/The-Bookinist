using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    [SerializeField] List<GameObject> layerHolder;
    public static int layerSpread = 10;
    public static int maxLayer;
    Transform layerPos;
    int offset = 0;

    private void Awake()
    {
        maxLayer = layerHolder.Count;
    }

    private void Start()
    {
        foreach (GameObject layer in layerHolder)
        {
            layerPos = layer.gameObject.GetComponent<Transform>();
            layerPos.position = new Vector3(0, 0, offset * layerSpread);
            offset += 1;
        }
    }
}
