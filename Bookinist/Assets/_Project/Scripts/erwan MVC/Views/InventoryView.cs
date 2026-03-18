using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private CanvasGroup canvasGroup;

    public Action<CardModel> OnCardClicked;

    // Liste interne de toutes les cartes affichées dans l'inventaire
    private readonly List<CardView> _cardViews = new List<CardView>();

    public void RegisterCard(CardView cardView)
    {
        // On garde une référence pour pouvoir gérer les highlights
        if (!_cardViews.Contains(cardView))
        {
            _cardViews.Add(cardView);
        }

        cardView.OnCardClicked += (view) =>
        {
            // Propager l'évènement au contrôleur via le modèle
            OnCardClicked?.Invoke(view.CardModel);
        };
    }

    // Active le highlight sur la carte donnée et le désactive sur les autres
    public void SelectCardView(CardModel cardModel)
    {
        foreach (var cardView in _cardViews)
        {
            if (cardView == null) continue;
            cardView.SetSelected(cardView.CardModel == cardModel);
        }
    }


    // Désélectionne toutes les cartes (utile si nécessaire plus tard)
    public void ClearSelection()
    {
        foreach (var cardView in _cardViews)
        {
            if (cardView == null) continue;
            cardView.SetSelected(false);
        }
    }

    // Retire visuellement une carte de la main en fonction de son modèle
    public void RemoveCard(CardModel cardModel)
    {
        CardView viewToRemove = null;

        foreach (var cardView in _cardViews)
        {
            if (cardView != null && cardView.CardModel == cardModel)
            {
                viewToRemove = cardView;
                break;
            }
        }

        if (viewToRemove != null)
        {
            _cardViews.Remove(viewToRemove);
            Destroy(viewToRemove.gameObject);
        }
    }

    public void InitHand(List<SlotModel> slotModels)
    {
        foreach (SlotModel slot in slotModels)
        {
            if (slot.CardModel == null)
                continue;

            if (slot.SlotView == null)
            {
                Debug.LogError($"SlotView manquant pour slot {slot.Index}");
                continue;
            }

            GameObject cardGO = Instantiate(cardPrefab, slot.SlotView.transform, false);
            RectTransform rt = cardGO.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;

            CardView cardView = cardGO.GetComponent<CardView>();
            cardView.Init(slot.CardModel);

            RegisterCard(cardView);
        }
    }

    public void InitSlots(List<SlotModel> slotModels)
    {
        foreach (SlotModel slot in slotModels)
        {
            GameObject slotGO = Instantiate(slotPrefab, gameObject.transform);
            SlotView slotView = slotGO.GetComponent<SlotView>();

            slotView.Init(slot);
        }
    }

    public void AddSlot(SlotModel newSlotModel)
    {
        GameObject slotGO = Instantiate(slotPrefab, gameObject.transform);
        SlotView slotView = slotGO.GetComponent<SlotView>();
        slotView.Init(newSlotModel);
    }

    public void AddCard(SlotModel slot)
    {
        GameObject cardGO = Instantiate(cardPrefab, slot.SlotView.transform, false);
        RectTransform rt = cardGO.GetComponent<RectTransform>();
        rt.anchoredPosition = Vector2.zero;

        CardView cardView = cardGO.GetComponent<CardView>();
        cardView.Init(slot.CardModel);

        RegisterCard(cardView);
    }

    public void SetInteractable(bool isInteractable)
    {
        canvasGroup.interactable = isInteractable;
        canvasGroup.blocksRaycasts = isInteractable;
        canvasGroup.alpha = isInteractable ? 1f : 0.5f;
    }
}