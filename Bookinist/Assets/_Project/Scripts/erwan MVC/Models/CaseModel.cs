public class CaseModel
{
    public enum ECaseState
    {
        EMPTY,
        CARDFILL,
        EFFECTFILL,
        OBSTACLEFILL
    }

    private CardModel _cardModel;

    public CaseModel(ECaseState caseState = ECaseState.EMPTY)
    {
        CaseState = caseState;
    }

    public ECaseState CaseState { get; private set; }

    public CardModel CardModel => _cardModel;

    public bool PlaceCard(CardModel card)
    {
        if (CaseState != ECaseState.EMPTY)
            return false;

        _cardModel = card;
        CaseState = ECaseState.CARDFILL;
        return true;
    }

    public void RemoveCard()
    {
        if (CaseState == ECaseState.CARDFILL)
        {
            _cardModel = null;
            CaseState = ECaseState.EMPTY;
        }
    }

    public void PlaceObstacle()
    {
        _cardModel = null;
        CaseState = ECaseState.OBSTACLEFILL;
    }

    public void PlaceEffect()
    {
        _cardModel = null;
        CaseState = ECaseState.EFFECTFILL;
    }

    public bool IsEmpty()
    {
        return CaseState == ECaseState.EMPTY;
    }
}
