using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Runtime.CompilerServices; // Permet d'utiliser une Coroutine


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
    [SerializeField] public TextMeshProUGUI Text_Layeur;
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
        if(CM_Player != null&& Text_Layeur!=null)
        {
            Text_Layeur.text = $"Layeur: {Mathf.Round(CM_Player.transform.position.z / 20)}";
        }
    }

    public void Panel_Tache(GameObject GO) // Ce code permet uniquement de cacher ou non le panel des Taches
    {
        if(GO!=null) GO.SetActive(!GO.activeSelf);
    }

    IEnumerator Chronometre()
    {
        float Position_x;
        if(totalSeconds-1 >=1)
        {
            totalSeconds-=1;
            Text_Chronom.text = $"Time: {totalSeconds / 60}:{totalSeconds % 60}s";
            
            yield return new WaitForSeconds(1);
            StartCoroutine("Chronometre");
        }
        else
        {
            StopCoroutine("Chronometre");
        }
    }
}
