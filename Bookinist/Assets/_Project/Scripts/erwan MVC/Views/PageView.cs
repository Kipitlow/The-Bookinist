using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class BoardView : MonoBehaviour
{
    [SerializeField] private GameObject casePrefab;
    [SerializeField] private float margin = 1f;

    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private RectTransform rectTranform;

    private int _width;
    private int _height;

    public Action<int, int> OnCaseClicked;

    // Grille de CaseView pour retrouver rapidement une case par (y, x)
    private CaseView[,] _caseViews;

    private void OnRectTransformDimensionsChange()
    {
        if (gridLayout != null && rectTranform != null)
            UpdateCellSize();
    }

    public void InitBoard(int width, int height)
    {
        _width = width;
        _height = height;

        _caseViews = new CaseView[_height, _width]; // [y, x] = [ligne, colonne]

        float cellWidth = (rectTranform.rect.width - (_width + 1) * margin) / _width;
        float cellHeight = (rectTranform.rect.height - (_height + 1) * margin) / _height;

        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
        gridLayout.spacing = new Vector2(margin, margin);
        gridLayout.padding.left = gridLayout.padding.right = gridLayout.padding.top = gridLayout.padding.bottom = (int)margin;

        for (int y = 0; y < height; y++)       // lignes
        {
            for (int x = 0; x < width; x++)    // colonnes
            {
                GameObject go = Instantiate(casePrefab, transform);
                if (go == null)
                {
                    Debug.LogError("casePrefab n'est pas assigné !");
                    return;
                }

                CaseView caseView = go.GetComponent<CaseView>();
                if (caseView == null)
                {
                    Debug.LogError("Le prefab n'a pas de CaseView !");
                    return;
                }

                caseView.X = x;
                caseView.Y = y;

                _caseViews[y, x] = caseView;

                caseView.OnCaseClicked += (ix, iy) => OnCaseClicked?.Invoke(ix, iy);
            }
        }
    }

    public void PlaceCardView(CardModel cardModel, int x, int y, GameObject cardPrefab)
    {
        if (_caseViews == null)
        {
            Debug.LogError("BoardView n'a pas été initialisé !");
            return;
        }

        if (x < 0 || x >= _width || y < 0 || y >= _height)
        {
            Debug.LogError($"Coordonnées invalides : ({x},{y})");
            return;
        }

        CaseView caseView = _caseViews[y, x]; // Accès [y, x]
        if (caseView == null)
        {
            Debug.LogError($"Aucune CaseView enregistrée pour ({x},{y})");
            return;
        }

        if (cardPrefab == null)
        {
            Debug.LogError("cardPrefab est null !");
            return;
        }

        GameObject cardGO = Instantiate(cardPrefab, caseView.transform, false);
        CardView cardView = cardGO.GetComponent<CardView>();
        if (cardView == null)
        {
            Debug.LogError("Le prefab de carte n'a pas de CardView !");
            return;
        }

        RectTransform cardRect = cardGO.GetComponent<RectTransform>();
        if (cardRect != null)
        {
            cardRect.anchorMin = new Vector2(0.5f, 0.5f);
            cardRect.anchorMax = new Vector2(0.5f, 0.5f);
            cardRect.sizeDelta = gridLayout.cellSize;
            cardRect.anchoredPosition = Vector2.zero;
            cardRect.localScale = Vector3.one;
        }

        cardView.Init(cardModel);
    }

    public bool TryGetCardViewAt(int x, int y, out CardView cardView)
    {
        cardView = null;

        if (_caseViews == null)
            return false;

        if (x < 0 || x >= _width || y < 0 || y >= _height)
            return false;

        CaseView caseView = _caseViews[y, x];
        if (caseView == null)
            return false;

        cardView = caseView.GetComponentInChildren<CardView>(includeInactive: true);
        return cardView != null;
    }

    private void UpdateCellSize()
    {
        float cellWidth = (rectTranform.rect.width - (_width + 1) * margin) / _width;
        float cellHeight = (rectTranform.rect.height - (_height + 1) * margin) / _height;

        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
        gridLayout.spacing = new Vector2(margin, margin);
        gridLayout.padding.left = gridLayout.padding.right = gridLayout.padding.top = gridLayout.padding.bottom = (int)margin;
    }
}
