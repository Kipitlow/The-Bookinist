using System;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public T Read<T>(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"[SaveManager] Fichier non trouvé : {filePath}");
            return default(T);
        }
        try
        {
            string json = File.ReadAllText(filePath);
            T value = JsonUtility.FromJson<T>(json);
            if (value == null)
                Debug.LogWarning($"[SaveManager] Fichier vide ou invalide : {filePath}");
            return value;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SaveManager] Erreur lors du chargement : {ex.Message}");
            return default(T);
        }
    }

    public void Write<T>(string fileName, T value)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            string json = JsonUtility.ToJson(value, true);
            File.WriteAllText(filePath, json);
            Debug.Log($"[SaveManager] Sauvegarde réussie : {filePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SaveManager] Erreur lors de la sauvegarde : {ex.Message}");
        }
    }

    public void Delete(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"[SaveManager] Fichier supprimé : {filePath}");
        }
        else
        {
            Debug.LogWarning($"[SaveManager] Fichier à supprimer introuvable : {filePath}");
        }
    }
}
