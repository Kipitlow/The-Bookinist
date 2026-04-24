using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum _makeChoiceSprite { Money, Gemme, Plume }

[System.Serializable]
public class typeMoney
{
    [Header("SetCurrent")]
    public int currentMoney;
    public int nombreInstanceMoney;
    public _makeChoiceSprite choiceSprite;
    public Transform targetSpawn;
}

public class ScriptSpawnMoney : MonoBehaviour
{
    #region Variable
    [Header("SetupPrefable")]
    [SerializeField] private GameObject _prefableMoney;
    [SerializeField] private Sprite[] _allSprite;
    [SerializeField] private List<typeMoney> _typeMoney=new();

    #endregion
    #region Method Unity
    private void Start()
    {
        PlaySpawnMoney();
    }
    public void PlaySpawnMoney()
    {
        StartCoroutine(_instantiateMoney());
    }
    #endregion
    #region Unity
    private IEnumerator _instantiateMoney()
    {
        foreach (typeMoney _var in _typeMoney)
        {
            int i = 0;
            while (i < _var.nombreInstanceMoney)
            {
                switch (_var.choiceSprite)
                {
                    case _makeChoiceSprite.Money:
                        foreach (Sprite _sprite in _allSprite)
                        {
                            if (_sprite.name == "Franc")
                            {
                                GameObject _object = Instantiate(_prefableMoney, _var.targetSpawn.position, Quaternion.identity);
                                ScriptMoneyShop script = _object.GetComponent<ScriptMoneyShop>();
                                script.targetTransform = GameObject.Find("SoftMoneyImage").GetComponent<RectTransform>();
                                _object.GetComponent<SpriteRenderer>().sprite = _sprite;
                                script.play(_var.currentMoney, _sprite.name);
                            }
                        }
                        break;
                    case _makeChoiceSprite.Gemme:
                        foreach (Sprite _sprite in _allSprite)
                        {
                            if (_sprite.name == "Gemme")
                            {
                                GameObject _object = Instantiate(_prefableMoney, _var.targetSpawn.position, Quaternion.identity);
                                ScriptMoneyShop script = _object.GetComponent<ScriptMoneyShop>();
                                script.targetTransform = GameObject.Find("HardMoneyImage").GetComponent<RectTransform>();
                                _object.GetComponent<SpriteRenderer>().sprite = _sprite;
                                script.play(_var.currentMoney, _sprite.name);
                            }
                        }
                        break;
                    case _makeChoiceSprite.Plume:
                        foreach (Sprite _sprite in _allSprite)
                        {
                            if (_sprite.name == "Plume")
                            {
                                GameObject _object = Instantiate(_prefableMoney, _var.targetSpawn.position, Quaternion.identity);
                                ScriptMoneyShop script = _object.GetComponent<ScriptMoneyShop>();
                                script.targetTransform = GameObject.Find("IDunnoWhatsthis").GetComponent<RectTransform>();
                                _object.GetComponent<SpriteRenderer>().sprite = _sprite;
                                script.play(_var.currentMoney, _sprite.name);
                            }
                        }
                        break;
                }
                i++;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
    #endregion
}
