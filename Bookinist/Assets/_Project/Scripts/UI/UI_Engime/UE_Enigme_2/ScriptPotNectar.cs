using UnityEngine;

public class ScriptPotNectar : MonoBehaviour
{
    private SpriteRenderer _sprite;
    [SerializeField] private Sprite[] _allSprite;
    [SerializeField] private Item[] _allItem;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //Debug.Log(transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _sprite = GetComponent<SpriteRenderer>();
        SpriteRenderer _otherSprite = collision.gameObject.GetComponent<SpriteRenderer>();
        if (_otherSprite == null) return;

        if (_otherSprite.sprite.name == "POT_NECTAR")                    
        {
            Sprite _newSprite = Resources.Load<Sprite>("Props/POT_2");

            switch (_sprite.sprite.name)
            {
                case "POMME copie":
                    if (_newSprite != null) _otherSprite.sprite = _newSprite;

                    foreach (Sprite _allsprite in _allSprite)
                    {
                        if (_allsprite.name == "POT_NECTAR_POMME")
                        {
                            _otherSprite.sprite = _allsprite;
                        }
                    }
                    foreach(Item _allitem in _allItem)
                    {
                        if( _allitem.name == "Pot Nectar Pomme")
                        {
                            collision.gameObject.GetComponent <Pickable>().SetItem(_allitem);
                        }
                    }
                    Destroy(gameObject,0.1f);
                    break;
                case "FIGUE copie":
                    //Debug.Log("<color=green> [Sucess Critique] <color=white> La FIGUE est bien était nommée</color>");
                    //if (_newSprite != null) _otherSprite.sprite = _newSprite;
                    foreach (Sprite _allsprite in _allSprite)
                    {
                        if (_allsprite.name == "POT_NECTAR_FIGUE")
                        {
                            _otherSprite.sprite = _allsprite;
                        }
                    }
                    foreach (Item _allitem in _allItem)
                    {
                        if (_allitem.name == "Pot Nectar Figue")
                        {
                            collision.gameObject.GetComponent<Pickable>().SetItem(_allitem);
                        }
                    }
                    Destroy(gameObject,0.1f);
                    break;
                case "ORANGE copie":
                    //Debug.Log("<color=green> [Sucess Critique] <color=white> La ORANGE est bien était nommée</color>");
                    //if (_newSprite != null) _otherSprite.sprite = _newSprite;
                    foreach (Sprite _allsprite in _allSprite)
                    {
                        if (_allsprite.name == "POT_NECTAR_ORANGE")
                        {
                            _otherSprite.sprite = _allsprite;
                        }
                    }
                    foreach (Item _allitem in _allItem)
                    {
                        if (_allitem.name == "Pot Nectar Orange")
                        {
                            collision.gameObject.GetComponent<Pickable>().SetItem(_allitem);
                        }
                    }
                    Destroy(gameObject,0.1f);
                    break;
                case "CITRON copie":
                    //Debug.Log("<color=green> [Sucess Critique] <color=white> Le CITRON est bien était nommée</color>");
                    //if (_newSprite != null) _otherSprite.sprite = _newSprite;
                    foreach (Sprite _allsprite in _allSprite)
                    {
                        if (_allsprite.name == "POT_NECTAR_CITRON")
                        {
                            _otherSprite.sprite = _allsprite;
                        }
                    }
                    foreach (Item _allitem in _allItem)
                    {
                        if (_allitem.name == "Pot Nectar Citron")
                        {
                            collision.gameObject.GetComponent<Pickable>().SetItem(_allitem);
                        }
                    }
                    Destroy(gameObject,0.1f);
                    break;
            }
        }
        if(_otherSprite.sprite.name == "PotMiel")                    
        {
            Sprite _newSprite = Resources.Load<Sprite>("Props/POT_2");

            switch (_sprite.sprite.name)
            {
                case "POMME copie":
                    if (_newSprite != null) _otherSprite.sprite = _newSprite;

                    foreach (Sprite _allsprite in _allSprite)
                    {
                        if (_allsprite.name == "POT_MIEL_POMME")
                        {
                            _otherSprite.sprite = _allsprite;
                        }
                    }
                    foreach (Item _allitem in _allItem)
                    {
                        if (_allitem.name == "Pot Miel Pomme")
                        {
                            collision.gameObject.GetComponent<Pickable>().SetItem(_allitem);
                        }
                    }
                    Destroy(gameObject,0.1f);
                    break;
                case "FIGUE copie":
                    //Debug.Log("<color=green> [Sucess Critique] <color=white> La FIGUE est bien était nommée</color>");
                    //if (_newSprite != null) _otherSprite.sprite = _newSprite;
                    foreach (Sprite _allsprite in _allSprite)
                    {
                        if (_allsprite.name == "POT_MIEL_FIGUE")
                        {
                            _otherSprite.sprite = _allsprite;
                        }
                    }
                    foreach (Item _allitem in _allItem)
                    {
                        if (_allitem.name == "Pot Miel Figue")
                        {
                            collision.gameObject.GetComponent<Pickable>().SetItem(_allitem);
                        }
                    }
                    Destroy(gameObject,0.1f);
                    break;
                case "ORANGE copie":
                    //Debug.Log("<color=green> [Sucess Critique] <color=white> La ORANGE est bien était nommée</color>");
                    //if (_newSprite != null) _otherSprite.sprite = _newSprite;
                    foreach (Sprite _allsprite in _allSprite)
                    {
                        if (_allsprite.name == "POT_MIEL_ORANGE")
                        {
                            _otherSprite.sprite = _allsprite;
                        }
                    }
                    foreach (Item _allitem in _allItem)
                    {
                        if (_allitem.name == "Pot Miel Orange")
                        {
                            collision.gameObject.GetComponent<Pickable>().SetItem(_allitem);
                        }
                    }
                    Destroy(gameObject,0.1f);
                    break;
                case "CITRON copie":
                    //Debug.Log("<color=green> [Sucess Critique] <color=white> Le CITRON est bien était nommée</color>");
                    //if (_newSprite != null) _otherSprite.sprite = _newSprite;
                    foreach (Sprite _allsprite in _allSprite)
                    {
                        if (_allsprite.name == "POT_MIEL_CITRON")
                        {
                            _otherSprite.sprite = _allsprite;
                        }
                    }
                    foreach (Item _allitem in _allItem)
                    {
                        if (_allitem.name == "Pot Miel Citron")
                        {
                            collision.gameObject.GetComponent<Pickable>().SetItem(_allitem);
                        }
                    }
                    Destroy(gameObject,0.1f);
                    break;
            }
        }
    }
}
