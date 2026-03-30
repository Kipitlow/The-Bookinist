using UnityEngine;

public class TouchDetection : MonoBehaviour
{

    [SerializeField] private Camera _cam;
    [SerializeField] private float _maxDistance = 100f;
    [SerializeField] private LayerMask _hitMask = ~0;



    private void Awake()
    {
        if (_cam == null)
            _cam = Camera.main;
    }


    public void OnTouch(Vector2 screenPosition)
    {
        Ray ray = _cam.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _hitMask))
        {
            string hitlayer = hit.collider.GetComponent<SpriteRenderer>().sortingLayerName;
            int camLayer = _cam.GetComponent<CameraMovement>().layerID;
            Debug.Log("layer objet touché : " + hitlayer + " layer cam : " + camLayer);

            //if (hitlayer == camLayer)
            //{
            //    Debug.Log("hitcorrect");
            //    
            //}
            

        }
    }


}
