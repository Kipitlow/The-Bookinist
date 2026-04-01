using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;

[Serializable]
public class Saving_Json
{
    public int Energie { set; get; }
    public int Max_Energie = 5;
    public string Last_Time;
}

public class SC_Gestion_Energie : MonoBehaviour
{
    [Header("Time")]
    private TextMeshProUGUI T_Horloge;
    
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

    IEnumerator Time_Energie()
    {
        while (SaveData.Energie < SaveData.Max_Energie)
        {
            SaveData.Energie += 1;
            T_Horloge.text = $"Energie: {SaveData.Energie}";
            yield return new WaitForSeconds(Current);
        }
    }

    
    public void Loading()
    {
        if(File.Exists(NamePath))
        {

            string json_DataPath = File.ReadAllText(NamePath);
            SaveData = JsonUtility.FromJson<Saving_Json>(json_DataPath);
            DateTime last_Timer = DateTime.Parse(SaveData.Last_Time);

            Resultat = DateTime.Now - last_Timer;

            Affiche_Energie();
            StartCoroutine(Time_Energie());
        }
        else
        {
            Debug.LogError("Pas SauveGarde");
        }
    }
    public void Affiche_Energie()
    {
        
        int point_Energie = (int)Resultat.TotalSeconds / (int)Current;
        if(SaveData.Energie + point_Energie >= SaveData.Max_Energie)
        {
            SaveData.Energie = SaveData.Max_Energie;
        }
        else
        {
            SaveData.Energie += point_Energie;
        }
        T_Horloge.text = $"Energie: {Mathf.Abs(SaveData.Energie)}";
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
