using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SC_Inventaire : MonoBehaviour
{
    private List<Button> Button_inventaire = new List<Button>();
    public List<SC_Iteam> Inventaire = new List<SC_Iteam>();

    private void Start()
    {
        //Setup les B_Inventaire dans le canva, de façon simple, lors du transfére des données sur GitHup s'efface.
        Button B1_Inventaire = GameObject.Find("B_inventaire_01").GetComponent<Button>();
        Button_inventaire.Add(B1_Inventaire);
        Button B2_Inventaire = GameObject.Find("B_inventaire_02").GetComponent<Button>();
        Button_inventaire.Add(B2_Inventaire);
        Button B3_Inventaire = GameObject.Find("B_inventaire_03").GetComponent<Button>();
        Button_inventaire.Add(B3_Inventaire);
        Button B4_Inventaire = GameObject.Find("B_inventaire_04").GetComponent<Button>();
        Button_inventaire.Add(B4_Inventaire);
        Button B5_Inventaire = GameObject.Find("B_inventaire_05").GetComponent<Button>();
        Button_inventaire.Add(B5_Inventaire);
    }
    public void Prendre_Object(GameObject Objet)
    {
        //On éffectuant un ajoute d'iteam dans l'inventaire je me permet de rajouter une limitation on nombre de 5 pour le moment
        SC_Iteam RR = Objet.GetComponent<SC_Iteam>(); 
        if (RR != null) Inventaire.Add(RR);
        
        
    }
    public void UpdateInventaire()
    {
        if(Inventaire.Count <= 5)
        {
            for (int RR = 0; RR < Inventaire.Count; RR++)
            {
                //Inventaire[RR].Image_Iteam;
                Image ee =Button_inventaire[RR].GetComponentInChildren<Image>(); // Permet de modifier une Image
            }
        }
        else { Debug.LogError("La Liste D'inventaire est supérieur a 5"); }
    }
}
