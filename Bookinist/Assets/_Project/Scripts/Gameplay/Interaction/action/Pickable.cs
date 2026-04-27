using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(InteractionRunner))]

public class Pickable : MonoBehaviour
{
    [SerializeField] private Item _item;
    [SerializeField] private InventoryController _invController;
    [SerializeField] private GameObject _particles;

    private void Start()
    {
        if (_invController == null) _invController = GameObject.Find("InventoryManager").GetComponent<InventoryController>();
    }

    public void SetItem(Item item) {  _item = item; }

    public void Pick(GameObject objetclicked)
    {
        if (_invController.IsInventoryHasPlace())
        {
            Destroy(objetclicked);
            _invController.AddInventoryItem(_item);
            SpawnParticles();
        }
    }


    public void SpawnParticles()
    {
        Debug.Log("Particles");
        GameObject ps = Instantiate(_particles, gameObject.transform.position, Quaternion.identity);
        if (gameObject.GetComponent<SpriteRenderer>() == null)
        {
            ps.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerID = gameObject.GetComponentInChildren<SpriteRenderer>().sortingLayerID;
        }
        else
        {
            ps.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerID = gameObject.GetComponent<SpriteRenderer>().sortingLayerID;
        }
        ps.GetComponent<ParticleSystem>().Play();
    }
}
