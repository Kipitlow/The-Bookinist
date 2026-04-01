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
    public string Last_Time;
}

public class SC_Gestion_Energie : MonoBehaviour
{
    [Header("Time")]
    private TextMeshProUGUI T_Energie;
    private TextMeshProUGUI T_Horloge;
    public int Max_Energie;
    public float Current = 5; //<-- cette variable est utiliser on tant que delay de recharge energie
    float Temps_Restant;
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
        T_Energie = GameObject.Find("T_Energie").GetComponent<TextMeshProUGUI>();
        T_Horloge = GameObject.Find("T_Horloge").GetComponent<TextMeshProUGUI>();
        Loading();
        /*if (T_Horloge != null)
        {
            Last_Time = DateTime.Now;
            StartCoroutine(Time_Energie());
        }*/

    }
    private void Update()
    {
        //T_Horloge.text = $"Energie: {SaveData.Energie}";//<-voir l'interval avec une autre valeur
    }
    IEnumerator Time_Energie()
    {
        
        if (Temps_Restant > 0)// la on veut savoir si il reste du temps
        {
            yield return new WaitForSeconds(Mathf.Abs(Temps_Restant - Current));
        }
        while (SaveData.Energie < Max_Energie)
        {
            print($"waitA1:{Current}");
            SaveData.Energie += 1;
            T_Energie.text = $"Energie: {SaveData.Energie}";
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
        
        int totalSeconds = (int)Resultat.TotalSeconds;
        int point_Energie = totalSeconds / (int)Current;
        Temps_Restant = totalSeconds % (int)Current;
        print($"Energie Save: {SaveData.Energie}");
        if (SaveData.Energie + point_Energie >= Max_Energie)
        {
            SaveData.Energie = Max_Energie;
        }
        else
        {
            SaveData.Energie += point_Energie;
        }
        T_Horloge.text = $"Energie: {Mathf.Abs(SaveData.Energie)}";
    }


    private void OnApplicationQuit()
    {
        Saving();
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
