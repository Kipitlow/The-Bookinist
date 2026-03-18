using UnityEngine;
using TMPro;
using System;
using System.Collections; // Permet d'utiliser une Coroutine


[Serializable]
public class Tache_Layourt
{
    [Header("Inscription Tache")]
    [SerializeField] public GameObject[] AllObject;
}

public class SC_Tache : MonoBehaviour
{
    [Header("Variable Utiliser pour le Chronomêtre")]
    [SerializeField] public TextMeshProUGUI Text_Chronom;
    public int totalSeconds;
    [Header("Autre")]
    public Camera CM_Player;
    public Tache_Layourt[] TL;
    public int Layourt;


    void Start()
    {
        if(CM_Player==null)CM_Player = GameObject.Find("CameraManager").GetComponent<Camera>();
        StartCoroutine("Chronometre"); //Permet de lancer la coroutine;
    }
    void Update()
    {
        /*if(CM_Player != null)
        {
            Vector3 Camera = CM_Player.transform.position;
            if(Camera.z/20 <0)
            {
                
            }
            else
            {

            }
            Debug.Log($"Position Camera{Camera.z}");
        }*/
    }

    public void Panel_Tache(GameObject GO) // Ce code permet uniquement de cacher ou non le panel des Taches
    {
        if(GO!=null) GO.SetActive(!GO.activeSelf);
    }

    IEnumerator Chronometre()
    {
        
        if(totalSeconds-1 >=1)
        {
            totalSeconds-=1;
            Text_Chronom.text = $"Time: {totalSeconds/60}min {totalSeconds%60}Sec";
            yield return new WaitForSeconds(1);
            StartCoroutine("Chronometre");
        }
        else
        {
            StopCoroutine("Chronometre");
        }
    }
}
