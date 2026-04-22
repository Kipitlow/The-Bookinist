using UnityEngine;

public class ScriptPotNectar : MonoBehaviour
{
    private SpriteRenderer _sprite;
    [SerializeField] private Sprite[] _allSprite;
    [SerializeField] private Item[] _allItem;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        Debug.Log($"<color=Red> SpriteName:</color> <color=white> {_sprite.sprite.name}</color>");
    }

    private void OnCollisionEnter(Collision collision)
    {
        _sprite = GetComponent<SpriteRenderer>();
        if(collision.gameObject.GetComponent<SpriteRenderer>().sprite.name == "POT_NECTAR")                    
        {
            Sprite _newSprite = Resources.Load<Sprite>("Props/POT_2");

            switch (_sprite.sprite.name)
            {
                case "POMME copie":
                    if (_newSprite != null) collision.gameObject.GetComponent<SpriteRenderer>().sprite = _newSprite;

                    foreach (Sprite _allsprite in _allSprite)
                    {
                        if (_allsprite.name == "POT_NECTAR_POMME")
                        {
                            collision.gameObject.GetComponent<SpriteRenderer>().sprite = _allsprite;
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
                    //if (_newSprite != null) collision.gameObject.GetComponent<SpriteRenderer>().sprite = _newSprite;
                    foreach (Sprite _allsprite in _allSprite)
                    {
                        if (_allsprite.name == "POT_NECTAR_FIGUE")
                        {
                            collision.gameObject.GetComponent<SpriteRenderer>().sprite = _allsprite;
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
                    //if (_newSprite != null) collision.gameObject.GetComponent<SpriteRenderer>().sprite = _newSprite;
                    foreach (Sprite _allsprite in _allSprite)
                    {
                        if (_allsprite.name == "POT_NECTAR_ORANGE")
                        {
                            collision.gameObject.GetComponent<SpriteRenderer>().sprite = _allsprite;
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
                    //if (_newSprite != null) collision.gameObject.GetComponent<SpriteRenderer>().sprite = _newSprite;
                    foreach (Sprite _allsprite in _allSprite)
                    {
                        if (_allsprite.name == "POT_NECTAR_CITRON")
                        {
                            collision.gameObject.GetComponent<SpriteRenderer>().sprite = _allsprite;
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
        if(collision.gameObject.GetComponent<SpriteRenderer>().sprite.name == "PotMiel")                    
        {
            Sprite _newSprite = Resources.Load<Sprite>("Props/POT_2");

            switch (_sprite.sprite.name)
            {
                case "POMME copie":
                    if (_newSprite != null) collision.gameObject.GetComponent<SpriteRenderer>().sprite = _newSprite;

                    foreach (Sprite _allsprite in _allSprite)
                    {
                        if (_allsprite.name == "POT_MIEL_POMME")
                        {
                            collision.gameObject.GetComponent<SpriteRenderer>().sprite = _allsprite;
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
                    //if (_newSprite != null) collision.gameObject.GetComponent<SpriteRenderer>().sprite = _newSprite;
                    foreach (Sprite _allsprite in _allSprite)
                    {
                        if (_allsprite.name == "POT_MIEL_FIGUE")
                        {
                            collision.gameObject.GetComponent<SpriteRenderer>().sprite = _allsprite;
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
                    //if (_newSprite != null) collision.gameObject.GetComponent<SpriteRenderer>().sprite = _newSprite;
                    foreach (Sprite _allsprite in _allSprite)
                    {
                        if (_allsprite.name == "POT_MIEL_ORANGE")
                        {
                            collision.gameObject.GetComponent<SpriteRenderer>().sprite = _allsprite;
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
                    //if (_newSprite != null) collision.gameObject.GetComponent<SpriteRenderer>().sprite = _newSprite;
                    foreach (Sprite _allsprite in _allSprite)
                    {
                        if (_allsprite.name == "POT_MIEL_CITRON")
                        {
                            collision.gameObject.GetComponent<SpriteRenderer>().sprite = _allsprite;
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
