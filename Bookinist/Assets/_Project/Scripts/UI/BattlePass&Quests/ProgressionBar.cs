using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionBar : MonoBehaviour
{

    //battlepass
    public int palier = 0;
    public float xpPass;
    public Image BattlePassBar;
    public GameObject BattlePassIcon;
    public List<Image> Paliers = new List<Image>();

    private Color Lock = new Color32(109, 109, 109, 255);
    private Color Unlock = new Color32(241, 211, 0, 255);

    float lerpTime = 0.7f;
    float waitingXp = 0;

    public List<PalierDebloqueur> rewardPalier = new List<PalierDebloqueur>();

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
        for (int i = 0; i <= palier - 1; i++)
        {
            if (Paliers[i].color == Lock)
            {
                Paliers[i].color = Unlock;
                for(int j = 0; rewardPalier[i].rewards.Count > 0; j++)
                {
                    rewardPalier[i].rewards[j].interactable = true;
                }
            }
        }
    }
    public void addXpPass() // fonction appeler par le bouton d'ouverture de pass, pour actualiser la barre que lorsque on est dessus
    {
        BattlePassIcon.SetActive(false);
        StartCoroutine(AnimateBar(waitingXp));
        waitingXp = 0;
    }
    public void xpGain(float xp) // fonction à appeler pour les récompenses de quêtes
    {
        waitingXp += xp;
        if(palier == 0)
        {
            if ((1f / 15000f * waitingXp) + xpPass >= (palier * 0.0333 + 0.015)) // check si on a passer le palier 1
            {
                BattlePassIcon.SetActive(true);
            }
        }
        else
        {
            if ((1f / 15000f * waitingXp) + xpPass >= (palier * 0.0333 + 0.015) && palier < 30) // check si on a passer un palier
            {
                BattlePassIcon.SetActive(true);
            }
        }
        
    }

    private IEnumerator AnimateBar(float xp)
    {
        float currentTime = 0;
        float startXp = xpPass;
        float targetXp = Mathf.Clamp(xpPass + (1f / 15000f) * xp, 0, 1);

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;

            xpPass = Mathf.Lerp(startXp, targetXp, currentTime / lerpTime);

            BattlePassBar.fillAmount = xpPass;

            CheckPalierProgress();
            unlockVerif();

            yield return null; 
        }

        xpPass = targetXp;
        BattlePassBar.fillAmount = xpPass;
    }




}



/*missions quotidiennes : 100points 
  missions hebdomadaires : 500points
  missions d'évènement : 2000points

  3missions quotidiennes : se connecter
                           utiliser de l'énérgie
                           dépensé des francs ou de l'or
  3missions hebdomadaires : finir un livre sans utiliser d'indice
                            finir 3livres
                            acheter une decoration
  3missions de saisons : se connecter 14jours d'affiler
                         gagner un total de 10.000francs
                         Trouver un total de 30oeufs / 30 cannes à sucres / 30 bonbons / 30 boite de chocolat / trefles à 4feuilles(5 dispersés chaque 
                            jour dans le decors de la bouquinerie sur lesquels il suffit d'appuyer pour qu'ils dispawnent et se comptabilisent, 
                            respectivement pour paques / noel / halloween / saint valentin et saint patrick)
 Mission de saison 0 pour la beta: participer à la beta (+2k points à la fin de l'experience)

30paliers avec 500 entre chaque => 15k points à récupérer

15000   5000
1       0.333
765 325
*/


