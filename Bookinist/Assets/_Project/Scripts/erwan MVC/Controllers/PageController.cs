using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private BoardView boardView;
    [SerializeField] private GameController gameController;

    private void Awake()
    {
        boardView.OnCaseClicked += gameController.OnCaseSelected;
    }

    private void OnDestroy()
    {
        boardView.OnCaseClicked -= gameController.OnCaseSelected;
    }

    private void Start()
    {
        boardView.InitBoard(gameController.HorizontalCaseNumber,
            gameController.VerticalCaseNumber);
    }
}
