using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionBar : MonoBehaviour
{
    public static ProgressionBar instance;

    [Header("UI Elements")]
    public Image BattlePassBar;
    public GameObject BattlePassIcon;
    public List<Image> PaliersVisuals = new List<Image>();

    [Header("Rewards Lists")]
    public List<Reward> freeRewards = new List<Reward>();
    public List<Reward> premiumRewards = new List<Reward>();

    [Header("Settings")]
    public bool isPremiumActive = false;
    public int palier = 0;
    public float xpPass;
    private float confirmedTotalXp = 0;
    private float waitingXp = 0;
    private float lerpTime = 0.7f;

    private Color colorLock = new Color32(109, 109, 109, 255);
    private Color colorUnlock = new Color32(241, 211, 0, 255);

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SaveSystem.instance.OnDataUpdate += LoadProgression;
        if (SaveSystem.instance.bp!= null) LoadProgression();
    }

    public void xpGain(float xp)
    {
        waitingXp += xp;
        float virtualXpPass = (confirmedTotalXp + waitingXp) / 15000f;

        if (CheckIfNewPalier(virtualXpPass, palier))
        {
            BattlePassIcon.SetActive(true);
        }
        SaveSystem.instance.Save();
    }

    public void addXpPass()
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
                RefreshRewardsUI();
            }
            yield return null;
        }
        xpPass = targetXp;
        BattlePassBar.fillAmount = xpPass;
    }

    private void LoadProgression()
    {
        PlayerBP data = SaveSystem.instance.bp;
        if (data == null) return;

        confirmedTotalXp = data.confirmedXp;
        waitingXp = data.waitingXp;
        isPremiumActive = data.isPremiumActive;
        xpPass = confirmedTotalXp / 15000f;

        for (int i = 0; i < freeRewards.Count; i++)
        {
            if (i < data.freeRewardsTaken.Count) freeRewards[i].isTaken = data.freeRewardsTaken[i];
        }

        for (int i = 0; i < premiumRewards.Count; i++)
        {
            if (i < data.premiumRewardsTaken.Count) premiumRewards[i].isTaken = data.premiumRewardsTaken[i];
        }

        palier = 0;
        while (CheckIfNewPalier(xpPass, palier))
        {
            palier++;
        }

        RefreshRewardsUI();
    }

    public void RefreshRewardsUI()
    {
        BattlePassBar.fillAmount = xpPass;

        for (int i = 0; i < 30; i++)
        {
            bool isReached = (i < palier);

            if (i < PaliersVisuals.Count)
                PaliersVisuals[i].color = isReached ? colorUnlock : colorLock;

            if (i < freeRewards.Count)
                freeRewards[i].UpdateUI(isReached, false);

            if (i < premiumRewards.Count)
                premiumRewards[i].UpdateUI(isReached, !isPremiumActive);
        }
    }

    public void ClaimReward(int index, bool isPremium)
    {
        if (isPremium)
            premiumRewards[index].isTaken = true;
        else
            freeRewards[index].isTaken = true;

        RefreshRewardsUI();
        SaveSystem.instance.Save();
    }

    private bool CheckIfNewPalier(float currentXpFill, int palierIndex)
    {
        float threshold = (palierIndex == 0) ? 0.015f : (palierIndex * 0.0333f + 0.015f);
        return currentXpFill >= threshold && palierIndex < 30;
    }

    public void ActivatePremium()
    {
        isPremiumActive = true;
        RefreshRewardsUI();
        SaveSystem.instance.Save();
    }

    public PlayerBP GetDataForSave()
    {
        PlayerBP data = new PlayerBP();
        data.confirmedXp = confirmedTotalXp;
        data.waitingXp = waitingXp;
        data.isPremiumActive = isPremiumActive;

        data.freeRewardsTaken = new List<bool>();
        foreach (var r in freeRewards) data.freeRewardsTaken.Add(r.isTaken);

        data.premiumRewardsTaken = new List<bool>();
        foreach (var r in premiumRewards) data.premiumRewardsTaken.Add(r.isTaken);

        return data;
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


