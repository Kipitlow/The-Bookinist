using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;

[System.Serializable]
public class Saving_Json
{
    public int Energie;
    public string Last_Time;
}

public class SC_Gestion_Energie : MonoBehaviour
{
    [Header("Time")]
    private TextMeshProUGUI T_Energie;
    private TextMeshProUGUI T_Horloge;
    public int Max_Energie;
    public int Plume_Game;
    public float Current = 5; //<-- cette variable est utiliser on tant que delay de recharge energie
    float Temps_Restant;
    TimeSpan Resultat;

    [Header("Text_Saving")]
    public TextMeshProUGUI T_EnergieSaving;
    public TextMeshProUGUI T_Plume;

    [Header("Saving Json")]
    private string NamePath;
    private Saving_Json SaveData = new Saving_Json();

    private void Awake()
    {
        NamePath = Application.persistentDataPath+ "/energie.json";
    }


    void Start()
    {
        T_Energie = GameObject.Find("T_Energie").GetComponent<TextMeshProUGUI>();
        T_Horloge = GameObject.Find("T_Horloge").GetComponent<TextMeshProUGUI>();
        Invoke("Loading", 0.5f);

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
        while (Plume_Game < Max_Energie)
        {
            print($"waitA1:{Current}");
            Plume_Game += 1;
            T_Energie.text = $"Energie: {Plume_Game}";
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

            T_Plume.text = $"Plume_Game: {Plume_Game}";
            T_EnergieSaving.text = $"Energie Savegarde: {SaveData.Energie}";


            Resultat = DateTime.Now - last_Timer; //Code consite a prendre 2 type datatime (old/new) et le resultat c'est l'intervale entre les 2 time
            Plume_Game += SaveData.Energie; // on setup cette variable depuis la sauvegarde

            Affiche_Energie();
            StartCoroutine(Time_Energie());
        }
        else
        {
            Debug.LogWarning("Pas SauveGarde");
        }
    }
    public void Affiche_Energie()
    {
        int totalSeconds = (int)Resultat.TotalSeconds; //ce code consister a prendre
        int point_Energie = totalSeconds / (int)Current;
        Temps_Restant = totalSeconds % (int)Current;


        
        if (Plume_Game + point_Energie >= Max_Energie)
        {
            Plume_Game = Max_Energie;
        }
        else
        {
            Plume_Game += point_Energie;
        }
        T_Plume.text = $"Plume_Game: {Plume_Game}";
        T_Horloge.text = $"Energie: {Mathf.Abs(Plume_Game)}";
    }


    private void OnApplicationQuit()
    {
        //Saving();
    }

    public void Saving()
    {
        SaveData.Last_Time = DateTime.Now.ToString();
        SaveData.Energie = Plume_Game;


        string JsonTo = JsonUtility.ToJson(SaveData);
        File.WriteAllText(NamePath, JsonTo);
    }

    public void Deleting()
    {
        if (File.Exists(NamePath)) File.Delete(NamePath);
    }

}
