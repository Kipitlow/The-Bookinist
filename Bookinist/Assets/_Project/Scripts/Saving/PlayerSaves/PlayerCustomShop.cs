using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCustomShop
{
    // Format de chaque entrée : "viewIndex:FurnitureType:itemID"
    // Exemple : "0:Lamp:item_lamp_01"
    public List<string> placedItems = new List<string>();

    /// <summary>
    /// Encode et ajoute (ou remplace) une entrée pour une view + type donnés.
    /// </summary>
    public void SetPlacedItem(int viewIndex, FurnitureType type, string itemID)
    {
        string prefix = $"{viewIndex}:{type}:";

        // Remplace l'entrée existante pour cette view+type si elle existe
        for (int i = 0; i < placedItems.Count; i++)
        {
            if (placedItems[i].StartsWith(prefix))
            {
                placedItems[i] = $"{prefix}{itemID}";
                return;
            }
        }

        placedItems.Add($"{prefix}{itemID}");
    }

    /// <summary>
    /// Supprime l'entrée pour une view + type donnés.
    /// </summary>
    public void RemovePlacedItem(int viewIndex, FurnitureType type)
    {
        string prefix = $"{viewIndex}:{type}:";
        placedItems.RemoveAll(entry => entry.StartsWith(prefix));
    }

    /// <summary>
    /// Parse toutes les entrées et retourne une structure utilisable par CustomShopManager.
    /// Retourne un tableau indexé par viewIndex, chaque élément étant un dict FurnitureType -> itemID.
    /// </summary>
    public Dictionary<FurnitureType, string>[] ParsePlacedItems(int viewCount)
    {
        Dictionary<FurnitureType, string>[] result = new Dictionary<FurnitureType, string>[viewCount];

        for (int i = 0; i < viewCount; i++)
            result[i] = new Dictionary<FurnitureType, string>();

        foreach (string entry in placedItems)
        {
            string[] parts = entry.Split(':');

            if (parts.Length != 3)
            {
                Debug.LogWarning($"[PlayerCustomShop] Entrée invalide ignorée : '{entry}'");
                continue;
            }

            if (!int.TryParse(parts[0], out int viewIndex) || viewIndex < 0 || viewIndex >= viewCount)
            {
                Debug.LogWarning($"[PlayerCustomShop] viewIndex invalide dans '{entry}'");
                continue;
            }

            if (!System.Enum.TryParse(parts[1], out FurnitureType type))
            {
                Debug.LogWarning($"[PlayerCustomShop] FurnitureType invalide dans '{entry}'");
                continue;
            }

            result[viewIndex][type] = parts[2];
        }

        return result;
    }
}