using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionBar : MonoBehaviour
{
    public int palier = 0;
    public float xpPass;
    public Image BattlePassBar;
    public List<Image> Paliers = new List<Image>();
    private Color Lock = new Color32(109, 109, 109, 255);
    private Color Unlock = new Color32(241, 211, 0, 255);


    void Start()
    {
        BattlePassBar.fillAmount = 0;
        xpPass = 0;
        ResetLock();
    }

    private void CheckPalierProgress()
    {
        if (palier == 0)
        {
            if (xpPass >= 0.015)
                palier += 1;
        }
        else
        {
            if (xpPass >= (palier * 0.0333 + 0.015))
                palier += 1;
        }
    }
   

    private void ResetLock()
    {
        foreach (Image img in Paliers)
        {
            img.color = Lock;
        }
    }
    public void unlockVerif()
    {
        for (int i = 0; i <= palier-1; i++)
        {
            if (Paliers[i].color == Lock)
            {
                Paliers[i].color = Unlock;
            }
        }
    }
    public void addXpPass(float xp)
    {
        xpPass += (1f / 15000f) * xp;
        BattlePassBar.fillAmount = xpPass;
        if (xpPass >= 15400)
            xpPass = 15400;
        CheckPalierProgress();
        unlockVerif();
    }


    /*missions quotidiennes : 100points 
      missions hebdomadaires : 500points
      missions d'évènement : 2000points
     
      3missions quotidiennes : se connecter
                               utiliser de l'énérgie
                               dépensé des francs ou de l'or
      3missions hebdomadaires : finir un livre sans utiliser d'indice
                                finir 3livres
                                acheter une decoration d'appartement
      3missions de saisons : se connecter 14jours d'affiler
                             gagner un total de 10.000francs
                             Trouver un total de 30oeufs / 30 cannes à sucres / 30 bonbons / 30 boite de chocolat / trefles à 4feuilles(5 dispersés chaque 
                                jour dans le decors de la bouquinerie sur lesquels il suffit d'appuyer pour qu'ils dispawnent et se comptabilisent, 
                                respectivement pour paques / noel / halloween / saint valentin et saint patrick)
     Mission de saison 0 pour la beta: participer à la beta (+2k points à la fin de l'experience)

    30paliers avec 500 entre chaque => 15k points à récupérer

    15000   5000
    1       0.333
     
    */
}
