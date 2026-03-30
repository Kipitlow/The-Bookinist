using UnityEngine;

public class SC_Iteam : MonoBehaviour
{
    [Header("Iteam")]
    public string Name_Iteam;
    public Sprite Image_Iteam;
    public GameObject Object_Iteam;

    [Header("L'inventaire")]
    SC_Inventaire Inventaire;


    private void Start()
    {
        Inventaire = GameObject.Find("InventoryPanel").GetComponent<SC_Inventaire>();
    }
    public void DropIteam()
    {
        Inventaire.Prendre_Object(gameObject);
        Destroy(gameObject);
    }
}
