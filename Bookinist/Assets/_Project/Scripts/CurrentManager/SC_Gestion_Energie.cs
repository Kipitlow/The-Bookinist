using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class SC_Gestion_Energie : MonoBehaviour
{
    [Header("Time")]
    private int Max_Energie= 5;
    private int Energie = 0;
    private float Current = 5;



    DateTime Last_Time;
    TimeSpan Resultat;
    private TextMeshProUGUI T_Horloge;

    void Start()
    {
        T_Horloge = GameObject.Find("T_Horloge").GetComponent<TextMeshProUGUI>();
        if (T_Horloge != null)
        {
            //StartCoroutine(Time_Energie());
        }
        Last_Time = DateTime.Now;
    }

    IEnumerator Time_Energie()
    {
        while (true)
        {

            Resultat = Last_Time - DateTime.Now;
            int point_Energie = (int)Resultat.TotalSeconds / (int)Current;
            T_Horloge.text = $"Energie: {Mathf.Abs(point_Energie)}";
            print("1");


            //T_Horloge.text = Last_Time.ToString("HH:mm:ss");
            //T_Horloge.text = $"Time{Last_Time.ToString("HH:mm:ss")}, Calcule ";
            yield return new WaitForSeconds(1);
        }
    }

    public void BBBBBB()
    {
        Resultat = Last_Time - DateTime.Now;
        int point_Energie = (int)Resultat.TotalSeconds / (int)Current;
        T_Horloge.text = $"Energie: {Mathf.Abs(point_Energie)}";
    }

}
