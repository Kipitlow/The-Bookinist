using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;


[Serializable]
public class Tache_Layourt
{
    [Header("Inscription Tache")]
    [SerializeField] public GameObject[] AllObject;
}

public class SC_Tache : MonoBehaviour
{
    public Camera CM_Player;
    public Tache_Layourt[] TL;
    public int Layourt;
    void Start()
    {
        if(CM_Player==null)CM_Player = GameObject.Find("CameraManager").GetComponent<Camera>();
    }
    void Update()
    {
        if(CM_Player != null)
        {
            Vector3 Camera = CM_Player.transform.position;
            if(Camera.z/20 <0)
            {
                
            }
            else
            {

            }
            Debug.Log($"Position Camera{Camera.z}");
        }
    }
}
