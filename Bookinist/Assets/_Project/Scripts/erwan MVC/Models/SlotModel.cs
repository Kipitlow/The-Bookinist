using UnityEngine;
using System.Collections.Generic;

public class SlotModel
{
    private int _index;
    private bool _isEmpty = true;
    public CardModel CardModel { get; set; }
    public SlotView SlotView { get; set; }

    // Référence vers les cartes disponibles (ScriptableObjects)
    private List<CardData> _availableCards;

    public SlotModel(int index, bool isEmpty, bool isPlayer1Card, List<CardData> availableCards)
    {
        _index = index;
        _availableCards = availableCards;
        SetIsEmpty(isEmpty, isPlayer1Card, availableCards);
    }

    public void SetIsEmpty(bool isEmpty, bool isPlayer1Card, List<CardData> availableCards)
    {
        _isEmpty = isEmpty;

        if (isEmpty)
        {
            CardModel = null;
            return;
        }

        if (availableCards != null && availableCards.Count > 0)
        {
            int randomIndex = Random.Range(0, availableCards.Count);
            CardData data = availableCards[randomIndex];

            // Utiliser le constructeur basé sur CardData
            CardModel = new CardModel(data, isPlayer1Card);
        }
    }

    public void SetCardModel(CardModel cardModel)
    {
        CardModel = cardModel;
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    #region Helpers
    public bool IsEmpty => _isEmpty;
    public int Index => _index;
    #endregion
}
