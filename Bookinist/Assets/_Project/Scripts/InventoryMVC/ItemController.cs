using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemController : MonoBehaviour/*, IDragHandler, IBeginDragHandler, IEndDragHandler*/
{
    #region Variables

    [SerializeField] private ItemModel _itemModel;
    [SerializeField] private ItemView _itemView;
    [SerializeField] private bool _itemIsSelected;
    public static Action<GameObject> onItemClicked;
    //[SerializeField] private GameObject _EventManager;
    //private RectTransform _rectTransform;
    //private Image _image;


    #endregion

    #region Unity Methods
    private void Awake()
    {
        _itemView.UpdateSprite(_itemModel.itemSprite);
    }

    private void Start()
    {
        //_rectTransform = GetComponent<RectTransform>();
        //_image = GetComponent<Image>(); 
    }

    #endregion

    #region Methods

    public void UpdateItem()
    {
        _itemView.UpdateSprite(_itemModel.itemSprite);
    }

    
    public void OnZoobyClick()
    {
        Debug.Log("Event called OnClick, with item " + gameObject);
        onItemClicked?.Invoke(gameObject);
    }
    /*
    public void OnDrag(PointerEventData eventData)
    {
       //_rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //_itemModel.EventManager.OnDragEnded?.Invoke(this);
    }
    */
    #endregion
}
