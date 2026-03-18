using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Runtime.CompilerServices; // Permet d'utiliser une Coroutine


[Serializable]
public class Tache_Layourt
{
    [Header("Inscription Tache")]
    public string[] Objectif;

    public string GetObjectif()
    {
        return Objectif[0];
    }
}

public class SC_Tache : MonoBehaviour
{
    [Header("Variable Utiliser pour le Chronomêtre")]
    [SerializeField] public TextMeshProUGUI Text_Chronom;
    [SerializeField] public TextMeshProUGUI Text_Objectif;
    public int totalSeconds;

    [Header("Autre")]
    public Camera CM_Player;
    public Tache_Layourt[] Tache_Dans_Ce_Layeur;
    private int WaitCondition;


    void Start()
    {
        if(CM_Player==null)CM_Player = GameObject.Find("CameraManager").GetComponent<Camera>();
        StartCoroutine("Chronometre"); //Permet de lancer la coroutine;
        UpdateQuestText();
    }

    void Update()
    {
        //Ce code consiste a vêrifier le layeur du joueur en fonction de sa position axe z.
        //Text_Objectif.text = $"Layeur: {Mathf.Round(CM_Player.transform.position.z / 20)}";
        if(CM_Player != null && Text_Objectif != null && WaitCondition != (int)Mathf.Round(CM_Player.transform.position.z)+1)
        {
            WaitCondition = (int)Mathf.Round(CM_Player.transform.position.z / 20)+1;
            string Message = "";
            switch (WaitCondition)
            {
                case 0:
                    int i = 0;
                    while (i< Tache_Dans_Ce_Layeur[WaitCondition].Objectif.Length)
                    {
                        Message += "\n";
                        Message += Tache_Dans_Ce_Layeur[WaitCondition].Objectif[i];
                        i++;
                    }

                    break;
            }
            

            //Text_Objectif.text = $"{Tache_Dans_Ce_Layeur[RR].Objectif[0]}";
        }
    }

    /*public void Panel_Tache(GameObject GO) // Ce code permet uniquement de cacher ou non le panel des Taches
    {
        if(GO!=null) GO.SetActive(!GO.activeSelf);
    }*/

    IEnumerator Chronometre()
    {
        float Position_x;
        if(totalSeconds-1 >=1)
        {
            totalSeconds-=1;
            Text_Chronom.text = $"{totalSeconds / 60}:{totalSeconds % 60}";
            
            yield return new WaitForSeconds(1);
            StartCoroutine("Chronometre");
        }
        else
        {
            StopCoroutine("Chronometre");
        }
    }

    private void UpdateQuestText()
    {
        Text_Objectif.text = Tache_Dans_Ce_Layeur[0].GetObjectif();
    }
}
