using System.Collections;
using UnityEngine;


enum _makeChoiceSprite { Money, Gemme, Plume }

public class typeMoney
{
    
}

public class ScriptSpawnMoney : MonoBehaviour
{
    #region Variable
    [Header("SetupPrefable")]
    [SerializeField] private GameObject _prefableMoney;
    [SerializeField] private Sprite[] _allSprite;
    [SerializeField] private _makeChoiceSprite _choiceSprite;

    [Header("SetCurrent")]
    [SerializeField] private int _currentMoney;
    private int _nombreInstanceMoney;
    #endregion
    #region Method Unity
    private void Start()
    {
        switch (_choiceSprite)
        {
            case _makeChoiceSprite.Money: _nombreInstanceMoney = 10; break;
            case _makeChoiceSprite.Gemme: _nombreInstanceMoney = 8; break;
            case _makeChoiceSprite.Plume: _nombreInstanceMoney = 5; break;
        }
        StartCoroutine(_instantiateMoney());
    }
    #endregion
    #region Unity
    private IEnumerator _instantiateMoney()
    {
        int i = 0;
        while(i< _nombreInstanceMoney)
        {
            switch(_choiceSprite)
            {
                case _makeChoiceSprite.Money:
                    foreach(Sprite _sprite in _allSprite)
                    {
                        if (_sprite.name == "Franc")
                        {
                            GameObject _object = Instantiate(_prefableMoney, this.transform.position, Quaternion.identity);
                            ScriptMoneyShop script = _object.GetComponent<ScriptMoneyShop>();
                            script.targetTransform = GameObject.Find("SoftMoneyImage").GetComponent<RectTransform>();
                            _object.GetComponent<SpriteRenderer>().sprite = _sprite;
                            script.play(_currentMoney, _sprite.name);
                        }
                    }
                    break;
                case _makeChoiceSprite.Gemme:
                    foreach (Sprite _sprite in _allSprite)
                    {
                        if (_sprite.name == "Gemme")
                        {
                            GameObject _object = Instantiate(_prefableMoney, this.transform.position, Quaternion.identity);
                            ScriptMoneyShop script = _object.GetComponent<ScriptMoneyShop>();
                            script.targetTransform = GameObject.Find("HardMoneyImage").GetComponent<RectTransform>();
                            _object.GetComponent<SpriteRenderer>().sprite = _sprite;
                            script.play(_currentMoney, _sprite.name);
                        }
                    }
                    break;
                case _makeChoiceSprite.Plume:
                    foreach (Sprite _sprite in _allSprite)
                    {
                        if (_sprite.name == "Plume")
                        {
                            GameObject _object = Instantiate(_prefableMoney, this.transform.position, Quaternion.identity);
                            ScriptMoneyShop script = _object.GetComponent<ScriptMoneyShop>();
                            script.targetTransform = GameObject.Find("IDunnoWhatsthis").GetComponent<RectTransform>();
                            _object.GetComponent<SpriteRenderer>().sprite = _sprite;
                            script.play(_currentMoney, _sprite.name);
                        }
                    }
                    break;
            }
            i++;
            yield return new WaitForSeconds(0.05f);
        }

    }
    #endregion
}
