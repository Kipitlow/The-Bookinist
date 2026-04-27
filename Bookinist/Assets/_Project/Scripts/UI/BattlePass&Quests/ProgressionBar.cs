using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionBar : MonoBehaviour
{
    public static ProgressionBar instance;

    [Header("UI Elements")]
    [SerializeField] private Image _battlePassBar;
    [SerializeField] private GameObject _battlePassIcon;
    [SerializeField] private List<Image> _paliersVisuals = new List<Image>();

    [Header("Rewards Lists")]
    public List<Reward> freeRewards = new List<Reward>();
    public List<Reward> premiumRewards = new List<Reward>();

    [Header("Settings")]
    public bool isPremiumActive = false;
    public int palier = 0;
    public float xpPass;

    [SerializeField] private float _confirmedTotalXp = 0;
    [SerializeField] private float _waitingXp = 0;
    [SerializeField] private float _lerpTime = 0.7f;

    [SerializeField] private Color _colorLock = new Color32(109, 109, 109, 255);
    [SerializeField] private Color _colorUnlock = new Color32(241, 211, 0, 255);

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SaveSystem.instance.OnDataUpdate += LoadProgression;
        if (SaveSystem.instance.bp != null) LoadProgression();
    }

    public void XpGain(float xp)
    {
        _waitingXp += xp;
        float virtualXpPass = (_confirmedTotalXp + _waitingXp) / 15000f;

        if (CheckIfNewPalier(virtualXpPass, palier))
        {
            _battlePassIcon.SetActive(true);
        }
        SaveSystem.instance.Save();
    }

    public void AddXpPass()
    {
        if (_waitingXp <= 0) return;

        _battlePassIcon.SetActive(false);
        StartCoroutine(AnimateBar(_waitingXp));

        _confirmedTotalXp += _waitingXp;
        _waitingXp = 0;
        SaveSystem.instance.Save();
    }

    private IEnumerator AnimateBar(float amount)
    {
        float currentTime = 0;
        float startXp = xpPass;
        float targetXp = Mathf.Clamp((_confirmedTotalXp + amount) / 15000f, 0, 1);

        while (currentTime < _lerpTime)
        {
            currentTime += Time.deltaTime;
            xpPass = Mathf.Lerp(startXp, targetXp, currentTime / _lerpTime);
            _battlePassBar.fillAmount = xpPass;

            if (CheckIfNewPalier(xpPass, palier))
            {
                palier++;
                RefreshRewardsUI();
            }
            yield return null;
        }
        xpPass = targetXp;
        _battlePassBar.fillAmount = xpPass;
    }

    private void LoadProgression()
    {
        PlayerBP data = SaveSystem.instance.bp;
        if (data == null) return;

        _confirmedTotalXp = data.playerBPConfirmedXp;
        _waitingXp = data.playerBPWaitingXp;
        isPremiumActive = data.playerBPIsPremiumActive;
        xpPass = _confirmedTotalXp / 15000f;

        for (int i = 0; i < freeRewards.Count; i++)
        {
            if (i < data.playerBPFreeRewardsTaken.Count) freeRewards[i].isTaken = data.playerBPFreeRewardsTaken[i];
        }

        for (int i = 0; i < premiumRewards.Count; i++)
        {
            if (i < data.playerBPPremiumRewardsTaken.Count) premiumRewards[i].isTaken = data.playerBPPremiumRewardsTaken[i];
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
        _battlePassBar.fillAmount = xpPass;

        for (int i = 0; i < 30; i++)
        {
            bool isReached = (i < palier);

            if (i < _paliersVisuals.Count)
                _paliersVisuals[i].color = isReached ? _colorUnlock : _colorLock;

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
        data.playerBPConfirmedXp = _confirmedTotalXp;
        data.playerBPWaitingXp = _waitingXp;
        data.playerBPIsPremiumActive = isPremiumActive;

        data.playerBPFreeRewardsTaken = new List<bool>();
        foreach (var r in freeRewards) data.playerBPFreeRewardsTaken.Add(r.isTaken);

        data.playerBPPremiumRewardsTaken = new List<bool>();
        foreach (var r in premiumRewards) data.playerBPPremiumRewardsTaken.Add(r.isTaken);

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


