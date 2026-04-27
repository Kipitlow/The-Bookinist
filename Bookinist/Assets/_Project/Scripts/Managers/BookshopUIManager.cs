using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BookshopUIManager : MonoBehaviour
{
    [System.Serializable]
    public class NavItem
    {
        public string name;
        public Button button;
        public GameObject panel;
        public Sprite iconDefault;
        public Sprite iconActive;
        [HideInInspector] public Color colorDefault;
        [HideInInspector] public Color colorActive;
    }

    [SerializeField] private NavItem[] _navItems;
    [SerializeField] private int _defaultIndex = 2;
    [SerializeField] private GameObject _uiToDisable;
    [SerializeField] private float _animDuration = 0.35f;
    [SerializeField] private Canvas _canvas;

    [Header("Managers")]
    [SerializeField] private CustomShopManager _customShopManager;
    [SerializeField] private CamManager _camManager;

    private float _screenWidth;
    private int _currentIndex = -1;
    private bool _isAnimating = false;
    private List<int> _panelStack = new List<int>();

    private GameObject _npcToDisable;

    void Start()
    {
        _npcToDisable = FindFirstObjectByType<NPCTalker>().gameObject;

        _screenWidth = _canvas.GetComponent<RectTransform>().rect.width;

        for (int i = 0; i < _navItems.Length; i++)
        {
            int index = i;
            _navItems[i].button.onClick.AddListener(() => NavigateTo(index));
            _navItems[i].colorDefault = _navItems[i].button.GetComponent<Image>().color;
            _navItems[i].colorActive = _navItems[i].button.colors.selectedColor;

            if (_navItems[i].panel != null)
                _navItems[i].panel.SetActive(false);
        }

        NavigateTo(_defaultIndex);
    }

    public void NavigateTo(int index)
    {
        if (index == _currentIndex || _isAnimating) return;
        StartCoroutine(HandleNavigation(index));
    }

    private IEnumerator HandleNavigation(int targetIndex)
    {
        _isAnimating = true;

        bool targetIsHub = _navItems[targetIndex].panel == null;
        bool currentIsHub = _currentIndex == _defaultIndex || _currentIndex == -1;
        bool targetIsRight = targetIndex > _defaultIndex;
        bool currentIsRight = _currentIndex > _defaultIndex;

        SwitchButtonActivation(targetIndex);


        if (targetIsHub)
        {
            // Retour au HUB : rétracte toute la pile
            yield return StartCoroutine(RetractStack());
            _uiToDisable.SetActive(true);

            _npcToDisable.SetActive(true);
            _camManager.ResetToView(0);
        }
        else if (currentIsHub)
        {
            _npcToDisable.SetActive(false);

            _camManager.ResetToView(0);
            _customShopManager.ResetPanelToCurrentView();

            // Depuis le HUB : empile séquentiellement jusqu'à la cible
            yield return StartCoroutine(PushUntil(targetIndex));

        }
        else if (targetIsRight == currentIsRight)
        {
            if (IsForward(targetIndex))
            {
                // Même côté, on avance : empile les panels intermédiaires
                yield return StartCoroutine(PushUntil(targetIndex));
            }
            else
            {
                // Même côté, on recule : dépile jusqu'à la cible
                yield return StartCoroutine(RetractUntil(targetIndex));
            }
        }
        else
        {
            // Changement de côté : rétracte tout puis empile de l'autre côté
            yield return StartCoroutine(RetractStack());
            _uiToDisable.SetActive(false);
            _camManager.ResetToView(0);
            yield return StartCoroutine(PushUntil(targetIndex));
        }

        _navItems[targetIndex].button.Select();
        _currentIndex = targetIndex;
        _isAnimating = false;
    }

    private void SwitchButtonActivation(int targetIndex)
    {
        if (_currentIndex == -1) return;

        _navItems[_currentIndex].button.interactable = true;
        _navItems[targetIndex].button.interactable = false;
    }

    // Empile séquentiellement tous les panels entre le sommet actuel et la cible
    private IEnumerator PushUntil(int targetIndex)
    {
        bool goRight = targetIndex > _defaultIndex;

        // Détermine le point de départ (panel suivant après le sommet de la pile)
        int startIndex;
        if (_panelStack.Count == 0)
            startIndex = goRight ? _defaultIndex + 1 : _defaultIndex - 1;
        else
            startIndex = goRight ? _panelStack[_panelStack.Count - 1] + 1
                                 : _panelStack[_panelStack.Count - 1] - 1;

        if (goRight)
        {
            for (int i = startIndex; i <= targetIndex; i++)
            {
                yield return StartCoroutine(PushPanel(i, true));
            }
        }
        else
        {
            for (int i = startIndex; i >= targetIndex; i--)
            {
                yield return StartCoroutine(PushPanel(i, false));
            }
        }
    }

    // Pousse un seul panel par dessus
    private IEnumerator PushPanel(int targetIndex, bool fromRight)
    {
        NavItem target = _navItems[targetIndex];
        if (target.panel == null) yield break;

        RectTransform rect = target.panel.GetComponent<RectTransform>();
        float startX = fromRight ? _screenWidth : -_screenWidth;
        rect.anchoredPosition = new Vector2(startX, rect.anchoredPosition.y);
        target.panel.SetActive(true);
        _uiToDisable.SetActive(false);

        yield return rect
            .DOAnchorPosX(0f, _animDuration)
            .SetEase(Ease.OutCubic)
            .WaitForCompletion();

        _panelStack.Add(targetIndex);
    }

    // Rétracte séquentiellement jusqu'à exposer targetIndex (il doit déjà être dans la pile)
    private IEnumerator RetractUntil(int targetIndex)
    {
        while (_panelStack.Count > 0 && _panelStack[_panelStack.Count - 1] != targetIndex)
        {
            int top = _panelStack[_panelStack.Count - 1];
            _panelStack.RemoveAt(_panelStack.Count - 1);
            yield return StartCoroutine(PopPanel(top));
        }
    }

    // Rétracte toute la pile jusqu'au HUB
    private IEnumerator RetractStack()
    {
        while (_panelStack.Count > 0)
        {
            int top = _panelStack[_panelStack.Count - 1];
            _panelStack.RemoveAt(_panelStack.Count - 1);
            yield return StartCoroutine(PopPanel(top));
        }
    }

    // Fait sortir un panel du côté dont il est venu
    private IEnumerator PopPanel(int index)
    {
        NavItem item = _navItems[index];
        if (item.panel == null || !item.panel.activeSelf) yield break;

        RectTransform rect = item.panel.GetComponent<RectTransform>();
        bool isRight = index > _defaultIndex;
        float exitX = isRight ? _screenWidth : -_screenWidth;

        yield return rect
            .DOAnchorPosX(exitX, _animDuration)
            .SetEase(Ease.InCubic)
            .WaitForCompletion();

        item.panel.SetActive(false);
        rect.anchoredPosition = new Vector2(0f, rect.anchoredPosition.y);
    }

    private bool IsForward(int targetIndex)
    {
        bool targetIsRight = targetIndex > _defaultIndex;
        return targetIsRight ? targetIndex > _currentIndex : targetIndex < _currentIndex;
    }

    public void NavigateTo(string panelName)
    {
        for (int i = 0; i < _navItems.Length; i++)
        {
            if (_navItems[i].name == panelName)
            {
                NavigateTo(i);
                return;
            }
        }
        Debug.LogWarning($"NavBar : panel '{panelName}' introuvable.");
    }
}