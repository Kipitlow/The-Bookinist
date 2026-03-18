using System;
using UnityEngine;

public class BoardModel
{
    private CaseModel[,] _caseList;
    public int Width { get; }
    public int Height { get; }

    public Action<bool, int> OnStealOpponentCard;
    public Action OnPoseWeakCard;
    public Action OnPoseNeutralCard;
    public Action<int, int, bool> OnCardCaptured;
    public Action OnCardCapturedSound;

    public BoardModel(int colNumber, int lineNumber)
    {
        Width = colNumber;
        Height = lineNumber;
        CreateBoard(colNumber, lineNumber);
    }

    private void CreateBoard(int colNumber, int lineNumber)
    {
        _caseList = new CaseModel[lineNumber, colNumber];
        for (int y = 0; y < lineNumber; y++)
        {
            for (int x = 0; x < colNumber; x++)
            {
                _caseList[y, x] = new CaseModel();
            }
        }
    }

    public void PlaceCard(int x, int y, CaseModel caseModel, CardModel cardModel)
    {
        if (!caseModel.PlaceCard(cardModel))
            return;

        CheckAdjacentCases(x, y);
    }

    private void CheckAdjacentCases(int originX, int originY)
    {
        CardModel originCard = _caseList[originY, originX].CardModel;

        // gauche
        CheckCase(originX - 1, originY, originCard,
            CardModel.EPowerSide.Left, CardModel.EPowerSide.Right,
            new Vector2Int(-1, 0));

        // droite
        CheckCase(originX + 1, originY, originCard,
            CardModel.EPowerSide.Right, CardModel.EPowerSide.Left,
            new Vector2Int(1, 0));

        // haut
        CheckCase(originX, originY - 1, originCard,
            CardModel.EPowerSide.Top, CardModel.EPowerSide.Bottom,
            new Vector2Int(0, -1));

        // bas
        CheckCase(originX, originY + 1, originCard,
            CardModel.EPowerSide.Bottom, CardModel.EPowerSide.Top,
            new Vector2Int(0, 1));
    }

    private void CheckCase(int x, int y, CardModel originCard, CardModel.EPowerSide originSide, CardModel.EPowerSide neighbourSide, Vector2Int dir)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return;

        CaseModel neighbourCase = _caseList[y, x];
        if (neighbourCase.CaseState != CaseModel.ECaseState.CARDFILL)
            return;

        CardModel neighbourCard = neighbourCase.CardModel;
        int originPower = originCard.GetPowerBySide(originSide);
        int neighbourPower = neighbourCard.GetPowerBySide(neighbourSide);

        if (originCard.IsPlayer1Card != neighbourCard.IsPlayer1Card)
        {
            if (originPower > neighbourPower)
            {
                neighbourCard.SetIsPlayerOneCard(originCard.IsPlayer1Card);
                OnStealOpponentCard?.Invoke(originCard.IsPlayer1Card, neighbourCard.CumulativPower);

                OnCardCaptured?.Invoke(x, y, originCard.IsPlayer1Card);
                OnCardCapturedSound?.Invoke();

                Debug.Log($"Capture : ({x},{y}) maintenant appartient   Player {(originCard.IsPlayer1Card ? "1" : "2")}");

                CheckCase(x + dir.x, y + dir.y, originCard, originSide, neighbourSide, dir);
            }
            else if (originPower < neighbourPower)
            {
                OnPoseWeakCard?.Invoke();
            }
            else
            {
                OnPoseNeutralCard?.Invoke();
            }
        }
    }

    public CaseModel GetCase(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            throw new ArgumentOutOfRangeException($"Invalid coordinates ({x},{y})");

        return _caseList[y, x];
    }

    public CaseModel[,] CaseList => _caseList;
}
