using System;
using System.Collections.Generic;

public class GameModel
{
    private BoardModel _boardModel;
    private CardModel _selectedCard;
    private InventoryModel _inventoryModelPlayer1;
    private InventoryModel _inventoryModelPlayer2;
    private float _remainTime;
    private float _maxTime;

    private int _playerOnePower;
    private int _playerTwoPower;
    private int _powerToWinGame;
    private bool _isPlayerOneTurn;


    public int HorizontalCaseNumber { get; private set; }
    public int VerticalCaseNumber { get; private set;}
    public int StartSlotNumber { get; private set;}
    public int StartCardNumber { get; private set;}

    public Action<bool> OnWinGame;
    public Action<bool, int> OnAddPower;
    public Action<bool> OnOpponentCardDrawn;
    public Action OnCardDrawn;

    public GameModel(
        int horizontalCaseNumber,
        int verticalCaseNumber,
        int startSlotNumber,
        int startCardNumber,
        List<CardData> availableCardsPlayer1,
        List<CardData> availableCardsPlayer2,
        float maxTime,
        int powerToWinGame)
    {
        _boardModel = new BoardModel(horizontalCaseNumber, verticalCaseNumber);
        _inventoryModelPlayer1 = new InventoryModel(startSlotNumber, startCardNumber, true, availableCardsPlayer1);
        _inventoryModelPlayer2 = new InventoryModel(startSlotNumber, startCardNumber, false, availableCardsPlayer2);

        HorizontalCaseNumber = horizontalCaseNumber;
        VerticalCaseNumber = verticalCaseNumber;
        StartSlotNumber = startSlotNumber;
        StartCardNumber = startCardNumber;

        _maxTime = maxTime;
        _remainTime = maxTime;
        _powerToWinGame = powerToWinGame;
        _playerOnePower = 0;
        _playerTwoPower = 0;

        _boardModel.OnStealOpponentCard += HandleStealOpponentCard;
        _boardModel.OnPoseWeakCard += HandlePoseWeakCard;
        _boardModel.OnPoseNeutralCard += HandlePoseNeutralCard;
    }

    private void HandlePoseNeutralCard()
    {
        // Le joueur qui pose une carte neutre pioche une carte
        if (IsPlayerOneTurn)
            InventoryModelPlayer1.AddOneSlot(InventoryModelPlayer1.SlotList.Count + 1, IsPlayerOneTurn);
        else
            InventoryModelPlayer2.AddOneSlot(InventoryModelPlayer2.SlotList.Count + 1, IsPlayerOneTurn);

        DrawCard();
        OnCardDrawn?.Invoke();
    }

    private void HandlePoseWeakCard()
    {
        if (IsPlayerOneTurn)
            InventoryModelPlayer2.AddOneSlot(InventoryModelPlayer2.SlotList.Count + 1, !IsPlayerOneTurn);
        else
            InventoryModelPlayer1.AddOneSlot(InventoryModelPlayer1.SlotList.Count + 1, !IsPlayerOneTurn);

        OpponentDrawCard();
    }

    private void HandleStealOpponentCard(bool isPlayerOne, int power)
    {
        AddPower(isPlayerOne, power);
    }

    public void AddPower(bool isPlayerOne, int power)
    {
        if (isPlayerOne)
        {
            _playerOnePower += power;
            OnAddPower?.Invoke(true, _playerOnePower);

            if (_playerOnePower >= _powerToWinGame)
                OnWinGame?.Invoke(true);
        }
        else
        {
            _playerTwoPower += power;
            OnAddPower?.Invoke(false, _playerTwoPower);

            if (_playerTwoPower >= _powerToWinGame)
                OnWinGame?.Invoke(false);
        }
    }

    public void ChangePlayerTurn()
    {
        _isPlayerOneTurn = !_isPlayerOneTurn;
    }

    public void SetIsPlayerOneTurn(bool isPlayerOneTurn)
    {
        _isPlayerOneTurn = isPlayerOneTurn;
    }

    public void DrawCard()
    {
        // Le joueur dont c'est le tour pioche une carte dans son inventaire
        if (_isPlayerOneTurn)
        {
            _inventoryModelPlayer1.DrawCard();
        }
        else
        {
            _inventoryModelPlayer2.DrawCard();
        }
    }

    public void OpponentDrawCard()
    {
        // Le joueur dont c'est le tour pioche une carte dans son inventaire
        if (_isPlayerOneTurn)
        {
            _inventoryModelPlayer2.DrawCard();
        }
        else
        {
            _inventoryModelPlayer1.DrawCard();
        }

        OnOpponentCardDrawn?.Invoke(!_isPlayerOneTurn);
    }

    #region Helpers
    public BoardModel BoardModel => _boardModel;
    public InventoryModel InventoryModelPlayer1 => _inventoryModelPlayer1;
    public InventoryModel InventoryModelPlayer2 => _inventoryModelPlayer2;
    public float RemainTime => _remainTime;
    public float MaxTime => _maxTime;
    public bool IsPlayerOneTurn => _isPlayerOneTurn;
    public int PlayerOnePower => _playerOnePower;
    public int PlayerTwoPower => _playerTwoPower;

    public CardModel SelectedCard { get; set; }
    #endregion
}
