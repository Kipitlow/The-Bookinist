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
    [SerializeField] private int _defaultIndex = 2; // HUB
    [SerializeField] private GameObject _uiToDisable;
    [SerializeField] private float _animDuration = 0.35f;
    [SerializeField] private float _screenWidth = 1080f; // largeur de ton Canvas

    private int _currentIndex = -1;
    private bool _isAnimating = false;

    void Start()
    {
        for (int i = 0; i < _navItems.Length; i++)
        {
            int index = i;
            _navItems[i].button.onClick.AddListener(() => NavigateTo(index));
            _navItems[i].colorDefault = _navItems[i].button.GetComponent<Image>().color;
            _navItems[i].colorActive = _navItems[i].button.colors.selectedColor;

            // Cacher tous les panels au départ
            if (_navItems[i].panel != null)
                _navItems[i].panel.SetActive(false);
        }

        NavigateTo(_defaultIndex);
    }

    public void NavigateTo(int index)
    {
        if (index == _currentIndex || _isAnimating) return;

        // HUB (panel null) : on repasse par le home
        if (_navItems[index].panel == null)
        {
            StartCoroutine(ReturnToHub());
            return;
        }

        int hubIndex = _defaultIndex;

        // Même côté ou navigation directe ?
        bool goingRight = index > _currentIndex;
        bool currentIsHub = _currentIndex == hubIndex || _currentIndex == -1;
        bool targetIsHub = index == hubIndex;

        // Cas : on est à droite et on va à gauche (ou inversement) → repasser par le hub
        bool crossingHub = !currentIsHub && !targetIsHub &&
                           ((index < hubIndex && _currentIndex > hubIndex) ||
                            (index > hubIndex && _currentIndex < hubIndex));

        if (crossingHub)
        {
            StartCoroutine(CrossHubThenNavigate(index));
            return;
        }

        // Navigation directe
        StartCoroutine(SlideToPanel(index, goingRight));
    }

    // Navigation simple : slide du panel entrant par dessus le courant
    private IEnumerator SlideToPanel(int targetIndex, bool fromRight)
    {
        _isAnimating = true;

        NavItem target = _navItems[targetIndex];
        float startX = fromRight ? _screenWidth : -_screenWidth;

        // Préparer le panel entrant
        target.panel.SetActive(true);
        RectTransform targetRect = target.panel.GetComponent<RectTransform>();
        targetRect.anchoredPosition = new Vector2(startX, targetRect.anchoredPosition.y);

        // Slide in (par dessus, pas besoin de bouger le panel actuel)
        yield return targetRect
            .DOAnchorPosX(0f, _animDuration)
            .SetEase(Ease.OutCubic)
            .WaitForCompletion();

        // Cacher l'ancien panel
        if (_currentIndex >= 0 && _navItems[_currentIndex].panel != null)
            _navItems[_currentIndex].panel.SetActive(false);

        UpdateButtonStates(targetIndex);
        _uiToDisable.SetActive(false);
        _currentIndex = targetIndex;
        _isAnimating = false;
    }

    // Retour au HUB : retrait séquentiel des panels empilés
    private IEnumerator ReturnToHub()
    {
        _isAnimating = true;

        // Construire la pile des panels visibles entre hub et currentIndex
        List<int> stack = GetPanelStack();

        // Rétracter séquentiellement de la surface vers le hub
        for (int i = stack.Count - 1; i >= 0; i--)
        {
            NavItem item = _navItems[stack[i]];
            if (item.panel == null || !item.panel.activeSelf) continue;

            RectTransform rect = item.panel.GetComponent<RectTransform>();
            bool wasRight = stack[i] > _defaultIndex;
            float exitX = wasRight ? _screenWidth : -_screenWidth;

            yield return rect
                .DOAnchorPosX(exitX, _animDuration)
                .SetEase(Ease.InCubic)
                .WaitForCompletion();

            item.panel.SetActive(false);
            rect.anchoredPosition = new Vector2(0f, rect.anchoredPosition.y);
        }

        UpdateButtonStates(_defaultIndex);
        _uiToDisable.SetActive(true);
        _currentIndex = _defaultIndex;
        _isAnimating = false;
    }

    // Traversée du hub : retrait puis entrée de l'autre côté
    private IEnumerator CrossHubThenNavigate(int targetIndex)
    {
        // D'abord retour au hub
        yield return StartCoroutine(ReturnToHub());

        // Puis navigation vers la cible
        bool goingRight = targetIndex > _defaultIndex;
        yield return StartCoroutine(SlideToPanel(targetIndex, goingRight));
    }

    // Retourne les indices des panels actifs entre hub et currentIndex
    private List<int> GetPanelStack()
    {
        List<int> stack = new List<int>();

        if (_currentIndex == _defaultIndex || _currentIndex == -1)
            return stack;

        bool isRight = _currentIndex > _defaultIndex;

        if (isRight)
        {
            for (int i = _defaultIndex + 1; i <= _currentIndex; i++)
                if (_navItems[i].panel != null && _navItems[i].panel.activeSelf)
                    stack.Add(i);
        }
        else
        {
            for (int i = _defaultIndex - 1; i >= _currentIndex; i--)
                if (_navItems[i].panel != null && _navItems[i].panel.activeSelf)
                    stack.Add(i);
        }

        return stack;
    }

    private void UpdateButtonStates(int activeIndex)
    {
        for (int i = 0; i < _navItems.Length; i++)
            _navItems[i].button.Select();

        _navItems[activeIndex].button.Select();
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