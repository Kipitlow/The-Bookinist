using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    [Header("Identification")]
    public int rewardIndex;
    public bool isPremium;

    [Header("State")]
    public bool isTaken = false;

    [Header("UI References")]
    [SerializeField] private Image _background;
    [SerializeField] private Image _isTakenImage;
    public Button button;

    private void Start()
    {
        if (button != null)
        {
            button.onClick.AddListener(HandleClick);
        }
    }

    private void HandleClick()
    {
        if (ProgressionBar.instance != null)
        {
            ProgressionBar.instance.ClaimReward(rewardIndex, isPremium);
        }
    }

    public void UpdateUI(bool canUnlock, bool isPremiumLocked)
    {
        if (isTaken)
        {
            button.interactable = false;
            SetCheckmarkAlpha(1f);
        }
        else
        {
            button.interactable = canUnlock && !isPremiumLocked;
            SetCheckmarkAlpha(0f);
        }
    }

    private void SetCheckmarkAlpha(float alpha)
    {
        if (_isTakenImage != null)
        {
            Color c = _isTakenImage.color;
            c.a = alpha;
            _isTakenImage.color = c;
        }
    }
}