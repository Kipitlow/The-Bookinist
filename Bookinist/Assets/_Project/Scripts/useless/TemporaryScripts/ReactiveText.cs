using TMPro;
using UnityEngine;

public class ReactiveText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textVal;

    private void Start()
    {
        textUpdate();
    }

    public void textUpdate()
    {
        textVal.text = SaveData.instance.player.playerName;
    }
}
