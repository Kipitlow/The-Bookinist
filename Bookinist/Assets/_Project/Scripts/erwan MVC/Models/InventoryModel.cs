using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel
{
    private readonly List<SlotModel> _slotList = new List<SlotModel>();

    // Source de données de cartes (ScriptableObjects)
    private readonly List<CardData> _availableCards;

    // Indique si cet inventaire appartient au joueur 1 (utile pour marquer les cartes piochées)
    private readonly bool _isPlayer1Inventory;

    public Action<SlotModel> OnCreateNewSlot;

    public InventoryModel(int numberSlot, int numberCard, bool isPlayer1Inventory, List<CardData> availableCards)
    {
        _availableCards = availableCards;
        _isPlayer1Inventory = isPlayer1Inventory;
        AddSlot(numberSlot, numberCard, isPlayer1Inventory);
    }

    public void AddSlot(int numberSlot, int numberCard, bool isPlayer1Card)
    {
        for (int i = 0; i < numberSlot; i++)
        {
            bool isEmpty = i >= numberCard;
            _slotList.Add(new SlotModel(i, isEmpty, isPlayer1Card, _availableCards));
        }
    }

    public void AddOneSlot(int newNumberSlot, bool isPlayer1Card)
    {
        _slotList.Add(new SlotModel(newNumberSlot, true, isPlayer1Card, _availableCards));
        OnCreateNewSlot?.Invoke(_slotList[^1]);
    }

    public void RemoveCard(CardModel card)
    {
        foreach (SlotModel slot in _slotList)
        {
            if (slot.CardModel == card)
            {
                // On marque simplement le slot comme vide, sans recréer de carte
                slot.SetIsEmpty(true, true, _availableCards);
                return;
            }
        }
    }

    // Ajoute une carte précise dans le premier slot vide
    public void AddCard(CardModel card)
    {
        SlotModel emptySlot = null;
        foreach (var slot in _slotList)
        {
            if (slot.IsEmpty)
            {
                emptySlot = slot;
                break;
            }
        }

        if (emptySlot == null)
        {
            Debug.LogWarning("InventoryModel.AddCard : aucun slot vide disponible, la carte ne peut pas être ajoutée.");
            return;
        }
        
        emptySlot.SetCardModel(card);

        bool isPlayer1Card = card != null && card.IsPlayer1Card;
        emptySlot.SetIsEmpty(false, isPlayer1Card, _availableCards);
    }

    // Pioche une carte aléatoire parmi AvailableCards et l'ajoute dans un slot vide
    public void DrawCard()
    {
        if (_availableCards == null || _availableCards.Count == 0)
        {
            Debug.LogWarning("InventoryModel.DrawCard : aucune CardData disponible pour piocher.");
            return;
        }

        // Cherche un slot vide
        SlotModel emptySlot = null;
        foreach (var slot in _slotList)
        {
            if (slot.IsEmpty)
            {
                emptySlot = slot;
                break;
            }
        }

        if (emptySlot == null)
        {
            Debug.LogWarning("InventoryModel.DrawCard : aucun slot vide disponible, la carte ne peut pas être piochée.");
            return;
        }

        // Choisit une CardData aléatoire
        int index = UnityEngine.Random.Range(0, _availableCards.Count);
        CardData data = _availableCards[index];

        // Crée le modèle de carte pour le bon joueur
        CardModel newCard = new CardModel(data, _isPlayer1Inventory);

        // Ajoute la carte via la méthode existante
        AddCard(newCard);
    }

    #region Helpers
    public List<SlotModel> SlotList => _slotList;
    public List<CardData> AvailableCards => _availableCards;
    #endregion
}
