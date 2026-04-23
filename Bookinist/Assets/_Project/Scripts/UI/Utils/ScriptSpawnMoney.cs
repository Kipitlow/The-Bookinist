using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ScriptSpawnMoney : MonoBehaviour
{
    #region Variable
    [SerializeField] private GameObject _prefableMoney;
    [SerializeField] private int _nombreInstanceMoney;
    [SerializeField] private Sprite[] _allSprite;
    [SerializeField] private _makeChoiceSprite _choiceSprite;
    enum _makeChoiceSprite{Money,Gemme,Plume}
    #endregion
    #region Method Unity
    private void Start()
    {
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
                            _object.GetComponent<ScriptMoneyShop>().targetTransform = GameObject.Find("SoftMoneyImage").GetComponent<RectTransform>();
                            _object.GetComponent<SpriteRenderer>().sprite = _sprite;
                        }
                    }
                    break;
                case _makeChoiceSprite.Gemme:
                    foreach (Sprite rr in _allSprite)
                    {
                        if (rr.name == "Gemme")
                        {
                            GameObject _object = Instantiate(_prefableMoney, this.transform.position, Quaternion.identity);
                            _object.GetComponent<SpriteRenderer>().sprite = rr;
                        }
                    }
                    break;
                case _makeChoiceSprite.Plume:
                    foreach (Sprite rr in _allSprite)
                    {
                        if (rr.name == "Plume")
                        {
                            GameObject _object = Instantiate(_prefableMoney, this.transform.position, Quaternion.identity);
                            _object.GetComponent<SpriteRenderer>().sprite = rr;
                        }
                    }
                    break;
            }
            i++;
            yield return new WaitForSeconds(0.1f);
        }

    }
    #endregion
}
