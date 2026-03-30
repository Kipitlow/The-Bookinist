using UnityEngine;

public class TouchDetection : MonoBehaviour
{
    Camera _cam;
    [SerializeField] private float _maxDistance = 100f;
    [SerializeField] private LayerMask _hitMask = ~0;
    [SerializeField] private InteractionRunner _interactionRunner;



    private void Awake()
    {
        _cam = Camera.main;
    }


    public void OnTouch(Vector2 screenPosition)
    {
        Ray ray = _cam.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _hitMask))
        {
            string hitlayer = hit.collider.GetComponent<SpriteRenderer>().sortingLayerName;
            int camLayer = _cam.GetComponent<CameraMovement>().layerID;
            InteractionRunner interactionRunner = hit.collider.GetComponent<InteractionRunner>();
            Debug.Log("layer objet touché : " + hitlayer + " layer cam : " + camLayer);

            InteractionContext context = new InteractionContext
            {
                instigator = null ,
                target = gameObject,
            };

            if (interactionRunner != null)
            {
                interactionRunner.TryExecute(context);
            }
        }
    }
}
