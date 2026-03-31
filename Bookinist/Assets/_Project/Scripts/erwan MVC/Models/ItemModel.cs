using System;
using System.Collections.Generic;

public class ItemModel
{
    public enum EPowerSide
    {
        Left,
        Top,
        Right,
        Bottom
    };

    private string _name;
    private Dictionary<EPowerSide, int> _powerside = new Dictionary<EPowerSide, int>();

    private bool _isTooStrong;
    private bool _isPlayer1Card;

    private int _cumulativPowerLimit = 18;
    private int _cumulativPower;

    // Nouveau : référence à la CardData d’origine
    public ItemData Data { get; private set; }

    public bool IsPlayer1Card => _isPlayer1Card;

    public Action OnPlayerChange;

    // Nouveau constructeur à partir d'une CardData
    public ItemModel(ItemData data, bool isPlayer1Card)
    {
        Data = data;
        _name = data != null ? data.displayName : "Unknown";
        _isPlayer1Card = isPlayer1Card;

        //_powerside.Add(EPowerSide.Top, data != null ? data.powerTop : 1);

        UpdateCumulativPower();
    }

    public void SetPower(int power, EPowerSide side)
    {
        _powerside[side] = power;
    }

    public void UpdateCumulativPower()
    {
        _cumulativPower = 0;

        foreach (var kvp in _powerside)
        {
            _cumulativPower += kvp.Value;
        }
    }

    public void SetIsPlayerOneCard(bool isPlayerOneCard)
    {
        if (_isPlayer1Card == isPlayerOneCard)
            return;

        _isPlayer1Card = isPlayerOneCard;
        //récup x et y de la card dans le board pour utilise CaseView[x,y].ChangeColor jspasquoi 
        OnPlayerChange?.Invoke();
    }

    public void SetTooStrong(bool isTooStrong)
    {
        _isTooStrong = isTooStrong;
    }

    public int GetPowerBySide(EPowerSide side)
    {
        return _powerside[side];
    }

    public int CumulativPower => _cumulativPower;
    public string Name => _name;
}
