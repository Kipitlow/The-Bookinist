using UnityEngine;
using UnityEngine.UI;
using TMPro; // À retirer si vous utilisez le texte classique
using System.Collections;

public class RewardButton : MonoBehaviour
{
    public enum ItemType { Plume, Or, Franc }

    [Header("Configuration")]
    public ItemType typeSelectionne;
    public int nombreASpanner = 4;
    public string texteAAfficher = "400";

    [Header("Références UI")]
    public Image iconeBouton;
    public TMP_Text texteBouton; // Remplacez par Text si pas de TextMeshPro
    public Transform positionFinale;
    public GameObject itemPrefab;

    [Header("Sprites")]
    public Sprite spritePlume;
    public Sprite spriteOr;
    public Sprite spriteFranc;

    private Sprite spriteActuel;

    void Start()
    {
        ConfigurerBouton();
    }

    // Permet de mettre à jour visuellement le bouton dans Unity
    void OnValidate() { ConfigurerBouton(); }

    void ConfigurerBouton()
    {
        // Sélection du bon sprite
        switch (typeSelectionne)
        {
            case ItemType.Plume: spriteActuel = spritePlume; break;
            case ItemType.Or: spriteActuel = spriteOr; break;
            case ItemType.Franc: spriteActuel = spriteFranc; break;
        }

        if (iconeBouton) iconeBouton.sprite = spriteActuel;
        if (texteBouton) texteBouton.text = texteAAfficher;
    }

    public void OnClickBouton()
    {
        StartCoroutine(SpawnSequence());
    }

    IEnumerator SpawnSequence()
    {
        float intervalle = 1f / nombreASpanner;

        for (int i = 0; i < nombreASpanner; i++)
        {
            // Création de l'objet
            GameObject go = Instantiate(itemPrefab, transform);

            // Position aléatoire autour du bouton (rayon de 50 unités)
            Vector3 randomOffset = (Vector3)Random.insideUnitCircle * 50f;
            go.transform.position = transform.position + randomOffset;

            // Configuration de l'objet (Sprite et Cible)
            go.GetComponent<MovingItem>().Setup(spriteActuel, positionFinale.position);

            // Attend avant de spawn le suivant pour que le tout dure 1s
            yield return new WaitForSeconds(intervalle);
        }
    }
}