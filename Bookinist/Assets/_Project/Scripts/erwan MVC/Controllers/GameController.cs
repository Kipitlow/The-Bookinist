using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Grid")]
    [Header("")]
    [Range(4, 10)]
    [SerializeField] private int _horizontalCaseNumber;
    [Range(4, 10)]
    [SerializeField] private int _verticalCaseNumber;

    [Header("Game Settings")]
    [Header("")]
    [Range(1, 8)]
    [SerializeField] private int _startSlotNumber;
    [Range(1, 8)]
    [SerializeField] private int _startCardNumber;
    [Range(1, 100)]
    [SerializeField] private float _maxTime;
    [Range(1, 100)]
    [SerializeField] private float _closeTime;
    [Range(80, 1000)]
    [SerializeField] private int _powerToWinGame;

    [Header("Controllers")]
    [Header("")]
    [SerializeField] private InventoryController _inventoryControllerPlayer1;
    [SerializeField] private InventoryController _inventoryControllerPlayer2;

    [Header("Sounds")]
    [Header("")]
    [SerializeField] AudioClip _timeCloseSound;
    [SerializeField] AudioClip _turnPlayerOneSound;
    [SerializeField] AudioClip _turnPlayerTwoSound;
    [SerializeField] AudioClip _takeOpponentCardSound;
    [SerializeField] AudioClip _clickCardSound;
    [SerializeField] AudioClip _clickCaseSound;
    [SerializeField] AudioClip _winSound;
    [SerializeField] AudioClip[] _demonSounds;
    [SerializeField] AudioClip[] _orcSounds;

    [Header("Other")]
    [Header("")]
    [SerializeField] private BoardView _boardView;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private GameView _gameView;
    [SerializeField] private TextMeshProUGUI _winText;
    [SerializeField] private GameObject _winPanel;

    [Header("FX YourTurn")]
    [Header("")]
    [SerializeField] private Animator _yourTurnFxAnimator;

    [Header("FX Timer")]
    [Header("")]
    [SerializeField] private Animator _timerFxAnimator;

    [Tooltip("Temps (en secondes) avant la fin du tour considéré comme critique. Par défaut: 10 dernières secondes.")]
    [Min(0f)]
    [SerializeField] private float _criticalTimeWindowSeconds = 10f;

    private const string YourTurnTriggerPlayer1 = "Player1";
    private const string YourTurnTriggerPlayer2 = "Player2";
    private const string TimerTriggerReachingTime = "ReachingTime";
    private const string TimerTriggerEndTurn = "EndTurn";

    private CardModel _selectedCard;
    private GameModel _gameModel;

    private bool isTimeClose = false;
    private bool? _lastIsPlayerOneTurnFx;

    private bool _hasTriggeredCriticalTimeThisTurn;

    public int HorizontalCaseNumber => _horizontalCaseNumber;
    public int VerticalCaseNumber => _verticalCaseNumber;

    private Coroutine _turnCoroutine;

    private void Awake()
    {
        Time.timeScale = 1.0f;

        _gameModel = new GameModel(
            _horizontalCaseNumber,
            _verticalCaseNumber,
            _startSlotNumber,
            _startCardNumber,
            _inventoryControllerPlayer1.CardDataList,
            _inventoryControllerPlayer2.CardDataList,
            _maxTime,
            _powerToWinGame
            );

        if (_inventoryControllerPlayer1 != null)
        {
            _inventoryControllerPlayer1.Init(_gameModel.InventoryModelPlayer1);
        }
        else
        {
            return;
        }

        if (_inventoryControllerPlayer2 != null)
        {
            _inventoryControllerPlayer2.Init(_gameModel.InventoryModelPlayer2);
        }
        else
        {
            return;
        }

        float randomTurnDistrib = Random.Range(0.0f, 1.0f);
        float halfProba = 0.5f;

        if (randomTurnDistrib < halfProba)
            _gameModel.SetIsPlayerOneTurn(false);
        else
            _gameModel.SetIsPlayerOneTurn(true);

        _gameModel.OnWinGame += OnWinGame;
        _gameModel.OnAddPower += OnAddScore;
        _gameModel.OnOpponentCardDrawn += CallUpdateInventoryOpponentDraw;
        _gameModel.OnCardDrawn += CallUpdateInventoryDraw;

        if (_gameModel.BoardModel != null)
        {
            _gameModel.BoardModel.OnCardCaptured += HandleCardCapturedFx;
            _gameModel.BoardModel.OnCardCapturedSound += PlayCaptureSound;
        }
    }

    private void PlayCaptureSound()
    {
        SFXManager.Instance.PlaySFXClip(_takeOpponentCardSound, transform, 0.3f);
    }

    private void OnDestroy()
    {
        if (_gameModel != null && _gameModel.BoardModel != null)
            _gameModel.BoardModel.OnCardCaptured -= HandleCardCapturedFx;
    }

    private void HandleCardCapturedFx(int x, int y, bool isNowPlayer1)
    {
        if (_boardView == null)
            return;

        if (_boardView.TryGetCardViewAt(x, y, out CardView cardView))
        {
            cardView.PlayCaptureFx();
        }
    }

    private void CallUpdateInventoryOpponentDraw(bool isOpponentIsPlayerOne)
    {
        if (isOpponentIsPlayerOne)
            _inventoryControllerPlayer1.UpdateInventoryView(_gameModel.InventoryModelPlayer1);
        else
            _inventoryControllerPlayer2.UpdateInventoryView(_gameModel.InventoryModelPlayer2);
    }

    private void CallUpdateInventoryDraw()
    {
        if (_gameModel.IsPlayerOneTurn)
            _inventoryControllerPlayer1.UpdateInventoryView(_gameModel.InventoryModelPlayer1);
        else
            _inventoryControllerPlayer2.UpdateInventoryView(_gameModel.InventoryModelPlayer2);
    }

    private void OnAddScore(bool isPlayerOne, int power)
    {
        _gameView.UpdatePlayerPower(isPlayerOne, power);
    }

    private void OnWinGame(bool isPlayerOne)
    {
        if (isPlayerOne)
        {
            _winText.text = "Player 1 win";
        }
        else
        {
            _winText.text = "Player 2 win";
        }

        _winPanel.SetActive(true);
        SFXManager.Instance.PlaySFXClip(_winSound, transform, 1.0f);
        Time.timeScale = 0.0f;
    }

    private void Start()
    {
        _gameView.Init(_powerToWinGame, _gameModel.PlayerOnePower, _gameModel.PlayerTwoPower);
        StartTurn();
    }

    private void StartTurn()
    {
        // Sécurité : stop l'ancien timer
        if (_turnCoroutine != null)
            StopCoroutine(_turnCoroutine);

            // Reset UI / sélection
            _gameModel.SelectedCard = null;
        _inventoryControllerPlayer1.ClearSelection();
        _inventoryControllerPlayer2.ClearSelection();

        _inventoryControllerPlayer1.SetInteractable(_gameModel.IsPlayerOneTurn);
        _inventoryControllerPlayer2.SetInteractable(!_gameModel.IsPlayerOneTurn);

        PlayYourTurnFx(_gameModel.IsPlayerOneTurn);

        // Important: on NE déclenche PAS EndTurn ici, sinon l'Animator part directement en Transition.
        ResetTimerFxForNewTurn();

        _turnCoroutine = StartCoroutine(TurnTimer());
    }

    private void ResetTimerFxForNewTurn()
    {
        _hasTriggeredCriticalTimeThisTurn = false;

        if (_timerFxAnimator == null)
            return;

        // On repart dans un état clean pour le nouveau tour
        _timerFxAnimator.ResetTrigger(TimerTriggerReachingTime);
        _timerFxAnimator.ResetTrigger(TimerTriggerEndTurn);
    }

    private void TriggerEndTurnFx()
    {
        if (_timerFxAnimator == null)
            return;

        // Petit trigger de reset à la fin du tour
        _timerFxAnimator.ResetTrigger(TimerTriggerReachingTime);
        _timerFxAnimator.ResetTrigger(TimerTriggerEndTurn);
        _timerFxAnimator.SetTrigger(TimerTriggerEndTurn);
    }

    private void TriggerCriticalTimeFx()
    {
        if (_timerFxAnimator == null)
            return;

        if (_hasTriggeredCriticalTimeThisTurn)
            return;

        _hasTriggeredCriticalTimeThisTurn = true;

        _timerFxAnimator.ResetTrigger(TimerTriggerEndTurn);
        _timerFxAnimator.ResetTrigger(TimerTriggerReachingTime);
        _timerFxAnimator.SetTrigger(TimerTriggerReachingTime);
    }

    private IEnumerator TurnTimer()
    {
        float remaining = _gameModel.MaxTime;

        float criticalThreshold = Mathf.Min(_criticalTimeWindowSeconds, _gameModel.MaxTime);

        while (remaining > 0f)
        {
            remaining -= Time.deltaTime;
            _gameView.UpdateTimer(remaining);

            if (isTimeClose == false && remaining <= _closeTime)
            {
                isTimeClose = true;
                //animate
                SFXManager.Instance.PlaySFXClip(_timeCloseSound, transform, 1.0f);
            }

            if (!_hasTriggeredCriticalTimeThisTurn && remaining <= criticalThreshold)
            {
                TriggerCriticalTimeFx();
            }

            yield return null;
        }

        isTimeClose = false;
        TriggerEndTurnFx();

        yield return null;
        _gameModel.ChangePlayerTurn();
        StartTurn();
    }
    private void PlayYourTurnFx(bool isPlayerOneTurn)
    {
        if (_yourTurnFxAnimator == null)
            return;

        if (_lastIsPlayerOneTurnFx.HasValue && _lastIsPlayerOneTurnFx.Value == isPlayerOneTurn)
            return;

        _lastIsPlayerOneTurnFx = isPlayerOneTurn;

        _yourTurnFxAnimator.ResetTrigger(YourTurnTriggerPlayer1);
        _yourTurnFxAnimator.ResetTrigger(YourTurnTriggerPlayer2);
        _yourTurnFxAnimator.SetTrigger(isPlayerOneTurn ? YourTurnTriggerPlayer1 : YourTurnTriggerPlayer2);

        StartCoroutine(WaitPlaySoundUntilAnimMid(1.0f));
    }

    IEnumerator WaitPlaySoundUntilAnimMid(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (_gameModel.IsPlayerOneTurn)
            SFXManager.Instance.PlaySFXClip(_turnPlayerOneSound, transform, 1.0f);
        else
            SFXManager.Instance.PlaySFXClip(_turnPlayerTwoSound, transform, 0.05f);
    }

    public void OnCardSelected(CardModel card)
    {
        bool isValidSelection = (_gameModel.IsPlayerOneTurn && card.IsPlayer1Card) 
            || (!_gameModel.IsPlayerOneTurn && !card.IsPlayer1Card);

        if (!isValidSelection)
        {
            return;
        }

        _gameModel.SelectedCard = card;

        if (card.IsPlayer1Card)
            _inventoryControllerPlayer1.SelectCard(card);
        else
            _inventoryControllerPlayer2.SelectCard(card);
        SFXManager.Instance.PlaySFXClip(_clickCardSound, transform, 1.0f);
    }

    public void OnCaseSelected(int x, int y)
    {
        if (IsGameModelRefsNull())
        {
            return;
        }

        var caseModel = _gameModel.BoardModel.GetCase(x, y);
        if (caseModel == null)
        {
            return;
        }

        if (caseModel.CaseState != CaseModel.ECaseState.EMPTY)
            return;

        RemoveCardView();

        _gameModel.BoardModel.PlaceCard(x, y, caseModel, _gameModel.SelectedCard);

        

        if (_boardView != null && _cardPrefab != null)
        {
            _boardView.PlaceCardView(_gameModel.SelectedCard, x, y, _cardPrefab);
            
            if (_boardView.TryGetCardViewAt(x, y, out CardView placedCardView))
            {
                placedCardView.PlayToBoardFx();
            }
        }
        else
        {
            return;
        }

        _gameModel.AddPower(_gameModel.IsPlayerOneTurn, _gameModel.SelectedCard.CumulativPower);

        _gameModel.DrawCard();
        if (_gameModel.IsPlayerOneTurn)
        {
            int randomIndexSound = UnityEngine.Random.Range(0, _demonSounds.Length - 1);
            SFXManager.Instance.PlaySFXClip(_demonSounds[randomIndexSound], transform, 1.0f);
            
            _inventoryControllerPlayer1.DrawCard(_gameModel.InventoryModelPlayer1);
        }
        else
        {
            int randomIndexSound = UnityEngine.Random.Range(0, _orcSounds.Length - 1);
            SFXManager.Instance.PlaySFXClip(_orcSounds[randomIndexSound], transform, 1.0f);

            _inventoryControllerPlayer2.DrawCard(_gameModel.InventoryModelPlayer2);
        }

        SFXManager.Instance.PlaySFXClip(_clickCaseSound, transform, 1.0f);
        _gameModel.SelectedCard = null;

        TriggerEndTurnFx();

        StartCoroutine(ChangeTurnNextFrame());
    }

    private IEnumerator ChangeTurnNextFrame()
    {
        yield return null;
        _gameModel.ChangePlayerTurn();
        StartTurn();
    }

    private bool IsGameModelRefsNull()
    {
        if (_gameModel.SelectedCard == null)
        {
            return true;
        }

        if (_gameModel == null)
        {
            return true;
        }

        if (_gameModel.BoardModel == null)
        {
            return true;
        }

        return false;
    }

    private void RemoveCardView()
    {
        if (_gameModel.SelectedCard.IsPlayer1Card)
        {
            _gameModel.InventoryModelPlayer1.RemoveCard(_gameModel.SelectedCard);

            if (_inventoryControllerPlayer1 != null)
            {
                _inventoryControllerPlayer1.RemoveCardView(_gameModel.SelectedCard);
                _inventoryControllerPlayer1.ClearSelection();
            }
            else
            {
                return;
            }
        }
        else
        {
            _gameModel.InventoryModelPlayer2.RemoveCard(_gameModel.SelectedCard);

            if (_inventoryControllerPlayer2 != null)
            {
                _inventoryControllerPlayer2.RemoveCardView(_gameModel.SelectedCard);
                _inventoryControllerPlayer2.ClearSelection();
            }
            else
            {
                return;
            }
        }
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void NewGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}

