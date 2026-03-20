using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[Serializable]
public class Dialogue_Marchant
{
    [SerializeField] public enum CatagorieDialogue {Random_Talk_GoodMorning, Random_Talk_GoodBye}
    [SerializeField] public string[] T_Dialogue;
    

}
/*[Serializable]
public class GestionUI
{
    
}*/
public class SC_Marchant : MonoBehaviour
{
    [Header("Dialogue_Marchant")]
    [SerializeField] public List<Dialogue_Marchant> Marchant_Talk_Player;

    [Header("Gestion UI")]
    [SerializeField] public GameObject UI_Gameplay;
    [SerializeField] public GameObject UI_Marchant;
    [Header("Autre")]
    [SerializeField] public GameObject[] Button_Hidden;
    private Animator an;
    void Start()
    {
        an = GetComponentInChildren<Animator>();
        foreach (GameObject aa in Button_Hidden)
        {
            if (aa.name == "B_balance") aa.SetActive(true);
            else if (aa.name == "Panel_Talk") aa.SetActive(true);
            else aa.SetActive(false);
        }
        change_UI();
    }

    /*public IEnumerator Switch_Variable()
    {
        if(Savant== -2)
        {
            Savant = 2;
        }
        else { Savant -= 1; }
            an.SetInteger("Enumeration", Savant);

        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Switch_Variable());
    }*/

    public void Cache_Balance(GameObject Self)
    {
        switch (Self.name)
        {
            case "B_balance":
                Affichage(Self);
                break;
            case "B_Reset":
                foreach (GameObject aa in Button_Hidden)
                {
                    if (aa.name == "B_balance") aa.SetActive(true);
                    else if (aa.name == "Panel_Talk") aa.SetActive(true);
                    else aa.SetActive(false);
                }
                break;
            case "B_Object_1":
                Affichage(Self);
                change_An_Balance(-1);
                break;
            case "B_Object_2":
                Affichage(Self);
                change_An_Balance(0);

                foreach (GameObject aa in Button_Hidden)
                {
                    if (aa.name == "Panel_Blance") aa.SetActive(true);
                    else if (aa.name == "Panel_Talk") aa.SetActive(true);
                    else aa.SetActive(false);
                }

                Invoke("change_UI",1);
                break;
            case "B_Object_3":
                Affichage(Self);
                change_An_Balance(1);
                break;
            case "B_Object_4":
                Affichage(Self);
                change_An_Balance(2);
                break;
        }
    }

    private void Affichage(GameObject ee)
    {
        foreach(GameObject aa in Button_Hidden)
        {
            if(aa.name !="B_balance") aa.SetActive(true);
        }
        ee.SetActive(false);
    }

    private void change_An_Balance(int rr)
    {
        an.SetInteger("Enumeration", rr);
    }

    public void change_UI()
    {
        UI_Gameplay.SetActive(UI_Marchant.activeSelf);
        UI_Marchant.SetActive(!UI_Marchant.activeSelf);
    }
}
