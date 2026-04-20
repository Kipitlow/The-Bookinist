using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionBar : MonoBehaviour
{
    public static ProgressionBar instance;

    [Header("UI Elements")]
    public ScrollRect battlePassScrollRect;
    public Image BattlePassBar;
    public GameObject BattlePassIcon;
    public List<Image> Paliers = new List<Image>();
    public List<PalierDebloqueur> rewardPalier = new List<PalierDebloqueur>();

    [Header("Settings")]
    public int palier = 0;
    public float xpPass; // avancé du pass de 0 à 1.

    private float confirmedTotalXp = 0; // avancé du pass en xp
    private float waitingXp = 0;

    private Color Lock = new Color32(109, 109, 109, 255);
    private Color Unlock = new Color32(241, 211, 0, 255);
    private float lerpTime = 0.7f;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SaveSystem.instance.OnDataUpdate += LoadProgression;
        if (SaveSystem.instance.battlePass != null) LoadProgression();
    }

    private void LoadProgression()
    {
        BattlePassData data = SaveSystem.instance.battlePass;
        if (data == null) return;

        // On restaure l'état visuel exact de la dernière fois
        confirmedTotalXp = data.confirmedXp;
        waitingXp = data.waitingXp;

        xpPass = confirmedTotalXp / 15000f;
        BattlePassBar.fillAmount = xpPass;

        // On débloque instantanément les paliers déjà validés
        SyncPalierInstant();

        if (waitingXp > 0) BattlePassIcon.SetActive(true);
    }

    public BattlePassData GetDataForSave()
    {
        return new BattlePassData
        {
            confirmedXp = confirmedTotalXp,
            waitingXp = waitingXp
        };
    }

    public void xpGain(float xp) // Appelé par les quêtes
    {
        waitingXp += xp;

        float virtualXpPass = (confirmedTotalXp + waitingXp) / 15000f;
        if (CheckIfNewPalier(virtualXpPass, palier))
        {
            BattlePassIcon.SetActive(true);
        }

        SaveSystem.instance.Save();
    }

    public void addXpPass() // Appelé quand on ouvre le menu du pass
    {
        if (waitingXp <= 0) return;

        BattlePassIcon.SetActive(false);
        StartCoroutine(AnimateBar(waitingXp));

        confirmedTotalXp += waitingXp;
        waitingXp = 0;


        SaveSystem.instance.Save();
    }

    private IEnumerator AnimateBar(float amount)
    {
        float currentTime = 0;
        float startXp = xpPass;
        float targetXp = Mathf.Clamp((confirmedTotalXp + amount) / 15000f, 0, 1);

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;
            xpPass = Mathf.Lerp(startXp, targetXp, currentTime / lerpTime);
            BattlePassBar.fillAmount = xpPass;

            if (CheckIfNewPalier(xpPass, palier))
            {
                palier++;
                unlockVerif();
            }
            yield return null;
        }
        xpPass = targetXp;
        BattlePassBar.fillAmount = xpPass;
    }

    private void SyncPalierInstant()
    {
        palier = 0;
        while (CheckIfNewPalier(xpPass, palier))
        {
            palier++;
        }
        unlockVerif();
    }

    private bool CheckIfNewPalier(float currentXpFill, int currentPalier)
    {
        float threshold = (currentPalier == 0) ? 0.015f : (currentPalier * 0.0333f + 0.015f);
        return currentXpFill >= threshold && currentPalier < 30;
    }

    public void unlockVerif()
    {
        for (int i = 0; i < palier; i++)
        {
            if (i < Paliers.Count) Paliers[i].color = Unlock &&;

            if (i < rewardPalier.Count && rewardPalier[i] != null)
            {
                foreach (Button btn in rewardPalier[i].rewards)
                {
                    if (btn != null && !btn.interactable) btn.interactable = true;
                }
            }
        }
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




check bool Premium == true pour activer ou non les palier
quand on appuie sur le bouton pour debloquer le premium ca passe sur tous les palier debloqué et ca rend les boutons avec premium  interactable
*/


