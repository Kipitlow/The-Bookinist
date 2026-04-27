using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
    [SerializeField] private List<typeMoney> _typeMoney=new();

    #endregion
    
    #region Unity
    public void PeuImporte()
    {
        StartCoroutine(_instantiateMoney());
    }
    
    private IEnumerator _instantiateMoney()
    {
        foreach (typeMoney _var in _typeMoney)
        {
            int i = 0;
            while (i < _var.nombreInstanceMoney)
            {
                GameObject _object = Instantiate(_prefableMoney, _var.targetSpawn.position, Quaternion.identity);
                GameObject _canvas = GameObject.Find("Canvas");
                _object.transform.SetParent(_canvas.transform);
                switch (_var.choiceSprite)
                {
                    case _makeChoiceSprite.Money:
                        _object.GetComponent<ScriptMoneyShop>().play(_var.currentMoney, "Franc");
                        //Debug.Log(_object.GetComponent<ScriptMoneyShop>().GetRectTransform());
                        break;
                    case _makeChoiceSprite.Gemme:
                        _object.GetComponent<ScriptMoneyShop>().play(_var.currentMoney, "Gemme");
                        break;
                    case _makeChoiceSprite.Plume:
                        _object.GetComponent<ScriptMoneyShop>().play(_var.currentMoney, "Plume");
                        break;
                }
                i++;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
    #endregion
}
