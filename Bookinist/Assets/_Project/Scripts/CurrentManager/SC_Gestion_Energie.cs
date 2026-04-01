using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

[Serializable]
public class Saving_Json
{
    public int Energie { set; get; }
    public string Last_Time;
}

public class SC_Gestion_Energie : MonoBehaviour
{
    [Header("Time")]
    private TextMeshProUGUI T_Horloge;
    private int Max_Energie= 50;
    private int Energie = 0;
    private float Current = 2; //<-- cette variable est utiliser on tant que delay de recharge energie
    TimeSpan Resultat;
    //DateTime Last_Time;

    [Header("Saving Json")]
    private string NamePath;
    private Saving_Json SaveData = new Saving_Json();

    private void Awake()
    {
        NamePath = Path.Combine(Application.persistentDataPath, "energie.json");
    }


    void Start()
    {
        T_Horloge = GameObject.Find("T_Horloge").GetComponent<TextMeshProUGUI>();
        /*if (T_Horloge != null)
        {
            Last_Time = DateTime.Now;
            StartCoroutine(Time_Energie());
        }*/
        
    }

    /*IEnumerator Time_Energie()
    {
        while (true)
        {

            Resultat = DateTime.Now - Last_Time;
            int point_Energie = (int)Resultat.TotalSeconds / (int)Current;
            T_Horloge.text = $"Energie: {Mathf.Abs(point_Energie)}";
            print("1");


            //T_Horloge.text = Last_Time.ToString("HH:mm:ss");
            //T_Horloge.text = $"Time{Last_Time.ToString("HH:mm:ss")}, Calcule ";
            yield return new WaitForSeconds(1);
        }
    }*/

    
    public void Loading()
    {
        if(File.Exists(NamePath))
        {
            print(SaveData.Last_Time);
            string json_DataPath = File.ReadAllText(NamePath);
            SaveData = JsonUtility.FromJson<Saving_Json>(json_DataPath);
            print(SaveData.Last_Time);
            DateTime last_Timer = DateTime.Parse(SaveData.Last_Time);

            Resultat = DateTime.Now - last_Timer;

            Affiche_Energie();
        }
        else
        {
            Debug.LogError("Pas SauveGarde");
        }
    }
    public void Affiche_Energie()
    {
        
        int point_Energie = (int)Resultat.TotalSeconds / (int)Current;
        T_Horloge.text = $"Energie: {Mathf.Abs(point_Energie)}";
    }

    public void Saving()
    {

        SaveData.Last_Time = DateTime.Now.ToString();
        print(SaveData.Last_Time);
        string JsonTo = JsonUtility.ToJson(SaveData);
        File.WriteAllText(NamePath, JsonTo);
    }

    public void Deleting()
    {
        if (File.Exists(NamePath)) File.Delete(NamePath);
    }

}
